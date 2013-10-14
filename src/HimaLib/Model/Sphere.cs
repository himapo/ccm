using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;

namespace HimaLib.Model
{
    public abstract class Sphere
    {
        public Vector3 Position { get; set; }

        public float Radius { get; set; }

        public Sphere()
        {
            Position = Vector3.Zero;
            Radius = 1.0f;
        }

        public abstract void Render(SphereRenderParameter param);
    }
}
