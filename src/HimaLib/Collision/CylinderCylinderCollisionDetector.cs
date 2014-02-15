using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class CylinderCylinderCollisionDetector : ICollisionDetector
    {
        public CylinderCollisionPrimitive ParamA { get; set; }

        public CylinderCollisionPrimitive ParamB { get; set; }

        public bool Detect(CollisionResult result)
        {
            var baseA = ParamA.Base();
            var baseB = ParamB.Base();
            var horizontalA = new Vector2(baseA.X, baseA.Z);
            var horizontalB = new Vector2(baseB.X, baseB.Z);

            // 水平交差判定
            var hOverlapVector = horizontalB - horizontalA;
            var hCenterLength = hOverlapVector.Length();
            var hOverlapLength = ParamA.Radius() + ParamB.Radius() - hCenterLength;
            hOverlapVector *= hOverlapLength / hCenterLength;

            var horizontal = hOverlapLength > 0.0f;

            if (!horizontal)
            {
                result.Overlap = Vector3.Zero;
                return false;
            }

            result.Overlap.X = hOverlapVector.X;
            result.Overlap.Z = hOverlapVector.Y;

            // 垂直交差判定
            var al = baseA.Y;
            var ah = al + ParamA.Height();
            var bl = baseB.Y;
            var bh = bl + ParamB.Height();
            var vertical =
                (al > bl && al < bh)
                || (ah > bl && ah < bh)
                || (bl > al && bl < ah)
                || (bh > al && bh < ah);

            if (!vertical)
            {
                result.Overlap = Vector3.Zero;
                return false;
            }

            // Aの下とBの上がめり込んでると見る場合
            var a_bottom_b_top = al - bh;
            
            // Aの上とBの下がめり込んでると見る場合
            var a_top_b_bottom = ah - bl;

            // めり込み量が少ない方を採用
            result.Overlap.Y =
                MathUtil.Abs(a_bottom_b_top) < MathUtil.Abs(a_top_b_bottom)
                ? a_bottom_b_top
                : a_top_b_bottom;

            return true;
        }
    }
}
