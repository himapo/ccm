﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public interface IBillboardRendererXna
    {
        void SetParameter(IBillboardRenderParameter param);

        void Render();
    }
}
