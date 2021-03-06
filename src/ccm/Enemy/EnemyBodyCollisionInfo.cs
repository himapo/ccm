﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;
using HimaLib.Math;
using ccm.Collision;

namespace ccm.Enemy
{
    /// <summary>
    /// データ読みに対応するまでのプログラムによる構築
    /// </summary>
    class EnemyBodyCollisionInfo : CollisionInfo
    {
        public Func<Vector3> Base { set { Primitive.Base = value; } }

        public Action<int, int, CollisionResult> Reaction { set { CollisionReactor.Reaction = value; } }

        CylinderCollisionPrimitive Primitive = new CylinderCollisionPrimitive();

        CollisionReactor CollisionReactor = new CollisionReactor();

        public EnemyBodyCollisionInfo()
        {
            Active = () => true;
            Group = () => (int)ccm.Collision.CollisionGroup.EnemyBody;
            Reactor = CollisionReactor;

            Primitive.Radius = () => 2.0f;
            Primitive.Height = () => 4.0f;
            Primitives.Add(Primitive);
        }
    }
}
