using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ccm
{
    enum ShaderLabel
    {
        Constant,
        Lambert,
        HalfLambert,
        Phong,
        Toon,
        ShadowMap,
        Cube,
        InstancingPhong,
    }

    interface IShaderService
    {
        Dictionary<ShaderLabel, Shader> Shaders { get; set; }
    }
}
