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
    /// ある時点での 1 つのボーンの位置を記述します。
    /// </summary>
    public class Keyframe
    {
        /// <summary>
        /// 新しいキーフレーム オブジェクトを構築します。
        /// </summary>
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }


        /// <summary>
        /// XNB デシリアライザーによって使用されるプライベート コンストラクター。
        /// </summary>
        private Keyframe()
        {
        }


        /// <summary>
        /// このキーフレームがアニメーション表示する
        /// ターゲット ボーンのインデックスを取得します。
        /// </summary>
        [ContentSerializer]
        public int Bone { get; private set; }


        /// <summary>
        /// アニメーションの開始からこのキーフレームまでの
        /// 時間オフセットを取得します。
        /// </summary>
        [ContentSerializer]
        public TimeSpan Time { get; private set; }


        /// <summary>
        /// このキーフレームのボーン トランスフォームを取得します。
        /// </summary>
        [ContentSerializer]
        public Matrix Transform { get; private set; }
    }
}
