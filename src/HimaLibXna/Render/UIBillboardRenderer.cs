using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Texture;
using HimaLib.Math;
using HimaLib.Shader;

namespace HimaLib.Render
{
    public class UIBillboardRenderer
    {
        public Texture2D Texture { get; set; }

        public float Alpha { get; set; }

        public float Scale { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Position { get; set; }

        ConstantShader Shader { get; set; }

        public void Render()
        {
            // TODO : シェーダにパラメータを渡す

            Shader.RenderBillboard();
        }
    }
}
