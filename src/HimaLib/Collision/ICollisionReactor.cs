using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public interface ICollisionReactor
    {
        void React(int id, int count, ICollisionActor actor);
    }
}
