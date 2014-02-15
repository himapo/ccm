using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public interface ICollisionReactor
    {
        void React(int id, int count, ICollisionActor actor, Vector3 overlap);
    }
}
