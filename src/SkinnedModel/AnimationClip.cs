#region File Description
//-----------------------------------------------------------------------------
// AnimationClip.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// �A�j���[�V���� �N���b�v�́A
    /// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent �^
    /// �ɑ������郉���^�C�� �N���X�ł��B
    /// ���̃N���X�́A�P��̃A�j���[�V�������L�q���邽�߂�
    /// �K�v�Ȃ��ׂẴL�[�t���[����ێ����܂��B
    /// </summary>
    public class AnimationClip
    {
        /// <summary>
        /// �V�����A�j���[�V���� �N���b�v �I�u�W�F�N�g���\�z���܂��B
        /// </summary>
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
        }


        /// <summary>
        /// XNB �f�V���A���C�U�[�ɂ���Ďg�p�����v���C�x�[�g �R���X�g���N�^�[�B
        /// </summary>
        private AnimationClip()
        {
        }


        /// <summary>
        /// �A�j���[�V�����S�̂̒������擾���܂��B
        /// </summary>
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }


        /// <summary>
        /// ���Ԃɂ���ĕ��בւ���ꂽ�A���ׂẴ{�[����
        /// ���ׂẴL�[�t���[�����܂�
        /// �A�����ꂽ���X�g���擾���܂��B
        /// </summary>
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }
    }
}
