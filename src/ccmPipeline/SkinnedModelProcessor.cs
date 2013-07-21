#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// ShaderInstancePart.csのコメントを翻訳したもの
//
// コード変更点はMatrixの変わりにQuatTransformを使用し、
//  MaxBonesが59から118になった
//
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using SkinnedModel;
#endregion

namespace SkinnedModelPipeline
{
    /// <summary>
    /// 既存のModelProcessorを拡張して、スキンアニメーションにサポートした
    /// カスタムプロセッサ
    /// </summary>
    [ContentProcessor]
    public class SkinnedModelProcessor : ModelProcessor
    {
        // このサンプルでは頂点テクスチャを使うので最大ボーン数は256個になる
        const int MaxBones = 256;

        [DisplayName("マテリアルの使用")]
        [DefaultValue(false)]
        [Description("モデルに設定されたマテリアルパラメータをエフェクトに適用するかを指定します。")]
        public bool UseMaterial { get; set; }

        [DisplayName("テクスチャの使用")]
        [DefaultValue(false)]
        [Description("モデルに設定されたテクスチャをエフェクトに適用するかを指定します。")]
        public bool UseTexture { get; set; }
    
        /// <summary>
        /// コンテント・パイプライン内の中間データであるNodeContentから
        /// アニメーションデータを含んだModelContentへ変換するProcessメソッド。
        /// </summary>
        public override ModelContent Process(NodeContent input,
                                             ContentProcessorContext context)
        {
            ValidateMesh(input, context, null);

            // スケルトンを探す
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                throw new InvalidContentException("スケルトンが見つかりません");

            // モデル内のメッシュがそれぞれ違うローカル座標を持っていると扱いが
            // 面倒なので、全部焼いちゃう(メッシュの変換座標をあらかじめ頂点
            // データに適用すること)
            FlattenTransforms(input, skeleton);

            // バインド・ポーズとスケルトン構造を読み込む
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (bones.Count > MaxBones)
            {
                throw new InvalidContentException(string.Format(
                    "このスケルトンには{0}個のボーンがあります。最大ボーン数は{1}です。",
                    bones.Count, MaxBones));
            }

            List<QuatTransform> bindPose = new List<QuatTransform>();
            List<QuatTransform> inverseBindPose = new List<QuatTransform>();
            List<int> skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(QuatTransform.FromMatrix(bone.Transform));
                inverseBindPose.Add(QuatTransform.FromMatrix(Matrix.Invert(bone.AbsoluteTransform)));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // アニメーションデータをランタイム用フォーマットに変換する
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);

            // ベースクラスのProcessメソッドを呼び出してモデルデータを変換する
            ModelContent model = base.Process(input, context);

            // モデルのTagプロパティにカスタムアニメーションデータを設定する
            model.Tag = new SkinningData(animationClips, bindPose,
                                         inverseBindPose, skeletonHierarchy);

            return model;
        }


        /// <summary>
        /// コンテント・パイプライン内の中間フォーマットである
        /// AnimationContentDictionaryから、ランタイムフォーマットである
        /// AnimationClipフォーマットに変換する
        /// </summary>
        static Dictionary<string, AnimationClip> ProcessAnimations(
            AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // ボーン名からインデックスに変換する辞書テーブルを作る
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // それぞれのアニメーションを変換する
            Dictionary<string, AnimationClip> animationClips;
            animationClips = new Dictionary<string, AnimationClip>();

            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                AnimationClip processed = ProcessAnimation(animation.Value, boneMap);
                
                animationClips.Add(animation.Key, processed);
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException(
                            "入力ファイルはアニメーションが含まれていません。");
            }

