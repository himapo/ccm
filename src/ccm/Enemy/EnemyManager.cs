using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.Enemy
{
    public class EnemyManager
    {
        public EnemyCreator Creator { get; set; }

        List<Enemy> Enemys = new List<Enemy>();

        List<Enemy> DeleteList = new List<Enemy>();

        public EnemyManager()
        {
        }

        public void Update()
        {
            foreach (var enemy in Enemys)
            {
                enemy.Update();
            }

            DeleteList.ForEach((enemy) =>
            {
                Enemys.Remove(enemy);
            });
            DeleteList.Clear();
        }

        public void Draw()
        {
            foreach (var enemy in Enemys)
            {
                enemy.Draw();
            }
        }

        public void CreateEnemy(EnemyType type, AffineTransform transform)
        {
            Enemys.Add(Creator.Create(type, transform));
        }

        public void DeleteEnemy(Enemy enemy)
        {
            DeleteList.Add(enemy);
        }
    }
}
