using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using ccm.Enemy;

namespace ccm.Dungeon
{
    public class Dungeon
    {
        public int Floor { get; set; }

        EnemyCreator EnemyCreator = new EnemyCreator();

        ccm.Enemy.EnemyManager EnemyManager = new ccm.Enemy.EnemyManager();

        DungeonEnemyUpdater EnemyUpdater = new DungeonEnemyUpdater();

        EnemyDrawer EnemyDrawer = new EnemyDrawer();

        public Dungeon()
        {
            
        }

        public void Update()
        {
            EnemyManager.Update();
        }

        public void Draw()
        {
            EnemyManager.Draw();
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
