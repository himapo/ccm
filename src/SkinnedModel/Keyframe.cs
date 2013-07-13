#region File Description
//-----------------------------------------------------------------------------
// Keyframe.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// コードを翻訳し、Matrix部分をQuatTransformに変更したもの
//
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using System;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// キーフレーム、時間とボーンの変換行列のペア
    /// </summary>
    public class Keyframe
    {
        #region フィールド

        int boneValue;
        TimeSpan timeValue;
        QuatTransform transformValue;

        #endregion


        /// <summary>
        /// キーフレームオブジェクトのコンストラクタ
        /// </summary>
        public Keyframe( int bone, TimeSpan time, QuatTransform transform )
        {
            boneValue = bone;
            timeValue = time;
            transformValue = transform;
        }


        /// <summary>
        /// ボーンインデックスの取得
        /// </summary>
        public int Bone
        {
            get { return boneValue; }
        }


        /// <summary>
        /// このキーフレームが設定されたアニメーション開始時点からの経過時間の取得
        /// </summary>
        public TimeSpan Time
        {
            get { return timeValue; }
        }


        /// <summary>
        /// このキーフレームの変換行列の取得
        /// </summary>
        public QuatTransform Transform
        {
            get { return transformValue; }
        }
    }
}
