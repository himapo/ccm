﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public interface IBillboardRendererXna
    {
        void SetParameter(BillboardRenderParameter param);

        void Render();
    }
}
