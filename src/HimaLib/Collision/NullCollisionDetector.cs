using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public class NullCollisionDetector : ICollisionDetector
    {
        public bool Detect(CollisionResult result)
        {
            return false;
        }
    }
}
