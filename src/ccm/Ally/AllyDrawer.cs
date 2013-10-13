using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Camera;
using HimaLib.Texture;
using ccm.Render;

namespace ccm.Ally
{
    public class AllyDrawer : IAllyDrawer
    {
        public CameraBase Camera { get; set; }

        SimpleModelRenderParameter RenderParam = new SimpleModelRenderParameter();

        public AllyDrawer()
        {
            InitRenderParam();
        }

        void InitRenderParam()
        {
            RenderParam.Alpha = 1.0f;
            RenderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            RenderParam.ShadowMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
        }

        public void Draw(IModel model, AffineTransform transform)
        {
            RenderParam.Camera = Camera;
            RenderParam.Transform = transform.WorldMatrix;
            //RenderParam.Alpha = 0.5f;
            RenderSceneManager.Instance.RenderModel(model, RenderParam);
        }
    }
}
