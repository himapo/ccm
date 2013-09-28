﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Render
{
    public enum RenderTargetType
    {
        BackBuffer = 0,
        ShadowMap0,
        DiffuseLightMap,
        SpecularLightMap,
        GBuffer0,
        GBuffer1,
        GBuffer2,
        GBuffer3,

        Length,
    }
}