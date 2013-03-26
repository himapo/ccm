using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using HimaLib.Collision;
using HimaLib.Camera;
using HimaLib.Debug;
using ccm.Player;
using ccm.Enemy;
using ccm.Collision;
using ccm.Deco;

namespace ccm.Enemy
{
    public class DungeonEnemyUpdater : StateMachine, IEnemyUpdater
    {
        // 敵アクセス
        public EnemyManager EnemyManager { get; set; }

        // プレイヤー情報
        public Player.Player Player { get; set; }

        // コリジョン
        public HimaLib.Collision.CollisionManager CollisionManager { get; set; }

        // ダンジョン情報

        // カメラ
        public ICamera Camera { get; set; }

        // デコ
        public ccm.Deco.DecoManager DecoManager { get; set; }
     
        // システム
        HimaLib.Math.IRand GameRand { get { return System.GameRand.Instance; } }

        // 実体
        Enemy Enemy;

        IModel Model
        {
            get { return Enemy.Model; }
            set { }
        }

        AffineTransform Transform
        {
            get { return Enemy.Transform; }
            set { }
        }

        AffineTransform PrevTransform = new AffineTransform();

        HimaLib.Collision.CylinderCollisionPrimitive BodyCollisionPrimitive;

        CollisionReactor BodyCollisionReactor;

        HimaLib.Collision.CollisionInfo BodyCollision;

        HimaLib.Collision.SphereCollisionPrimitive DamageCollisionPrimitive;

        AttackCollisionReactor DamageCollisionReactor;

        HimaLib.Collision.CollisionInfo DamageCollision;

        HimaLib.Collision.SphereCollisionPrimitive AttackCollisionPrimitive;

        AttackCollisionActor AttackCollisionActor;

        HimaLib.Collision.CollisionInfo AttackCollision;

        int Frame;

        int HitPoint;

        float Speed;

        float Distance;

        public DungeonEnemyUpdater()
        {
            BodyCollisionPrimitive = new CylinderCollisionPrimitive()
            {
                Base = () => { return new Vector3(Transform.Translation - Vector3.UnitY * 2.0f); },
                Radius = () => 2.0f,
                Height = () => 4.0f,
            };

            BodyCollisionReactor = new CollisionReactor()
            {
                Reaction = (id, count) =>
                {
                    Transform.Translation = PrevTransform.Translation;
                },
            };

            BodyCollision = new HimaLib.Collision.CollisionInfo()
            {
                Active = () => true,
                Group = () => (int)ccm.Collision.CollisionGroup.EnemyBody,
                Reactor = BodyCollisionReactor,
            };

            DamageCollisionPrimitive = new SphereCollisionPrimitive()
            {
                Center = () => Transform.Translation,
                Radius = () => 3.0f,
            };

            DamageCollisionReactor = new AttackCollisionReactor()
            {
                AttackReaction = Damage,
            };

            DamageCollision = new HimaLib.Collision.CollisionInfo()
            {
                Active = () => true,
                Group = () => (int)ccm.Collision.CollisionGroup.EnemyDamage,
                Reactor = DamageCollisionReactor,
            };

            AttackCollisionPrimitive = new SphereCollisionPrimitive()
            {
                Center = () => Transform.Translation,
                Radius = () => 4.0f,
            };

            AttackCollisionActor = new AttackCollisionActor()
            {
                Power = 1,
            };

            AttackCollision = new HimaLib.Collision.CollisionInfo()
            {
                Active = () => Frame % 360 < 180,
                Group = () => (int)ccm.Collision.CollisionGroup.EnemyAttack,
                Actor = AttackCollisionActor,
            };

            Speed = GameRand.NextFloat() * 0.2f + 0.4f;

            Distance = GameRand.NextFloat() * 40.0f + 10.0f;

            UpdateState = UpdateStateInit;
        }

        void Damage(int collisionId, int collisionCount, AttackCollisionActor actor)
        {
            if (HitPoint > 0 && collisionCount == 1)
            {
                HitPoint -= actor.Power;
                DebugPrint.PrintLine("Enemy damage {0}, HP {1}", actor.Power, HitPoint);
            }
            if (HitPoint <= 0)
            {
                UpdateState = UpdateStateTerm;
                DecoManager.Add(new ccm.Deco.Deco_Twister(Transform, Camera, GameRand));
            }
        }

        public void Update(Enemy enemy)
        {
            Enemy = enemy;
            PrevTransform = new AffineTransform(Transform);

            Update();
        }

        void UpdateStateInit()
        {
            InitCollision();
            HitPoint = 10;
            UpdateState = UpdateStateMain;
        }

        void InitCollision()
        {
            BodyCollision.Primitives.Clear();
            BodyCollision.Primitives.Add(BodyCollisionPrimitive);
            CollisionManager.Add(BodyCollision);

            DamageCollision.Primitives.Clear();
            DamageCollision.Primitives.Add(DamageCollisionPrimitive);
            CollisionManager.Add(DamageCollision);

            AttackCollision.Primitives.Clear();
            AttackCollision.Primitives.Add(AttackCollisionPrimitive);
            CollisionManager.Add(AttackCollision);
        }

        void UpdateStateMain()
        {
            MoveToPlayer();

            if (++Frame == 6000)
            {
                UpdateState = UpdateStateTerm;
            }
        }

        void UpdateStateTerm()
        {
            CollisionManager.Remove(BodyCollision);
            CollisionManager.Remove(DamageCollision);
            EnemyManager.DeleteEnemy(Enemy);
        }

        void MoveToPlayer()
        {
            var vecToPlayer = new Vector2(
                Player.Transform.Translation.X - Transform.Translation.X,
                Player.Transform.Translation.Z - Transform.Translation.Z);

            if (vecToPlayer.LengthSquared() > Distance * Distance)
            {
                vecToPlayer.Normalize();

                var position = Transform.Translation;
                position.X += vecToPlayer.X * Speed;
                position.Z += vecToPlayer.Y * Speed;
                Transform.Translation = position;
            }
        }
    }
}
