using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class SimpleModelRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.Simple; } }

        public ICamera Camera { get; set; }

        public float Alpha { get; set; }

        public IVector3 AmbientLightColor { get; set; }

        public IVector3 DirLight0Direction { get; set; }

        public IVector3 DirLight0DiffuseColor { get; set; }

        public SimpleModelRenderParameter()
        {
        }
    }
}
