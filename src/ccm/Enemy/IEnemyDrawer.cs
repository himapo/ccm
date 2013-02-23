using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Enemy
{
    public interface IEnemyDrawer
    {
        void Draw(Enemy enemy);
    }
}
