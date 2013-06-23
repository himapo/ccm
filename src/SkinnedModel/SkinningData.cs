#region File Description
//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// �X�L�����K�p�����I�u�W�F�N�g�������_�����O���A�A�j���[�V�����\��
    /// ���邽�߂ɕK�v�Ȃ��ׂẴf�[�^���������܂��B
    /// ����́A�ʏ�A�j���[�V�����\������郂�f���� 
    /// Tag �v���p�e�B�Ɋi�[����܂��B
    /// </summary>
    public class SkinningData
    {
        /// <summary>
        /// �V�����X�L�j���O �f�[�^ �I�u�W�F�N�g���\�z���܂��B
        /// </summary>
        public SkinningData(Dictionary<string, AnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<int> skeletonHierarchy)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
        }


        /// <summary>
        /// XNB �f�V���A���C�U�[�ɂ���Ďg�p�����v���C�x�[�g �R���X�g���N�^�[�B
        /// </summary>
        private SkinningData()
        {
        }


        /// <summary>
        /// �A�j���[�V���� �N���b�v�̃R���N�V�������擾���܂��B
        /// �����̃N���b�v�́A
        /// "Walk"�A"Run"�A"JumpReallyHigh" �Ȃǂ̖��O��
        /// �����ɕۑ�����܂��B
        /// </summary>
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


        /// <summary>
        /// �e�{�[������ɂ����A�X�P���g�����̊e�{�[����
        /// �o�C���h �|�[�Y�s��B
        /// </summary>
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


        /// <summary>
        /// �X�P���g�����̊e�{�[���̃{�[����ԃg�����X�|�[�g�ɑ΂��钸�_�B
        /// </summary>
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        /// <summary>
        /// �X�P���g�����̃{�[�����ƂɁA�e�{�[���̃C���f�b�N�X���i�[���܂��B
        /// </summary>
        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }
    }
}
