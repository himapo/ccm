using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class SimpleInstancingRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.SimpleInstancing; } }

        public ICamera Camera { get; set; }

        public List<AffineTransform> Transforms { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

        public Vector3 DirLight0SpecularColor { get; set; }
    }
}
