using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using ccm.Player;

namespace ccm.Enemy
{
    public class DungeonEnemyUpdater : IEnemyUpdater
    {
        // プレイヤー情報
        public Player.Player Player { get; set; }

        // ダンジョン情報

        float Speed = GameProperty.gameRand.NextFloat() * 0.2f + 0.4f;

        float Distance = GameProperty.gameRand.NextFloat() * 40.0f + 10.0f;

        public void Update(IModel model, AffineTransform transform)
        {
            MoveToPlayer(transform);
        }

        void MoveToPlayer(AffineTransform transform)
        {
            var vecToPlayer = new Vector2(
                Player.Transform.Translation.X - transform.Translation.X,
                Player.Transform.Translation.Z - transform.Translation.Z);

            if (vecToPlayer.LengthSquared() > Distance * Distance)
            {
                vecToPlayer.Normalize();

                var position = transform.Translation;
                position.X += vecToPlayer.X * Speed;
                position.Z += vecToPlayer.Y * Speed;
                transform.Translation = position;
            }
        }
    }
}
