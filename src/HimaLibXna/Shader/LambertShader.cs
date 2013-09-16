using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class LambertShader
    {
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public Texture2D Texture { get; set; }

        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

        public bool ShadowEnabled { get; set; }

        public Texture2D ShadowMap { get; set; }

        public Matrix LightViewProjection { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect effect;

        public LambertShader()
        {
            Texture = new Texture2D(GraphicsDevice, 32, 32);

            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;
            Alpha = 1.0f;

            var contentLoader = new Content.EffectLoader();
            effect = contentLoader.Load("Effect/Lambert");
        }

        public void RenderModel()
        {
            if (ShadowEnabled)
            {
                SetUpEffect("PixelLightingShadow");
            }
            else
            {
                SetUpEffect("PixelLighting");
            }

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                    GraphicsDevice.Indices = part.IndexBuffer;

                    CopyMaterial(part.Effect as BasicEffect);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                            0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public void RenderBillboard()
        {
        }

        void SetUpEffect(string techniqueName)
        {
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["LightViewProjection"].SetValue(LightViewProjection);

            effect.Parameters["DiffuseMap"].SetValue(Texture);
            effect.Parameters["ShadowMap"].SetValue(ShadowMap);

            effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);
            effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
            effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);

            effect.CurrentTechnique = effect.Techniques[techniqueName];
        }

        void CopyMaterial(BasicEffect src)
        {
            if (src == null)
                return;

            effect.Parameters["DiffuseColor"].SetValue(src.DiffuseColor);
            effect.Parameters["Alpha"].SetValue(src.Alpha * Alpha);
        }
    }
}
