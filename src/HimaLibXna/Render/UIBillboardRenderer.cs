using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Shader;
using HimaLib.System;
using HimaLib.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class UIBillboardRenderer
    {
        public string TextureName { get; set; }

        public float Alpha { get; set; }

        public float Scale { get; set; }

        public HimaLib.Math.Vector3 Rotation { get; set; }

        public HimaLib.Math.Vector3 Position { get; set; }

        ConstantShader Shader { get; set; }

        TextureLoader TextureLoader { get; set; }

        public UIBillboardRenderer()
        {
            Shader = new ConstantShader();
            TextureLoader = new TextureLoader();
        }

        public void Render()
        {
            // TODO : シェーダにパラメータを渡す
            Shader.Texture = GetTexture();
            Shader.World = GetWorldMatrix();
            Shader.View = GetViewMatrix();
            Shader.Projection = GetProjMatrix();
            Shader.Alpha = Alpha;

            Shader.RenderBillboard();
        }

        Texture2D GetTexture()
        {
            return TextureLoader.Load(TextureName);
        }

        Matrix GetWorldMatrix()
        {
            var result = Matrix.CreateScale(Scale);
            result *= Matrix.CreateRotationZ(Rotation.Z);
            result *= Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);
            return result;
        }

        Matrix GetViewMatrix()
        {
            return Matrix.Identity;
        }

        Matrix GetProjMatrix()
        {
            return new Matrix(
                2.0f / SystemProperty.ScreenWidth, 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f / SystemProperty.ScreenHeight, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
        }
    }
}
