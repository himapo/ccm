#region File Description
//-----------------------------------------------------------------------------
// TypeReaders.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// コードを翻訳したもの
//
// オリジナルからの変更点としてはMatrixからQuatTransformになったことと、
// QuatTransformReaderを追加した
//
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// SkinningDataをXNBファイルから読み込む
    /// </summary>
    public class SkinningDataReader : ContentTypeReader<SkinningData>
    {
        protected override SkinningData Read(ContentReader input,
                                             SkinningData existingInstance)
        {
            IDictionary<string, AnimationClip> animationClips;
            IList<QuatTransform> bindPose, inverseBindPose;
            IList<int> skeletonHierarchy;
            Dictionary<string, int> boneIndices;

            animationClips = input.ReadObject<IDictionary<string, AnimationClip>>();
            bindPose = input.ReadObject<IList<QuatTransform>>();
            inverseBindPose = input.ReadObject<IList<QuatTransform>>();
            skeletonHierarchy = input.ReadObject<IList<int>>();
            boneIndices = input.ReadObject<Dictionary<string, int>>();

            return new SkinningData(animationClips, bindPose,
                                    inverseBindPose, skeletonHierarchy,
                                    boneIndices);
        }
    }


    /// <summary>
    /// AnimationClipをXNBファイルから読み込む
    /// </summary>
    public class AnimationClipReader : ContentTypeReader<AnimationClip>
    {
        protected override AnimationClip Read(ContentReader input,
                                              AnimationClip existingInstance)
        {
            TimeSpan duration = input.ReadObject<TimeSpan>();
            IList<Keyframe> keyframes = input.ReadObject < IList<Keyframe>>();

            return new AnimationClip(duration, keyframes);
        }
    }


    /// <summary>
    /// KeyFrameをXNBファイルから読み込む
    /// </summary>
    public class KeyframeReader : ContentTypeReader<Keyframe>
    {
        protected override Keyframe Read(ContentReader input,
                                         Keyframe existingInstance)
        {
            int bone = input.ReadObject<int>();
            TimeSpan time = input.ReadObject<TimeSpan>();
            QuatTransform transform = input.ReadObject<QuatTransform>();

            return new Keyframe(bone, time, transform);
        }
    }

    /// <summary>
    /// QuatTransformをXNBファイルから読み込む
    /// </summary>
    public class QuatTransformReader : ContentTypeReader<QuatTransform>
    {
        protected override QuatTransform Read( ContentReader input,
                                         QuatTransform existingInstance )
        {
            Quaternion rotation = input.ReadObject<Quaternion>();
            Vector3 translation = input.ReadObject<Vector3>();
            return new QuatTransform( rotation, translation );
        }
    }

}
