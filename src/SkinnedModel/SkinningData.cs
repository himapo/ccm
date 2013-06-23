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
    /// スキンが適用されるオブジェクトをレンダリングし、アニメーション表示
    /// するために必要なすべてのデータを結合します。
    /// これは、通常アニメーション表示されるモデルの 
    /// Tag プロパティに格納されます。
    /// </summary>
    public class SkinningData
    {
        /// <summary>
        /// 新しいスキニング データ オブジェクトを構築します。
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
        /// XNB デシリアライザーによって使用されるプライベート コンストラクター。
        /// </summary>
        private SkinningData()
        {
        }


        /// <summary>
        /// アニメーション クリップのコレクションを取得します。
        /// これらのクリップは、
        /// "Walk"、"Run"、"JumpReallyHigh" などの名前で
        /// 辞書に保存されます。
        /// </summary>
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


        /// <summary>
        /// 親ボーンを基準にした、スケルトン内の各ボーンの
        /// バインド ポーズ行列。
        /// </summary>
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


        /// <summary>
        /// スケルトン内の各ボーンのボーン空間トランスポートに対する頂点。
        /// </summary>
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        /// <summary>
        /// スケルトン内のボーンごとに、親ボーンのインデックスを格納します。
        /// </summary>
        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }
    }
}
