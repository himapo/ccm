using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    /// <summary>
    /// コリジョン判定のグループ
    /// </summary>
    enum CollisionGroup
    {
        PlayerBody,
        EnemyBody,
        AllyBody,
        PlayerAttack,
        EnemyAttack,
        AllyAttack,
        PlayerDamage,
        EnemyDamage,
        AllyDamage,
        Map,
        Item,
    }

    class CollisionInfo
    {
        // コリジョンオブジェクト一個に一意のID
        public int ID { get; set; }

        // 現在コリジョンが有効かどうか
        public Func<bool> Active { get; set; }

        // コリジョン形状
        public CollisionShape Shape {
            get { return ShapeParameter.Shape; }
        }

        // 形状パラメータ
        public ShapeParameter ShapeParameter { get; set; }

        // コリジョングループ
        public Func<CollisionGroup> Group { get; set; }

        // 応答
        public Action<Object, CollisionOpponent, int> Reaction { get; set; }

        // 応答の引数
        public Func<Object> ReactionArg { get; set; }

        // 相手に渡す情報
        public CollisionOpponent ToOpponent { get; set; }
    }

    interface ICollisionService
    {
        void Add(CollisionInfo info);
 
        void Remove(CollisionInfo info);

        void Reset(int ID);
    }
}
