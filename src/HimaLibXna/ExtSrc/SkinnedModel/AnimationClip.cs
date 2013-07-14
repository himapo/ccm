#region File Description
//-----------------------------------------------------------------------------
// AnimationClip.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// コードを翻訳しただけ
//
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using System;
using System.Collections.Generic;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContentに
    /// 相当する情報を持つアニメーションクリップ
    /// ひとつのアニメーションに必要な複数キーフレーム情報を持つ
    /// </summary>
    public class AnimationClip
    {
        /// <summary>
        /// 新しいアニメーションクリップの生成
        /// </summary>
        public AnimationClip(TimeSpan duration, IList<Keyframe> keyframes)
        {
            durationValue = duration;
            keyframesValue = keyframes;
        }


        /// <summary>
        /// アニメーション期間の取得
        /// </summary>
        public TimeSpan Duration
        {
            get { return durationValue; }
        }

        TimeSpan durationValue;


        /// <summary>
        /// 全ボーンの全キーフレームをもつリストの取得
        /// キーフレームは時間順に並べ替えてある
        /// </summary>
        public IList<Keyframe> Keyframes
        {
            get { return keyframesValue; }
        }

        IList<Keyframe> keyframesValue;
    }
}
