using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Updater;
using HimaLib.System;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using ccm.Render;

namespace ccm.Particle
{
    public abstract class Particle
    {
        IBillboard Billboard = BillboardFactory.Instance.Create();

        protected abstract BillboardRenderParameter BillboardRenderParameter { get; }

        protected virtual HimaLib.Updater.UpdaterManager UpdaterManager
        {
            // デフォルトはグローバルインスタンスを使う
            get { return HimaLib.Updater.UpdaterManager.Instance; }
        }

        public virtual bool Update()
        {
            return false;
        }

        public virtual void Draw()
        {
            RenderManager.Instance.RenderBillboard(Billboard, BillboardRenderParameter);
        }
    }
}
