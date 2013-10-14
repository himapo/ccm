using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Model
{
    public class Sphere
    {
        public Vector3 Position { get; set; }

        public float Radius { get; set; }

        public Sphere()
        {
            Position = Vector3.Zero;
            Radius = 1.0f;
        }
    }
}
