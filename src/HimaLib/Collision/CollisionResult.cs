using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public enum CollisionResultType
    {
        RaySphere,
    }

    /// <summary>
    /// コリジョンの結果を付加情報を含めて格納したクラス
    /// </summary>
    public class CollisionResult
    {
        public CollisionResultType Type { get; set; }

        public bool Detected { get; set; }

        /// <summary>
        /// めり込みベクトル
        /// </summary>
        public Vector3 Overlap;

        /// <summary>
        /// 交点までの距離
        /// </summary>
        public float Distance { get; set; }
    }
}
