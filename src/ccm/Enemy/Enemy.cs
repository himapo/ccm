using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Enemy
{
    public class Enemy
    {
        public IModel Model { get; set; }

        public AffineTransform Transform { get; set; }

        public IEnemyUpdater Updater { get; set; }

        public IEnemyDrawer Drawer { get; set; }

        // AI

        // スキル情報

        public Enemy()
        {
        }

        public void Update()
        {
            Updater.Update(Model, Transform);
        }

        public void Draw()
        {
            Drawer.Draw(Model, Transform);
        }
    }
}
