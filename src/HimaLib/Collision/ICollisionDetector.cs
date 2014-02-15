using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public interface ICollisionDetector
    {
        bool Detect(out Vector3 overlap);
    }
}
