﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

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
    }
}