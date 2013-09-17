using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;

namespace HimaLib.Light
{
    public class DirectionalLight
    {
        public Vector3 Direction { get; set; }

        public Color Color { get; set; }

        public DirectionalLight()
        {
            Direction = -Vector3.Up;
            Color = Color.White;
        }

        public CameraBase ToCamera(CameraBase camera)
        {
            var direction = Direction;
            direction.Normalize();

            return new CameraBase()
            {
                Eye = camera.At - direction * 50.0f,
                At = camera.At,
                Up = Vector3.Up,
                Near = 30.0f,
                Far = 200.0f,
            };
        }
    }
}
