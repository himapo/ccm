using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.System;
using HimaLib.Model;
using HimaLib.Light;

namespace ccm.Render
{
    public class RenderSceneManager
    {
        public static RenderSceneManager Instance
        {
            get
            {
                return Singleton<RenderSceneManager>.Instance;
            }
        }

        RenderScene RenderScene { get; set; }

        RenderSceneManager()
        {
            RenderScene = new RenderScene();
        }

        public void AddPath(RenderPathType index, IRenderPath path)
        {
            RenderScene.AddPath((int)index, path);
        }

        public IRenderPath GetPath(RenderPathType index)
        {
            return RenderScene.GetPath((int)index);
        }

        public void RemovePath(RenderPathType index)
        {
            RenderScene.RemovePath((int)index);
        }

        public void AddDirectionalLight(DirectionalLight light)
        {
            RenderScene.AddDirectionalLight(light);
        }

        public void RemoveDirectionalLight(DirectionalLight light)
        {
            RenderScene.RemoveDirectionalLight(light);
        }

        public void ClearDirectionalLight()
        {
            RenderScene.ClearDirectionalLight();
        }

        public void RenderModel(IModel model, IModelRenderParameter renderParam)
        {
            RenderScene.RenderModel(model, renderParam);
        }

        public void Render()
        {
            RenderScene.Render();
        }
    }
}
