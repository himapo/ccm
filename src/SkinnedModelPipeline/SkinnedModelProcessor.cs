#region File Description
//-----------------------------------------------------------------------------
// SkinnedModelProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using SkinnedModel;
#endregion

namespace SkinnedModelPipeline
{
    /// <summary>
    /// カスタム プロセッサは、ビルトイン フレームワーク ModelProcessor を拡張し、
    /// アニメーションのサポートを追加します。
    /// </summary>
    [ContentProcessor]
    public class SkinnedModelProcessor : ModelProcessor
    {
        /// <summary>
        /// メイン Process メソッドは、中間フォーマットの
        /// コンテンツ パイプライン NodeContent ツリーを
        /// 埋め込みアニメーション データを含む ModelContent 
        /// オブジェクトに変換します。
        /// </summary>
        public override ModelContent Process(NodeContent input,
                                             ContentProcessorContext context)
        {
            ValidateMesh(input, context, null);

            // スケルトンを見つけます。
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");

            // 別のローカル座標系にあるモデルの異なる部分について
            // 心配しなくても済むように、すべて焼き付けましょう。
            FlattenTransforms(input, skeleton);

            // バインド ポーズとスケルトンの階層データを読み込みます。
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (bones.Count > SkinnedEffect.MaxBones)
            {
                throw new InvalidContentException(string.Format(
                    "Skeleton has {0} bones, but the maximum supported is {1}.",
                    bones.Count, SkinnedEffect.MaxBones));
            }

            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // アニメーション データをランタイム形式に変換します。
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);

            // モデル データを変換できるように、基本 ModelProcessor 
            // クラスにチェーンします。
            ModelContent model = base.Process(input, context);

            // カスタム アニメーション データを、モデルの 
            // Tag プロパティに格納します。
            model.Tag = new SkinningData(animationClips, bindPose,
                                         inverseBindPose, skeletonHierarchy);

            return model;
        }


        /// <summary>
        /// 中間フォーマットのコンテンツ パイプライン 
        /// AnimationContentDictionary オブジェクトを
        /// 実行時の AnimationClip フォーマットに変換します。
        /// </summary>
        static Dictionary<string, AnimationClip> ProcessAnimations(
            AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // ボーン名をインデックスにマッピングする表を構築します。
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // 各アニメーションを順を追って変換します。
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
                            "Input file does not contain any animations.");
            }

            return animationClips;
        }


        /// <summary>
        /// 中間フォーマットのコンテンツ パイプライン 
        /// AnimationContent オブジェクトを
        /// 実行時の AnimationClip フォーマットに変換します。
        /// </summary>
        static AnimationClip ProcessAnimation(AnimationContent animation,
                                              Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            // 入力アニメーション チャンネルを繰り返します。
            foreach (KeyValuePair<string, AnimationChannel> channel in
                animation.Channels)
            {
                // このチャンネルが制御するボーンを検索します。
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));
                }

                // キーフレーム データを変換します。
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time,
                                               keyframe.Transform));
                }
            }

            // 結合されたキーフレームを時間別に並べ替えます。
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return new AnimationClip(animation.Duration, keyframes);
        }


        /// <summary>
        /// キーフレームを時間の昇順に並べ替えるための比較関数。
        /// </summary>
        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }


        /// <summary>
        /// このメッシュにアニメーション表示する方法がわかっている
        /// 種類のデータが含まれていることを確認します。
        /// </summary>
        static void ValidateMesh(NodeContent node, ContentProcessorContext context,
                                 string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // メッシュを検証します。
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} is a child of bone {1}. SkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.",
                        mesh.Name, parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} has no skinning information, so it has been deleted.",
                        mesh.Name);

                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // これがボーンの場合、その内部を見ていることを
                // 忘れないでください。
                parentBoneName = node.Name;
            }

            // 再帰処理します (子の検証によって一部が削除される場合が
            // あるため、子のコレクションのコピーを反復処理します)。
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }


        /// <summary>
        /// メッシュにスキニング情報が含まれているかどうかを確認します。
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
        /// すべてが同じ座標系に収まるように、不要なトランスフォームを
        /// モデル ジオメトリに焼き付けます。
        /// </summary>
        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // このスケルトンは特殊なので、処理しないでください。
                if (child == skeleton)
                    continue;

                // ローカル トランスフォームを実際のジオメトリに焼き付けます。
                MeshHelper.TransformScene(child, child.Transform);

                // 焼き付けが完了すると、ローカル座標系を
                // ID に戻すことができます。
                child.Transform = Matrix.Identity;

                // 再帰処理します。
                FlattenTransforms(child, skeleton);
            }
        }


        /// <summary>
        /// スキンが適用されたモデル エフェクトを使用するように、
        /// すべてのマテリアルを強制します。
        /// </summary>
        [DefaultValue(MaterialProcessorDefaultEffect.SkinnedEffect)]
        public override MaterialProcessorDefaultEffect DefaultEffect
        {
            get { return MaterialProcessorDefaultEffect.SkinnedEffect; }
            set { }
        }
    }
}
