using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using ccm.Player;

namespace ccm.Enemy
{
    public class DungeonEnemyUpdater : StateMachine, IEnemyUpdater
    {
        // プレイヤー情報
        public Player.Player Player { get; set; }

        // ダンジョン情報

        float Speed = GameProperty.gameRand.NextFloat() * 0.2f + 0.4f;

        float Distance = GameProperty.gameRand.NextFloat() * 40.0f + 10.0f;

        IModel Model;

        AffineTransform Transform;

        public DungeonEnemyUpdater()
        {
            UpdateState = UpdateStateInit;
        }

        public void Update(IModel model, AffineTransform transform)
        {
            Model = model;
            Transform = transform;
            UpdateState();
        }

        void UpdateStateInit()
        {
        }

        void UpdateStateMain()
        {
            MoveToPlayer();
        }

        void UpdateStateTerm()
        {
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
