#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// コードを翻訳、変更したもの
//
// オリジナルからの変更点としては以下の二つ:
// 1. skinTransformsがMatrix[]がQuaternion[]のskinRotations、
//      Vector3[]のskinTrnaslationsの二つに分解。
//      これにともない、SkinTranslationsプロパティも
//      SkinRotations,SkinTranslationsの二つのプロパティに置き換わった
//
// 2. UpdateメソッドからrootTransform引数を削除、rootTransformの結合は
//      シェーダー内でするようになったので、同じアニメーション結果を
//      複数のモデルで共有できるようになった
//  
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// AnimationClipからボーン行列を生成するアニメーションプレイヤー
    /// </summary>
    public class AnimationPlayer
    {
        #region フィールド

        // 再生中のアニメーション・クリップの情報
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyframe;

        // 現在のアニメーション変換行列
        QuatTransform[] boneTransforms;
        QuatTransform[] worldTransforms;
        Quaternion[] skinRotations;
        Vector4[] skinTranslations;

        // バインドポーズとスケルトン情報を取得するためのバックリンク
        SkinningData skinningDataValue;

        #endregion

        /// <summary>
        /// 新しいアニメーション・プレイヤーの生成
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new QuatTransform[skinningData.BindPose.Count];
            worldTransforms = new QuatTransform[skinningData.BindPose.Count];
            skinRotations = new Quaternion[skinningData.BindPose.Count];
            skinTranslations = new Vector4[skinningData.BindPose.Count];
        }


        /// <summary>
        /// 指定されたアニメーションクリップのデコーディング開始
        /// </summary>
        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // ボーン変換行列をバインドポーズで初期化する
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


        /// <summary>
        /// アニメーション再生位置の更新
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime)
        {
            UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms();
            UpdateSkinTransforms();
        }


        /// <summary>
        /// BoneTransformsを更新する
        /// Updateメソッドから呼ばれるヘルパーメソッド
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "StartClipが呼び出される前にAnimationPlayer.Updateを呼んだ" );

            // アニメーション再生位置の更新
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

                // アニメーションの終点まできたら、ループする
                while (time >= currentClipValue.Duration)
                    time -= currentClipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // 再生位置が過去方向に戻ったなら、キーフレームのインデックスをリセットする
            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // キーフレームの行列を読み出す
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // 現時刻まで到達したら読み込み終了
                if (keyframe.Time > currentTimeValue)
                    break;

                // このキーフレームを使う
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }


        /// <summary>
        /// WorldTransformsを更新する
        /// Updateメソッドから呼ばれるヘルパーメソッド
        /// </summary>
        public void UpdateWorldTransforms()
        {
            // ルートボーン
            worldTransforms[0] = boneTransforms[0];

            // 子のボーン
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// SkinTransformsの更新
        /// Updateメソッドから呼ばれるヘルパーメソッド
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinRotations.Length; bone++)
            {
                QuatTransform xform =
                    skinningDataValue.InverseBindPose[bone] * worldTransforms[bone];

                skinRotations[bone] = xform.Rotation;
                skinTranslations[bone].X = xform.Translation.X;
                skinTranslations[bone].Y = xform.Translation.Y;
                skinTranslations[bone].Z = xform.Translation.Z;
            }
        }

        /// <summary>
        /// ボーン変換行列(親のボーンへの変換行列)の取得
        /// </summary>
        public QuatTransform[] GetBoneTransforms()
        {
            return boneTransforms;
        }

        /// <summary>
        /// ボーン変換行列(ワールド座標への変換行列)の取得
        /// </summary>
        public QuatTransform[] GetWorldTransforms()
        {
            return worldTransforms;
        }

        /// <summary>
        /// ボーン変換行列(バインドポーズへの変換行列)の回転部分の取得
        /// </summary>
        public Quaternion[] GetSkinRotations()
        {
            return skinRotations;
        }

        /// <summary>
        /// ボーン変換行列(バインドポーズへの変換行列)の平行移動部分の取得
        /// </summary>
        public Vector4[] GetSkinTraslations()
        {
            return skinTranslations;
        }

        /// <summary>
        /// 現在再生中のAnimationClipの取得
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }

        /// <summary>
        /// 再生位置の取得
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
