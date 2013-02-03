using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Enemy
{
    public class EnemyManager
    {
        List<Enemy> Enemys = new List<Enemy>();

        public EnemyManager()
        {
        }

        public void Add(Enemy enemy)
        {
            Enemys.Add(enemy);
        }

        public void Update()
        {
            foreach (var enemy in Enemys)
            {
                enemy.Update();
            }
        }

        public void Draw()
        {
            foreach (var enemy in Enemys)
            {
                enemy.Draw();
            }
        }
    }
}
