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
        IModel Model;
        
        AffineTransform Transform = new AffineTransform();

        // AI

        // スキル情報

        public Enemy()
        {
        }

        public void InitModel(string modelName)
        {
            Model = ModelFactory.Instance.Create(modelName);
        }

        public void Update(IEnemyUpdater updater)
        {
            updater.Update(Model, Transform);
        }

        public void Draw(IEnemyDrawer drawer)
        {
            drawer.Draw(Model, Transform);
        }
    }
}
