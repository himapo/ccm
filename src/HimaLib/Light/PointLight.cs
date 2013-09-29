using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Light
{
    public class PointLight
    {
        public Vector3 Position { get; set; }

        public float AttenuationBegin { get; set; }

        public float AttenuationEnd { get; set; }

        public Color Color { get; set; }

        public PointLight()
        {
            Position = Vector3.Zero;
            AttenuationBegin = 0.0f;
            AttenuationEnd = 10.0f;
            Color = Color.White;
        }
    }
}
