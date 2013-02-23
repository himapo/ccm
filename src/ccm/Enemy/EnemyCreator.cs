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
        public Func<IEnemyUpdater> UpdaterCreator { get; set; }

        public Func<IEnemyDrawer> DrawerCreator { get; set; }

        Dictionary<EnemyType, string> ModelNameDic = new Dictionary<EnemyType,string>();

        public EnemyCreator()
        {
            ModelNameDic[EnemyType.Cube] = "cube003";
        }

        public Enemy Create(
            EnemyType type, 
            AffineTransform transform)
        {
            return new Enemy()
            {
                Model = LoadModel(type),
                Transform = transform,
                Updater = UpdaterCreator(),
                Drawer = DrawerCreator()
            };
        }

        IModel LoadModel(EnemyType type)
        {
            return ModelFactory.Instance.Create(ModelNameDic[type]);
        }
    }
}
