using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.Math;

namespace HimaLib.Model
{
    public class StaticModelXna : IModel
    {
        public string Name { get; set; }

        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        AffineTransform transform = new AffineTransform();

        public StaticModelXna()
        {
        }

        public void Render(IModelRenderParameter param)
        {
            var renderer = ModelRendererFactoryXna.Instance.Create(param);
            renderer.Render(Model, transform);
        }
    }
}
