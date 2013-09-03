using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Light
{
    public class SpotLight
    {
        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        public float Length { get; set; }

        public Color Color { get; set; }

        public SpotLight()
        {
            Position = Vector3.Zero;
            Direction = Vector3.Up;
            Length = 10.0f;
            Color = Color.White;
        }
    }
}
