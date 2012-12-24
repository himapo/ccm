using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;
using HimaLib.Camera;
using HimaLib.Model;

namespace ccm.Player
{
    public class PlayerDrawer : IPlayerDrawer
    {
        public ICamera Camera { get; set; }

        ToonModelRenderParameter renderParam = new ToonModelRenderParameter();

        public PlayerDrawer()
        {
        }

        public void Draw(IModel model, AffineTransform transform)
        {
            renderParam.Camera = Camera;

            model.Render(renderParam);
        }

    }
}
