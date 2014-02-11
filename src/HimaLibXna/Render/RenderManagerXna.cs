using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;
using HimaLib.Debug;

namespace HimaLib.Render
{
    class RenderManagerXna : RenderManager
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        bool IsDrawCalled = true;

        public RenderManagerXna()
        {
            GraphicsDevice.DeviceLost += new EventHandler<EventArgs>((o, args) =>
            {
                DebugPrint.PrintLine("DeviceLost");
            });

            GraphicsDevice.DeviceResetting += new EventHandler<EventArgs>((o, args) =>
            {
                DebugPrint.PrintLine("DeviceResetting");
            });

            GraphicsDevice.DeviceReset += new EventHandler<EventArgs>((o, args) =>
            {
                DebugPrint.PrintLine("DeviceReset");
            });
        }

        public override void StartRender()
        {
            // 最小化中などでDrawが呼ばれていないときはレンダリングしない
            // ロストしたリソースにアクセスする可能性がある
            if (!IsDrawCalled)
            {
                return;
            }

            IncrementBuffer();

            CopyPrevBuffer();

            ClearBuffer();

            if (AsyncRender)
            {
                RenderTask = Task.Factory.StartNew(Render);
                IsDrawCalled = false;
            }
        }

        public override void WaitRender()
        {
            if (AsyncRender)
            {
                RenderTask.Wait();
                IsDrawCalled = true;
            }
            else
            {
                Render();
            }
        }
    }

    public class RenderManagerFactory
    {
        public static RenderManagerFactory Instance
        {
            get
            {
                return Singleton<RenderManagerFactory>.Instance;
            }
        }

        RenderManagerFactory()
        {
        }

        public RenderManager Create()
        {
            return new RenderManagerXna();
        }
    }
}
