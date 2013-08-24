#region File Description
//-----------------------------------------------------------------------------
// TypeWriter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// コードを翻訳し、QuatTransformWriterを追加した
//
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SkinnedModel;
using Microsoft.Xna.Framework.Content.Pipeline;
#endregion

namespace SkinnedModelPipeline
{
    /// <summary>
    /// SkinningDataをXNBファイルに書き出す
    /// </summary>
    [ContentTypeWriter]
    public class SkinningDataWriter : ContentTypeWriter<SkinningData>
    {
        protected override void Write(ContentWriter output, SkinningData value)
        {
            output.WriteObject(value.AnimationClips);
            output.WriteObject(value.BindPose);
            output.WriteObject(value.InverseBindPose);
            output.WriteObject(value.SkeletonHierarchy);
            output.WriteObject(value.BoneIndices);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SkinningDataReader).AssemblyQualifiedName;
        }
    }


    /// <summary>
    /// AnimationClipをXNBファイルに書き出す
    /// </summary>
    [ContentTypeWriter]
    public class AnimationClipWriter : ContentTypeWriter<AnimationClip>
    {
        protected override void Write(ContentWriter output, AnimationClip value)
        {
            output.WriteObject(value.Duration);
            output.WriteObject(value.Keyframes);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationClipReader).AssemblyQualifiedName;
        }
    }


    /// <summary>
    /// KeyFrameをXNBファイルに書き出す
    /// </summary>
    [ContentTypeWriter]
    public class KeyframeWriter : ContentTypeWriter<Keyframe>
    {
        protected override void Write(ContentWriter output, Keyframe value)
        {
            output.WriteObject(value.Bone);
            output.WriteObject(value.Time);
            output.WriteObject(value.Transform);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(KeyframeReader).AssemblyQualifiedName;
        }
    }

    /// <summary>
    /// QuatTransformをXNBファイルに書き出す
    /// </summary>
    [ContentTypeWriter]
    public class QuatTransformWriter : ContentTypeWriter<QuatTransform>
    {
        protected override void Write( ContentWriter output, QuatTransform value )
        {
            output.WriteObject( value.Rotation );
            output.WriteObject( value.Translation );
        }

        public override string GetRuntimeReader( TargetPlatform targetPlatform )
        {
            return typeof( QuatTransformReader ).AssemblyQualifiedName;
        }
    }

}
