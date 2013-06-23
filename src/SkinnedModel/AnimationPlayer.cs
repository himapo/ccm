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
    /// �A�j���[�V���� �v���C���[�́A�A�j���[�V���� �N���b�v����
    /// �{�[���̈ʒu�s����f�R�[�h���܂��B
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // ���ݍĐ����̃A�j���[�V���� �N���b�v�Ɋւ�����B
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyframe;


        // ���݂̃A�j���[�V�����̃g�����X�t�H�[���s��B
        Matrix[] boneTransforms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;


        // �o�C���h �|�[�Y�ƃX�P���g���̊K�w�f�[�^�ւ̌�������N�B
        SkinningData skinningDataValue;


        #endregion


        /// <summary>
        /// �V�����A�j���[�V���� �v���C���[���\�z���܂��B
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
        /// �w�肳�ꂽ�A�j���[�V���� �N���b�v�̃f�R�[�h���J�n���܂��B
        /// </summary>
        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // �o�C���h �|�[�Y�ւ̃{�[�� �g�����X�t�H�[�������������܂��B
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


        /// <summary>
        /// ���݂̃A�j���[�V�����ʒu��O�i�����܂��B
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }


        /// <summary>
        /// BoneTransforms �f�[�^���X�V���邽�߂� 
        /// Update ���\�b�h���g�p����w���p�[�B
        /// </summary>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // �A�j���[�V�����ʒu���X�V���܂��B
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

                // �Ō�ɓ��B������A�ŏ��Ƀ��[�v�o�b�N���܂��B
                while (time >= currentClipValue.Duration)
                    time -= currentClipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // �ʒu������Ɉړ�������A�L�[�t���[����
            // �C���f�b�N�X�����Z�b�g���܂��B
            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // �L�[�t���[���̍s���ǂݍ��݂܂��B
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // ���݂̎��Ԉʒu�܂œǂݍ��񂾂Ƃ��ɒ�~���܂��B
                if (keyframe.Time > currentTimeValue)
                    break;

                // ���̃L�[�t���[�����g�p���܂��B
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }


        /// <summary>
        /// WorldTransforms �f�[�^���X�V���邽�߂� 
        /// Update ���\�b�h���g�p����w���p�[�B
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // ���[�g �{�[���B
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // �q�{�[���B
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// SkinTransforms �f�[�^���X�V���邽�߂� 
        /// Update ���\�b�h���g�p����w���p�[�B
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
        /// �e�{�[������ɂ����A���݂̃{�[�� �g�����X�t�H�[���s����擾���܂��B
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// ���݂̃{�[�� �g�����X�t�H�[���s����A��΃t�H�[�}�b�g�Ŏ擾���܂��B
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// �X�L�j���O �o�C���h �|�[�Y����ɂ����A���݂�
        /// �{�[�� �g�����X�t�H�[���s����擾���܂��B
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// ���݃f�R�[�h���̃N���b�v���擾���܂��B
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        /// <summary>
        /// ���݂̍Đ��ʒu���擾���܂��B
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
