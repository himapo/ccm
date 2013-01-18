using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    /// <summary>
    /// ヒットした相手の種類
    /// </summary>
    enum OpponentType
    {
        // 存在判定
        PlayerBody,
        EnemyBody,
        AllyBody,

        // 攻撃判定
        PlayerAttack,
        EnemyAttack,
        AllyAttack,

        // 食らい判定
        PlayerDamage,
        EnemyDamage,
        AllyDamage,

        Map,
        Item,
    }

    /// <summary>
    /// ヒットした相手に渡す自分の情報基底
    /// </summary>
    abstract class CollisionOpponent
    {
        public abstract OpponentType Type { get; }
    }

    /// <summary>
    /// プレイヤーがヒットした相手に渡す情報
    /// </summary>
    class PlayerBodyCollisionOpponent : CollisionOpponent
    {
        public override OpponentType Type
        {
            get { return OpponentType.PlayerBody; }
        }
    }

    /// <summary>
    /// プレイヤー攻撃で相手に渡す情報
    /// </summary>
    class PlayerAttackCollisionOpponent : CollisionOpponent
    {
        public override OpponentType Type
        {
            get { return OpponentType.PlayerAttack; }
        }

        // 攻撃力
        public Func<int> Power { get; set; }
    }

    /// <summary>
    /// プレイヤーダメージで相手に渡す情報
    /// </summary>
    class PlayerDamageCollisionOpponent : CollisionOpponent
    {
        public override OpponentType Type
        {
            get { return OpponentType.PlayerDamage; }
        }
    }

    /// <summary>
    /// 敵の存在判定で相手に渡す情報
    /// </summary>
    class EnemyBodyCollisionOpponent : CollisionOpponent
    {
        public override OpponentType Type
        {
            get { return OpponentType.EnemyBody; }
        }
    }

    /// <summary>
    /// 敵ダメージで相手に渡す情報
    /// </summary>
    class EnemyDamageCollisionOpponent : CollisionOpponent
    {
        public override OpponentType Type
        {
            get { return OpponentType.EnemyDamage; }
        }
    }
}
