using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Collision
{
    public enum CollisionGroup
    {
        PlayerBody,
        PlayerGround,   // 接地判定
        PlayerAttack,
        PlayerDamage,
        AllyBody,
        AllyAttack,
        AllyDamage,
        EnemyBody,
        EnemyAttack,
        EnemyDamage,
        Map,
    }
}
