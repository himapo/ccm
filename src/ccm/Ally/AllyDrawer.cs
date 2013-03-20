using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Camera;

namespace ccm.Ally
{
    public class AllyDrawer : IAllyDrawer
    {
        public ICamera Camera { get; set; }

        SimpleModelRenderParameter RenderParam = new SimpleModelRenderParameter();

        public AllyDrawer()
        {
            InitRenderParam();
        }

        void InitRenderParam()
        {
            RenderParam.Alpha = 1.0f;
            RenderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            RenderParam.DirLight0Direction = -Vector3.One;
            RenderParam.DirLight0Direction.Normalize();
            RenderParam.DirLight0DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
            //RenderParam.DirLight0SpecularColor = Vector3.One;
        }

        public void Draw(IModel model, AffineTransform transform)
        {
            RenderParam.Camera = Camera;
            RenderParam.Transform = transform;
            //RenderParam.Alpha = 0.5f;
            model.Render(RenderParam);
        }
    }
}
