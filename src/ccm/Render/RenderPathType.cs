﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Render
{
    public enum RenderPathType
    {
        SHADOW,
        GBUFFER,
        LIGHTBUFFER,
        DEFERRED,
        OPAQUE,
        DEBUG,
        TONEMAPPING,
        TRANSLUCENT,
        HUD,
        DEBUGHUD,
        DEBUGFONT,
    }
}
