using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using ccm.Enemy;
using ccm.Player;

namespace ccm.Dungeon
{
    public class Dungeon
    {
        // フロア情報
        public int Floor { get; set; }

        public ICamera Camera
        {
            set { EnemyDrawer.Camera = value; }
        }

        public Player.Player Player
        {
            set { EnemyUpdater.Player = value; }
        }

        EnemyCreator EnemyCreator = new EnemyCreator();

        ccm.Enemy.EnemyManager EnemyManager = new ccm.Enemy.EnemyManager();

        DungeonEnemyUpdater EnemyUpdater = new DungeonEnemyUpdater();

        EnemyDrawer EnemyDrawer = new EnemyDrawer();

        int Frame = 0;

        IRand Rand
        {
            get { return GameProperty.gameRand; }
            set { }
        }

        public Dungeon()
        {
            Floor = 1;
        }

        public void Update()
        {
            EnemyManager.Update();

            if (IsTimeToCreateEnemy())
            {
                CreateEnemy(EnemyType.Cube, CalcEnemyAppearPosition());
            }
        }

        public void Draw()
        {
            EnemyManager.Draw();
        }

        bool IsTimeToCreateEnemy()
        {
            if (++Frame >= 120)
            {
                Frame = 0;
                return true;
            }
            return false;
        }

        AffineTransform CalcEnemyAppearPosition()
        {
            return new AffineTransform(
                Vector3.One * 1.5f,
                Vector3.Zero,
                new Vector3(Rand.NextFloat(-100.0f, 100.0f), 1.5f, Rand.NextFloat(-100.0f, 100.0f)));
        }

        void CreateEnemy(EnemyType type, AffineTransform transform)
        {
            var enemy = EnemyCreator.Create(
                type,
                transform,
                EnemyUpdater,
                EnemyDrawer);

            EnemyManager.Add(enemy);
        }
    }
}
