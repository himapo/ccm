using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Enemy
{
    public enum EnemyType
    {
        Cube,
        //Seeker,
    }

    public class EnemyCreator
    {
        Dictionary<EnemyType, string> ModelNameDic = new Dictionary<EnemyType,string>();

        public EnemyCreator()
        {
            ModelNameDic[EnemyType.Cube] = "cube003";
        }

        public Enemy Create(
            EnemyType type, 
            AffineTransform transform,
            IEnemyUpdater updater,
            IEnemyDrawer drawer)
        {
            return new Enemy()
            {
                Model = LoadModel(type),
                Transform = transform,
                Updater = updater,
                Drawer = drawer
            };
        }

        IModel LoadModel(EnemyType type)
        {
            return ModelFactory.Instance.Create(ModelNameDic[type]);
        }
    }
}
