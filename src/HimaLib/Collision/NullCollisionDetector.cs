using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class NullCollisionDetector : ICollisionDetector
    {
        public bool Detect(out Vector3 overlap)
        {
            overlap = Vector3.Zero;
            return false;
        }
    }
}
