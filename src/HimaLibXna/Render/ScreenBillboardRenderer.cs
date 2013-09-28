using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Math;

namespace HimaLib.Render
{
    /// <summary>
    /// スクリーン全体の板ポリを描くレンダラ
    /// </summary>
    public abstract class ScreenBillboardRenderer : IBillboardRendererXna
    {
        protected float ScreenWidth { get { return SystemProperty.ScreenWidth; } }

        protected float ScreenHeight { get { return SystemProperty.ScreenHeight; } }

        public ScreenBillboardRenderer()
        {
        }

        public abstract void SetParameter(BillboardRenderParameter p);

        public abstract void Render();

        protected Matrix GetProjMatrix()
        {
            return new Matrix(
                2.0f / ScreenWidth, 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / ScreenHeight, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
