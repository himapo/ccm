﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using HimaLib.Collision;
using ccm.Player;
using ccm.Enemy;

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

        float Speed = GameProperty.gameRand.NextFloat() * 0.2f + 0.4f;

        float Distance = GameProperty.gameRand.NextFloat() * 40.0f + 10.0f;

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

        HimaLib.Collision.CollisionInfo BodyCollision;

        int Frame = 0;

        public DungeonEnemyUpdater()
        {
            BodyCollisionPrimitive = new CylinderCollisionPrimitive()
            {
                Base = () => { return new Vector3(Transform.Translation - Vector3.UnitY * 2.0f); },
                Radius = () => 2.0f,
                Height = () => 4.0f,
            };

            BodyCollision = new HimaLib.Collision.CollisionInfo()
            {
                Active = () => true,
                Primitives = new List<ICollisionPrimitive>(),
                Group = () => (int)ccm.Collision.CollisionGroup.EnemyBody,
                PreReaction = (id, count) => { },
                Reaction = (id, count) => 
                { 
                    Transform.Translation = PrevTransform.Translation; 
                },
            };

            UpdateState = UpdateStateInit;
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
            UpdateState = UpdateStateMain;
        }

        void InitCollision()
        {
            BodyCollision.Primitives.Clear();
            BodyCollision.Primitives.Add(BodyCollisionPrimitive);
            CollisionManager.Add(BodyCollision);
        }

        void UpdateStateMain()
        {
            MoveToPlayer();

            if (++Frame == 600)
            {
                UpdateState = UpdateStateTerm;
            }
        }

        void UpdateStateTerm()
        {
            CollisionManager.Remove(BodyCollision);
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