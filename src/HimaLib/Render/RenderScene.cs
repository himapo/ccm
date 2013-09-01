using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;

namespace HimaLib.Render
{
    public class ModelInfo
    {
        public IModel Model { get; set; }
        public IModelRenderParameter RenderParam { get; set; }
    }

    public class RenderScene
    {
        Dictionary<int, IRenderPath> PathDic = new Dictionary<int, IRenderPath>();

        List<ModelInfo> ModelInfoList = new List<ModelInfo>();

        public RenderScene()
        {
        }

        public void AddPath(int index, IRenderPath path)
        {
            PathDic[index] = path;
        }

        public IRenderPath GetPath(int index)
        {
            return PathDic[index];
        }

        public void RemovePath(int index)
        {
            PathDic.Remove(index);
        }

        public void RenderModel(IModel model, IModelRenderParameter renderParam)
        {
            ModelInfoList.Add(new ModelInfo() { Model = model, RenderParam = renderParam });
        }

        public void Render()
        {
            // TODO : 別スレッド化
            InnerRender();
        }

        void InnerRender()
        {
            foreach (var path in PathDic.Values)
            {
                path.ModelInfoList = ModelInfoList;
                path.Render();
            }

            ModelInfoList.Clear();
        }
    }
}
