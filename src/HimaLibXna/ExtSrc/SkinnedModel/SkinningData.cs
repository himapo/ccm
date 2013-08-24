#region File Description
//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinned Modelの
// コードを翻訳し、MatrixからQuatTransformに変更したもの
//
// http://creators.xna.com/en-US/sample/skinnedmodel
//-----------------------------------------------------------------------------
#endregion

#region Using ステートメント
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
    /// <summary>
    /// スキンアニメーションに必要な情報をもつSkinningDataクラス
    /// 通常、ModelクラスのTagプロパティに設定されている
    /// </summary>
    public class SkinningData
    {
        #region フィールド

        IDictionary<string, AnimationClip> animationClipsValue;
        IList<QuatTransform> bindPoseValue;
        IList<QuatTransform> inverseBindPoseValue;
        IList<int> skeletonHierarchyValue;

        #endregion


        /// <summary>
        /// 新しいSkinningDataの生成
        /// </summary>
        public SkinningData(IDictionary<string, AnimationClip> animationClips,
                            IList<QuatTransform> bindPose, IList<QuatTransform> inverseBindPose,
                            IList<int> skeletonHierarchy,
                            Dictionary<string, int> boneIndices)
        {
            animationClipsValue = animationClips;
            bindPoseValue = bindPose;
            inverseBindPoseValue = inverseBindPose;
            skeletonHierarchyValue = skeletonHierarchy;
            BoneIndices = boneIndices;
        }


        /// <summary>
        /// アニメーションクリップ辞書テーブルの取得
        /// アニメーションクリップは"Walk", "Run",
        /// "JumpReallyHigh"といった名前で取得できる
        /// </summary>
        public IDictionary<string, AnimationClip> AnimationClips
        {
            get { return animationClipsValue; }
        }


        /// <summary>
        /// 各ボーンのバインドポーズ行列の取得
        /// </summary>
        public IList<QuatTransform> BindPose
        {
            get { return bindPoseValue; }
        }


        /// <summary>
        /// 各ボーンのバインドポーズ逆行列の取得
        /// </summary>
        public IList<QuatTransform> InverseBindPose
        {
            get { return inverseBindPoseValue; }
        }


        /// <summary>
        /// 各ボーンの親のボーンのインデックス情報を取得する
        /// </summary>
        public IList<int> SkeletonHierarchy
        {
            get { return skeletonHierarchyValue; }
        }

        /// <summary>
        /// ボーン名を、上記のリスト内のそれらのインデックスにマッピングする辞書。
        /// </summary>
        //[ContentSerializer]
        public Dictionary<string, int> BoneIndices { get; private set; }
    }
}
