using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Camera
{
    public class OrthoCamera : CameraBase
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public override Matrix Projection
        {
            get
            {
                return Matrix.CreateOrthographic(Width, Height, Near, Far);
            }
        }

        public OrthoCamera()
        {
            Width = 160.0f;
            Height = 90.0f;
        }
    }
}
