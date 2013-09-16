using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Texture
{
    public class RenderTargetManager
    {
        public static RenderTargetManager Instance
        {
            get
            {
                return Singleton<RenderTargetManager>.Instance;
            }
        }

        Dictionary<int, RenderTarget2D> RenderTargetDic = new Dictionary<int, RenderTarget2D>();

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        HashSet<int> UsedIndices = new HashSet<int>();

        RenderTargetManager()
        {
        }

        public void AddRenderTarget(int index, int width, int height)
        {
            RenderTargetDic[index] = new RenderTarget2D(
                GraphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);
        }

        public void RemoveRenderTarget(int index)
        {
            RenderTargetDic.Remove(index);
        }

        public RenderTarget2D GetRenderTarget(int index)
        {
            return RenderTargetDic[index];
        }

        public void Cleanup()
        {
            var query = from index in RenderTargetDic.Keys
                        where !UsedIndices.Contains(index)
                        select index;

            foreach (var index in query)
            {
                RenderTargetDic.Remove(index);
            }

            UsedIndices.Clear();
        }
    }
}
