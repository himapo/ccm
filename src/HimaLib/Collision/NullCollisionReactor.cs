using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public class NullCollisionReactor : ICollisionReactor
    {
        public void React(int id, int count, ICollisionActor actor, CollisionResult result)
        {
        }
    }
}
