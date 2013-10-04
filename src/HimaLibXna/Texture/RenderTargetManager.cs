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

        public void AddRenderTarget(int index, int width, int height, SurfaceType colorFormat, bool depthEnable, bool stencilEnable)
        {
            var depthFormat = DepthFormat.None;
            if (stencilEnable)
            {
                depthFormat = DepthFormat.Depth24Stencil8;
            }
            else if (depthEnable)
            {
                depthFormat = DepthFormat.Depth24;
            }

            RenderTargetDic[index] = new RenderTarget2D(
                GraphicsDevice,
                width,
                height,
                false,
                ToXnaSurfaceFormat(colorFormat),
                depthFormat,
                1,
                RenderTargetUsage.PreserveContents);    // 深度バッファを保存するため
        }

        SurfaceFormat ToXnaSurfaceFormat(SurfaceType format)
        {
            switch (format)
            {
                case SurfaceType.A8R8G8B8:
                    return SurfaceFormat.Color;
                case SurfaceType.R32F:
                    return SurfaceFormat.Single;
                case SurfaceType.A32B32G32R32F:
                    return SurfaceFormat.Vector4;
                default:
                    break;
            }

            throw new NotImplementedException();
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
