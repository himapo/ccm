using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Math;

namespace HimaLib.Camera
{
    public abstract class CameraBase
    {
        public Vector3 Eye { get; set; }

        public Vector3 At { get; set; }

        public Vector3 Up { get; set; }

        public float Near { get; set; }

        public float Far { get; set; }

        public Matrix View { get { return Matrix.CreateLookAt(Eye, At, Up); } }

        public abstract Matrix Projection { get; }

        public CameraBase()
        {
            Eye = Vector3.Zero;
            At = Vector3.Forward;
            Up = Vector3.Up;
            Near = 1.0f;
            Far = 1000.0f;
        }
    }
}
