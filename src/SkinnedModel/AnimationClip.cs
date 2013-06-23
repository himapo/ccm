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
    /// アニメーション クリップは、
    /// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent 型
    /// に相当するランタイム クラスです。
    /// このクラスは、単一のアニメーションを記述するために
    /// 必要なすべてのキーフレームを保持します。
    /// </summary>
    public class AnimationClip
    {
        /// <summary>
        /// 新しいアニメーション クリップ オブジェクトを構築します。
        /// </summary>
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
        }


        /// <summary>
        /// XNB デシリアライザーによって使用されるプライベート コンストラクター。
        /// </summary>
        private AnimationClip()
        {
        }


        /// <summary>
        /// アニメーション全体の長さを取得します。
        /// </summary>
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }


        /// <summary>
        /// 時間によって並べ替えられた、すべてのボーンの
        /// すべてのキーフレームを含む
        /// 連結されたリストを取得します。
        /// </summary>
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }
    }
}
