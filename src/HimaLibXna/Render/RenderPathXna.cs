using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class RenderPathXna : RenderPath
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } set { } }

        protected override void ClearDepth()
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.Gray, 1.0f, 0);
        }

        protected override void SetDepthState(bool depthTestEnabled, bool depthWriteEnabled)
        {
            if (depthWriteEnabled)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else if (depthTestEnabled)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            }
            else
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
            }
        }
    }
}
