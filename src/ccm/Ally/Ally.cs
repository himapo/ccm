using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Ally
{
    public class Ally
    {
        public IModel Model { get; set; }

        public AffineTransform Transform { get; set; }

        public IAllyUpdater Updater { get; set; }

        public IAllyDrawer Drawer { get; set; }

        public Ally()
        {
        }

        public void Update()
        {
            Updater.Update(this);
        }

        public void Draw()
        {
            Drawer.Draw(Model, Transform);
        }

    }
}
