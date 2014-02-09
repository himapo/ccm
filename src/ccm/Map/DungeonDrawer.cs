using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Camera;
using HimaLib.Debug;
using ccm.Render;

namespace ccm.Map
{
    public class DungeonDrawer : IDungeonDrawer
    {
        public CameraBase Camera { get; set; }

        public bool IsLightCulling { get; set; }

        SimpleInstancingRenderParameter RenderParam = new SimpleInstancingRenderParameter();

        FrustumCulling FrustumCulling = new FrustumCulling();

        public DungeonDrawer()
        {
            InitRenderParam();
        }

        void InitRenderParam()
        {
            RenderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
        }

        public void DrawMapCube(IModel model, bool updated, List<Matrix> transforms)
        {
            FrustumCulling.UpdateFrustum(Camera);

            RenderParam.Camera = Camera;
            RenderParam.TransformsUpdated = updated;
            RenderParam.InstanceTransforms = transforms.Where(matrix =>
            {
                return
                    IsLightCulling ?
                    FrustumCulling.IsCulledLight(matrix, 3.0f) :
                    FrustumCulling.IsCulled(matrix, 3.0f);
            });

            RenderManagerAccessor.Instance.RenderModel(model, RenderParam);
        }
    }
}