            return animationClips;
        }


        /// <summary>
        /// コンテント・パイプライン内の中間フォーマットである
        /// AnimationContentから、ランタイムフォーマットである
        /// AnimationClipフォーマットに変換する
        /// </summary>
        static AnimationClip ProcessAnimation(AnimationContent animation,
                                              Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            // それぞれのアニメーション・チャンネルを処理する
            foreach (KeyValuePair<string, AnimationChannel> channel in
                animation.Channels)
            {
                // このチャンネルはどのボーンのものか？
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "スケルトンに属さないボーン'{0}'"+
                        "のアニメーションが見つかりました。", channel.Key));
                }

                // キーフレームの変換
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time,
                                               QuatTransform.FromMatrix(keyframe.Transform)));
                }
            }

            // キーフレームを時間順に並べ替える
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException(
                    "キーフレームの無いアニメーションです。");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException(
                    "再生時間が0のアニメーションです。");

            return new AnimationClip(animation.Duration, keyframes);
        }


        /// <summary>
        /// キーフレームを時間順に並べ替えるための比較用メソッド
        /// </summary>
        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }


        /// <summary>
        /// このメッシュはスキンアニメーションに向いたものかチェックする
        /// </summary>
        static void ValidateMesh(NodeContent node, ContentProcessorContext context,
                                 string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // メッシュの整合性を調べる
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "{0}メッシュは{1}ボーンの子です。"+
                        "SkinnedModelProcessorはメッシュがボーンの子であるケースに"+
                        "対応していません。",
                        mesh.Name, parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null,
                        "{0}メッシュはスキニング情報がないので変換しません",
                        mesh.Name);

                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // このノードがボーンなら、ボーン内を調査中であることを覚えておく
                parentBoneName = node.Name;
            }

            // 再帰的処理(調査中にノードが消去されるので、
            // 子供達のコピーを走査する)
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }


        /// <summary>
        /// メッシュがスキニング情報をもっているか調べる
        /// </summary>
        static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// 全てが同じ座標空間になるように、不必要な変換行列を
        /// モデル・ジオメトリに焼き付ける
        /// </summary>
        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // スケルトンは処理しない
                if (child == skeleton)
                    continue;

                // ローカル変換行列をジオメトリに焼きつける
                MeshHelper.TransformScene(child, child.Transform);

                // 焼き付けたので、ローカル座標変換行列は
                // 単位行列(Matrix.Identity)になる
                child.Transform = Matrix.Identity;

                // 再帰呼び出し
                FlattenTransforms(child, skeleton);
            }
        }


        /// <summary>
        /// 全てのマテリアルがスキンモデル用のエフェクトを使うように変更する
        /// </summary>
        protected override MaterialContent ConvertMaterial(MaterialContent material,
                                                        ContentProcessorContext context)
        {
            BasicMaterialContent basicMaterial = material as BasicMaterialContent;

            if (basicMaterial == null)
            {
                throw new InvalidContentException(string.Format(
                    "SkinnedModelProcessorはBasicMaterialContentのみをサポートします"+
                    "入力メッシュは{0}を使用しています。",material.GetType()));
            }

            EffectMaterialContent effectMaterial = new EffectMaterialContent();

            // スキンメッシュエフェクトを参照する
            string effectPath = Path.GetFullPath(@"Effect\SkinnedModel.fx");

            effectMaterial.Effect = new ExternalReference<EffectContent>(effectPath);

            // BasicMaterialContentのテクスチャを新しいマテリアルに設定する
            if (basicMaterial.Texture != null)
                effectMaterial.Textures.Add("Texture", basicMaterial.Texture);

            // マテリアルパラメータをエフェクトに設定
            if (basicMaterial.DiffuseColor != null)
                effectMaterial.OpaqueData.Add("MaterialDiffuse", basicMaterial.DiffuseColor);

            if (basicMaterial.EmissiveColor != null)
                effectMaterial.OpaqueData.Add("MaterialEmissive", basicMaterial.EmissiveColor);

            if (basicMaterial.SpecularColor != null)
                effectMaterial.OpaqueData.Add("MaterialSpecular", basicMaterial.SpecularColor);

            if (basicMaterial.SpecularPower != null)
                effectMaterial.OpaqueData.Add("MaterialSpecularPower", basicMaterial.SpecularPower);

            if (UseMaterial && UseTexture)
            {
                effectMaterial.OpaqueData.Add("TechniqueName", "MaterialTextureTechnique");
            }
            else if (UseMaterial)
            {
                effectMaterial.OpaqueData.Add("TechniqueName", "MaterialTechnique");
            }
            else if (UseTexture)
            {
                effectMaterial.OpaqueData.Add("TechniqueName", "TextureTechnique");
            }
            else
            {
                effectMaterial.OpaqueData.Add("TechniqueName", "BasicTechnique");
            }

            //　ModelProcessorのConvertMaterialを呼ぶ
            return base.ConvertMaterial(effectMaterial, context);
        }
    }
}
