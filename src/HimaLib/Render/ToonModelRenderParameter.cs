﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;

namespace HimaLib.Render
{
    public class ToonModelRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.Toon; } }

        public ICamera Camera { get; set; }

        public ToonModelRenderParameter()
        {
        }
    }
}