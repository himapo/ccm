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

        public RenderTarget2D GetRenderTarget(int index, int width, int height)
        {
            RenderTarget2D result;

            if (!RenderTargetDic.TryGetValue(index, out result))
            {
                result = new RenderTarget2D(GraphicsDevice, width, height);
                RenderTargetDic[index] = result;
            }

            return result;
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
