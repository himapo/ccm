using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Ally
{
    public enum AllyType
    {
        Cube,
    }

    public class AllyCreator
    {
        public Func<IAllyUpdater> UpdaterCreator { get; set; }

        public Func<IAllyDrawer> DrawerCreator { get; set; }

        Dictionary<AllyType, string> ModelNameDic = new Dictionary<AllyType, string>();

        public AllyCreator()
        {
            ModelNameDic[AllyType.Cube] = "cube000";
        }

        public Ally Create(
            AllyType type,
            AffineTransform transform)
        {
            return new Ally()
            {
                Model = LoadModel(type),
                Transform = transform,
                Updater = UpdaterCreator(),
                Drawer = DrawerCreator()
            };
        }

        IModel LoadModel(AllyType type)
        {
            return ModelFactory.Instance.Create(ModelNameDic[type]);
        }
    }
}
