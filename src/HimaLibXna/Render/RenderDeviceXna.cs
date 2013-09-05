using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class RenderDeviceXna : IRenderDevice
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        public void ClearDepth()
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.Gray, 1.0f, 0);
        }

        public void SetDepthState(bool depthTestEnabled, bool depthWriteEnabled)
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
