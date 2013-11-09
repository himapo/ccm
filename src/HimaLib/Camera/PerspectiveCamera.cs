using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Camera
{
    public class PerspectiveCamera : CameraBase
    {
        public float FovY { get; set; }

        public float Aspect { get; set; }

        public override Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(MathUtil.ToRadians(FovY), Aspect, Near, Far);
            }
        }

        public PerspectiveCamera()
        {
            FovY = 37.8f;   // 焦点距離35mm
            Aspect = 16.0f / 9.0f;
        }
    }
}
