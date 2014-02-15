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
using ccm.Battle;

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
        public CameraBase Camera { get; set; }

        // デコ
        public ccm.Deco.DecoManager DecoManager { get; set; }
     
        // システム
        HimaLib.Math.IRand GameRand { get { return System.GameRand.Instance; } }

        float UpdateTimeScale { get { return TimeKeeper.Instance.LastTimeScale; } }

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

        EnemyBodyCollisionInfo BodyCollision;

        EnemyDamageCollisionInfo DamageCollision;

        EnemyAttackCollisionInfo AttackCollision;

        int Frame;

        int HitPoint;

        float Speed;

        float ScaledSpeed { get { return Speed * UpdateTimeScale; } }

        float Distance;

        ComboCounter ComboCounter = new ComboCounter();

        public DungeonEnemyUpdater()
        {
            BodyCollision = new EnemyBodyCollisionInfo()
            {
                Base = () => { return new Vector3(Transform.Translation - Vector3.UnitY * 2.0f); },
                Reaction = (id, count, overlap) =>
                {
                    Transform.Translation = PrevTransform.Translation;
                },
            };

            DamageCollision = new EnemyDamageCollisionInfo()
            {
                Center = () => Transform.Translation,
                AttackReaction = Damage,
            };

            AttackCollision = new EnemyAttackCollisionInfo()
            {
                Active = () => Frame % 360 < 180,
                Center = () => Transform.Translation,                
            };

            Speed = GameRand.NextFloat() * 0.2f + 0.4f;

            Distance = GameRand.NextFloat() * 40.0f + 10.0f;

            UpdateState = UpdateStateInit;
        }

        void Damage(int collisionId, int collisionCount, AttackCollisionActor actor, Vector3 overlap)
        {
            if (HitPoint > 0 && collisionCount == 1)
            {
                HitPoint -= actor.Power;
                DebugPrint.PrintLine("Enemy damage {0}, HP {1}", actor.Power, HitPoint);
                ComboCounter.Damage(actor.Shock);
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
            CollisionManager.Add(BodyCollision);
            CollisionManager.Add(DamageCollision);
            CollisionManager.Add(AttackCollision);
        }

        void UpdateStateMain()
        {
            MoveToPlayer();

            if (++Frame == 6000)
            {
                //UpdateState = UpdateStateTerm;
            }
        }

        void UpdateStateTerm()
        {
            CollisionManager.Remove(BodyCollision);
            CollisionManager.Remove(DamageCollision);
            CollisionManager.Remove(AttackCollision);
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
                position.X += vecToPlayer.X * ScaledSpeed;
                position.Z += vecToPlayer.Y * ScaledSpeed;
                Transform.Translation = position;
            }
        }
    }
}
