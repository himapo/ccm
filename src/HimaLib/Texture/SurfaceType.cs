using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Texture
{
    public enum SurfaceType
    {
        // Integer format
        A8R8G8B8,

        // Float format
        R32F,
        A32B32G32R32F,

        // Depth format
        D16,
        D24,
        D24S8,
    }
}
