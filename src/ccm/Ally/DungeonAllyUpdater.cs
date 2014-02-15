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
        public CameraBase Camera { get; set; }

        // デコ
        public ccm.Deco.DecoManager DecoManager { get; set; }

        // システム
        HimaLib.Math.IRand GameRand { get { return System.GameRand.Instance; } }

        float UpdateTimeScale { get { return TimeKeeper.Instance.LastTimeScale; } }

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

        AllyBodyCollisionInfo BodyCollision;

        AllyDamageCollisionInfo DamageCollision;

        int Frame = 0;

        float Speed;

        float ScaledSpeed { get { return Speed * UpdateTimeScale; } }

        float Distance;

        public DungeonAllyUpdater()
        {
            BodyCollision = new AllyBodyCollisionInfo()
            {
                Base = () => { return new Vector3(Transform.Translation - Vector3.UnitY * 2.0f); },
                Reaction = (id, count, overlap) =>
                {
                    Transform.Translation = PrevTransform.Translation;
                },
            };

            DamageCollision = new AllyDamageCollisionInfo()
            {
                Center = () => Transform.Translation,
                AttackReaction = (id, count, actor, overlap) =>
                {
                    UpdateState = UpdateStateTerm;

                    DecoManager.Add(new ccm.Deco.Deco_Twister(Transform, Camera, GameRand));
                },
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
            CollisionManager.Add(BodyCollision);
            CollisionManager.Add(DamageCollision);
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
                position.X += vecToPlayer.X * ScaledSpeed;
                position.Z += vecToPlayer.Y * ScaledSpeed;
                Transform.Translation = position;
            }
        }
    }
}
