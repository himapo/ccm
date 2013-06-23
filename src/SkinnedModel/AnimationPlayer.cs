#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// アニメーション プレイヤーは、アニメーション クリップから
    /// ボーンの位置行列をデコードします。
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // 現在再生中のアニメーション クリップに関する情報。
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyframe;


        // 現在のアニメーションのトランスフォーム行列。
        Matrix[] boneTransforms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;


        // バインド ポーズとスケルトンの階層データへの後方リンク。
        SkinningData skinningDataValue;


        #endregion


        /// <summary>
        /// 新しいアニメーション プレイヤーを構築します。
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }


        /// <summary>
        /// 指定されたアニメーション クリップのデコードを開始します。
        /// </summary>
        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // バインド ポーズへのボーン トランスフォームを初期化します。
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


        /// <summary>
        /// 現在のアニメーション位置を前進させます。
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }


        /// <summary>
        /// BoneTransforms データを更新するために 
        /// Update メソッドが使用するヘルパー。
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // アニメーション位置を更新します。
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

                // 最後に到達したら、最初にループバックします。
                while (time >= currentClipValue.Duration)
                    time -= currentClipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // 位置が後方に移動したら、キーフレームの
            // インデックスをリセットします。
            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // キーフレームの行列を読み込みます。
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // 現在の時間位置まで読み込んだときに停止します。
                if (keyframe.Time > currentTimeValue)
                    break;

                // このキーフレームを使用します。
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }


        /// <summary>
        /// WorldTransforms データを更新するために 
        /// Update メソッドが使用するヘルパー。
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // ルート ボーン。
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // 子ボーン。
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// SkinTransforms データを更新するために 
        /// Update メソッドが使用するヘルパー。
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }


        /// <summary>
        /// 親ボーンを基準にした、現在のボーン トランスフォーム行列を取得します。
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// 現在のボーン トランスフォーム行列を、絶対フォーマットで取得します。
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// スキニング バインド ポーズを基準にした、現在の
        /// ボーン トランスフォーム行列を取得します。
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// 現在デコード中のクリップを取得します。
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        /// <summary>
        /// 現在の再生位置を取得します。
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
