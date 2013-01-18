using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class CylinderCylinderCpollisionDetector : ICollisionDetector
    {
        public CylinderCollisionPrimitive ParamA { get; set; }

        public CylinderCollisionPrimitive ParamB { get; set; }

        public bool Detect()
        {
            var baseA = ParamA.Base();
            var baseB = ParamB.Base();
            var horizonalA = new Vector2(baseA.X, baseA.Z);
            var horizonalB = new Vector2(baseB.X, baseB.Z);

            // 水平交差判定
            var horizonal = ((horizonalA - horizonalB).Length() < ParamA.Radius() + ParamB.Radius());
            if (!horizonal)
                return false;

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

            return vertical;
        }
    }
}
