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
    /// �J�X�^�� �v���Z�b�T�́A�r���g�C�� �t���[�����[�N ModelProcessor ���g�����A
    /// �A�j���[�V�����̃T�|�[�g��ǉ����܂��B
    /// </summary>
    [ContentProcessor]
    public class SkinnedModelProcessor : ModelProcessor
    {
        /// <summary>
        /// ���C�� Process ���\�b�h�́A���ԃt�H�[�}�b�g��
        /// �R���e���c �p�C�v���C�� NodeContent �c���[��
        /// ���ߍ��݃A�j���[�V���� �f�[�^���܂� ModelContent 
        /// �I�u�W�F�N�g�ɕϊ����܂��B
        /// </summary>
        public override ModelContent Process(NodeContent input,
                                             ContentProcessorContext context)
        {
            ValidateMesh(input, context, null);

            // �X�P���g���������܂��B
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");

            // �ʂ̃��[�J�����W�n�ɂ��郂�f���̈قȂ镔���ɂ���
            // �S�z���Ȃ��Ă��ςނ悤�ɁA���ׂďĂ��t���܂��傤�B
            FlattenTransforms(input, skeleton);

            // �o�C���h �|�[�Y�ƃX�P���g���̊K�w�f�[�^��ǂݍ��݂܂��B
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

            // �A�j���[�V���� �f�[�^�������^�C���`���ɕϊ����܂��B
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);

            // ���f�� �f�[�^��ϊ��ł���悤�ɁA��{ ModelProcessor 
            // �N���X�Ƀ`�F�[�����܂��B
            ModelContent model = base.Process(input, context);

            // �J�X�^�� �A�j���[�V���� �f�[�^���A���f���� 
            // Tag �v���p�e�B�Ɋi�[���܂��B
            model.Tag = new SkinningData(animationClips, bindPose,
                                         inverseBindPose, skeletonHierarchy);

            return model;
        }


        /// <summary>
        /// ���ԃt�H�[�}�b�g�̃R���e���c �p�C�v���C�� 
        /// AnimationContentDictionary �I�u�W�F�N�g��
        /// ���s���� AnimationClip �t�H�[�}�b�g�ɕϊ����܂��B
        /// </summary>
        static Dictionary<string, AnimationClip> ProcessAnimations(
            AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // �{�[�������C���f�b�N�X�Ƀ}�b�s���O����\���\�z���܂��B
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // �e�A�j���[�V����������ǂ��ĕϊ����܂��B
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
        /// ���ԃt�H�[�}�b�g�̃R���e���c �p�C�v���C�� 
        /// AnimationContent �I�u�W�F�N�g��
        /// ���s���� AnimationClip �t�H�[�}�b�g�ɕϊ����܂��B
        /// </summary>
        static AnimationClip ProcessAnimation(AnimationContent animation,
                                              Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            // ���̓A�j���[�V���� �`�����l�����J��Ԃ��܂��B
            foreach (KeyValuePair<string, AnimationChannel> channel in
                animation.Channels)
            {
                // ���̃`�����l�������䂷��{�[�����������܂��B
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));
                }

                // �L�[�t���[�� �f�[�^��ϊ����܂��B
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time,
                                               keyframe.Transform));
                }
            }

            // �������ꂽ�L�[�t���[�������ԕʂɕ��בւ��܂��B
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return new AnimationClip(animation.Duration, keyframes);
        }


        /// <summary>
        /// �L�[�t���[�������Ԃ̏����ɕ��בւ��邽�߂̔�r�֐��B
        /// </summary>
        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }


        /// <summary>
        /// ���̃��b�V���ɃA�j���[�V�����\��������@���킩���Ă���
        /// ��ނ̃f�[�^���܂܂�Ă��邱�Ƃ��m�F���܂��B
        /// </summary>
        static void ValidateMesh(NodeContent node, ContentProcessorContext context,
                                 string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // ���b�V�������؂��܂��B
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
                // ���ꂪ�{�[���̏ꍇ�A���̓��������Ă��邱�Ƃ�
                // �Y��Ȃ��ł��������B
                parentBoneName = node.Name;
            }

            // �ċA�������܂� (�q�̌��؂ɂ���Ĉꕔ���폜�����ꍇ��
            // ���邽�߁A�q�̃R���N�V�����̃R�s�[�𔽕��������܂�)�B
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }


        /// <summary>
        /// ���b�V���ɃX�L�j���O��񂪊܂܂�Ă��邩�ǂ������m�F���܂��B
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
        /// ���ׂĂ��������W�n�Ɏ��܂�悤�ɁA�s�v�ȃg�����X�t�H�[����
        /// ���f�� �W�I���g���ɏĂ��t���܂��B
        /// </summary>
        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // ���̃X�P���g���͓���Ȃ̂ŁA�������Ȃ��ł��������B
                if (child == skeleton)
                    continue;

                // ���[�J�� �g�����X�t�H�[�������ۂ̃W�I���g���ɏĂ��t���܂��B
                MeshHelper.TransformScene(child, child.Transform);

                // �Ă��t������������ƁA���[�J�����W�n��
                // ID �ɖ߂����Ƃ��ł��܂��B
                child.Transform = Matrix.Identity;

                // �ċA�������܂��B
                FlattenTransforms(child, skeleton);
            }
        }


        /// <summary>
        /// �X�L�����K�p���ꂽ���f�� �G�t�F�N�g���g�p����悤�ɁA
        /// ���ׂẴ}�e���A�����������܂��B
        /// </summary>
        [DefaultValue(MaterialProcessorDefaultEffect.SkinnedEffect)]
        public override MaterialProcessorDefaultEffect DefaultEffect
        {
            get { return MaterialProcessorDefaultEffect.SkinnedEffect; }
            set { }
        }
    }
}
