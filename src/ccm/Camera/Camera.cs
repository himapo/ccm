using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class Camera
    {
        public Matrix View { get; set; }
        public Matrix Proj { get; set; }

        public Vector3 Eye { get; set; }
        public Vector3 At { get; set; }
        public Vector3 Up { get; set; }
        public Quaternion Rotation { get; set; }

        public float FovY { get; set; }
        public float Aspect { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }

        public Camera()
        {
            View = Matrix.Identity;
            Proj = Matrix.Identity;

            Eye = Vector3.Zero;
            At = Vector3.Forward;   // Z軸負方向（右手系前方）を向いているのが回転0
            Up = Vector3.Up;
            Rotation = Quaternion.Identity;

            FovY = MathHelper.ToRadians(GameProperty.fov);
            Aspect = GameProperty.resolutionWidth / GameProperty.resolutionHeight;
            Near = 1.0f;
            Far = 100.0f;

            Update();
        }

        public void Update()
        {
            View = Matrix.CreateLookAt(Eye, At, Up);
            Proj = Matrix.CreatePerspectiveFieldOfView(FovY, Aspect, Near, Far);
        }
    }

    
}
