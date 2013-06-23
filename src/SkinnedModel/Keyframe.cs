#region File Description
//-----------------------------------------------------------------------------
// Keyframe.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// ���鎞�_�ł� 1 �̃{�[���̈ʒu���L�q���܂��B
    /// </summary>
    public class Keyframe
    {
        /// <summary>
        /// �V�����L�[�t���[�� �I�u�W�F�N�g���\�z���܂��B
        /// </summary>
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }


        /// <summary>
        /// XNB �f�V���A���C�U�[�ɂ���Ďg�p�����v���C�x�[�g �R���X�g���N�^�[�B
        /// </summary>
        private Keyframe()
        {
        }


        /// <summary>
        /// ���̃L�[�t���[�����A�j���[�V�����\������
        /// �^�[�Q�b�g �{�[���̃C���f�b�N�X���擾���܂��B
        /// </summary>
        [ContentSerializer]
        public int Bone { get; private set; }


        /// <summary>
        /// �A�j���[�V�����̊J�n���炱�̃L�[�t���[���܂ł�
        /// ���ԃI�t�Z�b�g���擾���܂��B
        /// </summary>
        [ContentSerializer]
        public TimeSpan Time { get; private set; }


        /// <summary>
        /// ���̃L�[�t���[���̃{�[�� �g�����X�t�H�[�����擾���܂��B
        /// </summary>
        [ContentSerializer]
        public Matrix Transform { get; private set; }
    }
}
