using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ccm
{
    interface IEnemyService
    {
        void Add();

        void Clear();

        void DamageRange(int damage, Vector3 source, float range);
    }
}
