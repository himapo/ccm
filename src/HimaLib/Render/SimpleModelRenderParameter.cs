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

        public CameraBase Camera { get; set; }

        public AffineTransform Transform { get; set; }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

        public SimpleModelRenderParameter()
        {
            Transform = new AffineTransform();
            Alpha = 1.0f;
            AmbientLightColor = Vector3.One * 0.4f;
            DirLight0Direction = Vector3.One * -1.0f;
            DirLight0DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
        }
    }
}
