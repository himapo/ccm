using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using HimaLib.Collision;
using HimaLib.Camera;
using ccm.Player;
using ccm.Collision;
using ccm.Deco;

namespace ccm.Ally
{
    public class DungeonAllyUpdater : StateMachine, IAllyUpdater
    {
        // 味方アクセス
        public ccm.Ally.AllyManager AllyManager { get; set; }

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
        Ally Ally;

        IModel Model
        {
            get { return Ally.Model; }
            set { }
        }

        AffineTransform Transform
        {
            get { return Ally.Transform; }
            set { }
        }

        AffineTransform PrevTransform = new AffineTransform();

        HimaLib.Collision.CylinderCollisionPrimitive BodyCollisionPrimitive;

        CollisionReactor BodyCollisionReactor;

        HimaLib.Collision.CollisionInfo BodyCollision;

        HimaLib.Collision.SphereCollisionPrimitive DamageCollisionPrimitive;

        AttackCollisionReactor DamageCollisionReactor;

        HimaLib.Collision.CollisionInfo DamageCollision;

        int Frame = 0;

        float Speed;

        float Distance;

        public DungeonAllyUpdater()
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
                Group = () => (int)ccm.Collision.CollisionGroup.AllyBody,
                Reactor = BodyCollisionReactor,
            };

            DamageCollisionPrimitive = new SphereCollisionPrimitive()
            {
                Center = () => Transform.Translation,
                Radius = () => 3.0f,
            };

            DamageCollisionReactor = new AttackCollisionReactor()
            {
                AttackReaction = (id, count, actor) =>
                {
                    UpdateState = UpdateStateTerm;

                    DecoManager.Add(new ccm.Deco.Deco_Twister(Transform, Camera, GameRand));
                }
            };

            DamageCollision = new HimaLib.Collision.CollisionInfo()
            {
                Active = () => true,
                Group = () => (int)ccm.Collision.CollisionGroup.AllyDamage,
                Reactor = DamageCollisionReactor,
            };

            Speed = GameRand.NextFloat() * 0.2f + 0.4f;

            Distance = GameRand.NextFloat() * 40.0f + 10.0f;

            UpdateState = UpdateStateInit;
        }

        public void Update(Ally enemy)
        {
            Ally = enemy;
            PrevTransform = new AffineTransform(Transform);

            Update();
        }

        void UpdateStateInit()
        {
            InitCollision();
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
            AllyManager.DeleteAlly(Ally);
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
