using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;
using HimaLib.Camera;

namespace ccm.Player
{
    public class PlayerDrawer : IPlayerDrawer
    {
        public ICamera Camera { get; set; }

        MMDXModelRenderer renderer = new MMDXModelRenderer();

        public PlayerDrawer()
        {
        }

        public void Draw(string modelName, List<string> attackmentNames, AffineTransform transform)
        {
            renderer.SetUp(modelName);

            renderer.Transform = transform;

            renderer.Camera = Camera;

            renderer.Render();
        }
    }
}
