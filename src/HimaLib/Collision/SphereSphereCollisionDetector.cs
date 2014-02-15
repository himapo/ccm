using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class SphereSphereCollisionDetector : ICollisionDetector
    {
        public SphereCollisionPrimitive ParamA { get; set; }

        public SphereCollisionPrimitive ParamB { get; set; }

        public bool Detect(CollisionResult result)
        {
            // 中心間ベクトル
            var overlap = ParamB.Center() - ParamA.Center();
            
            // 中心間の距離
            var centerLength = overlap.Length();

            // めり込み距離
            var overlapLength = ParamA.Radius() + ParamB.Radius() - centerLength;

            // めり込みベクトル
            overlap *= overlapLength / centerLength;

            result.Overlap = overlap;
            
            return overlapLength > 0.0f;
        }
    }
}
