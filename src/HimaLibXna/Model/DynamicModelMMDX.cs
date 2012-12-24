using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA.Model;
using HimaLib.Render;
using HimaLib.Math;

namespace HimaLib.Model
{
    public class DynamicModelMMDX : IModel
    {
        public string Name { get; set; }

        public MMDXModel Model { get; set; }

        AffineTransform transform = new AffineTransform();

        public DynamicModelMMDX()
        {
        }

        public void ChangeMotion()
        {
        }

        public void Render(IModelRenderParameter param)
        {
            var renderer = ModelRendererFactoryMMDX.Instance.Create(param);
            renderer.Render(Model, transform);
        }
    }
}
