using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Camera;
using ccm.Render;

namespace ccm.Map
{
    public class DungeonDrawer : IDungeonDrawer
    {
        public CameraBase Camera { get; set; }

        SimpleInstancingRenderParameter RenderParam = new SimpleInstancingRenderParameter();

        public DungeonDrawer()
        {
            InitRenderParam();
        }

        void InitRenderParam()
        {
            RenderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            RenderParam.DirLight0SpecularColor = Vector3.One;
        }

        public void DrawMapCube(IModel model, bool updated, List<AffineTransform> transforms)
        {
            RenderParam.Camera = Camera;
            RenderParam.TransformsUpdated = updated;
            RenderParam.Transforms = transforms;

            RenderSceneManager.Instance.RenderModel(model, RenderParam);
        }
    }
}
