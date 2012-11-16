﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Math;

namespace ccm.Camera
{
    public class BasicCamera : ICamera
    {
        public IVector3 Eye { get; set; }

        public IVector3 At { get; set; }

        public IVector3 Up { get; set; }

        public float FovY { get; set; }

        public float Aspect { get; set; }

        public float Near { get; set; }

        public float Far { get; set; }

        public BasicCamera()
        {
            Eye = Vector3.Zero;
            At = Vector3.Forward;
            Up = Vector3.Up;
            FovY = 30.0f;
            Aspect = 16.0f / 9.0f;
            Near = 1.0f;
            Far = 1000.0f;
        }
    }
}