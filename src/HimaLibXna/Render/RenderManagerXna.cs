using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimaLib.System;

namespace HimaLib.Render
{
    class RenderManagerXna : RenderManager
    {
        public override void StartRender()
        {
            if (AsyncRender && RenderTask != null && !RenderTask.IsCompleted)
            {
                RenderTask.Wait();
            }

            IncrementBuffer();

            CopyPrevBuffer();

            ClearBuffer();

            if (AsyncRender)
            {
                RenderTask = Task.Factory.StartNew(Render);
            }
        }

        public override void WaitRender()
        {
            if (AsyncRender)
            {
                RenderTask.Wait();
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
