using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;

namespace HimaLib.Model
{
    public abstract class Cylinder
    {
        public Vector3 Base { get; set; }

        public float Radius { get; set; }

        public float Height { get; set; }

        public Cylinder()
        {
            Base = Vector3.Zero;
            Radius = 1.0f;
            Height = 1.0f;
        }

        public abstract void Render(CylinderRenderParameter param);
    }
}
