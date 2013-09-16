using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Texture;
using HimaLib.Camera;
using HimaLib.Math;

namespace HimaLib.Render
{
    /// <summary>
    /// 影を描画するレンダーパラメータはこれから派生する
    /// </summary>
    public abstract class ShadowModelRenderParameter : ModelRenderParameter
    {
        public ITexture ShadowMap { get; set; }

        public CameraBase LightCamera { get; set; }

        AffineTransform transform;
        public AffineTransform Transform
        {
            get { return transform; }
            set
            {
                transform = value;
                DepthModelRenderParameter.Transform = value;
            }
        }

        public DepthModelRenderParameter DepthModelRenderParameter { get; private set; }

        public ShadowModelRenderParameter()
        {
            IsShadowCaster = true;
            IsTranslucent = false;

            DepthModelRenderParameter = new DepthModelRenderParameter();
        }
    }
}
