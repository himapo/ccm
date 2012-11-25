using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Shader
{
    public class LambertShader : Graphics.GraphicsDeviceUser
    {
        public Model Model { get; set; }

        public Texture2D Texture { get; set; }

        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public float Alpha { get; set; }

        public Vector3 AmbientLightColor { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

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
            SetUpEffect("PixelLighting");

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                    GraphicsDevice.Indices = part.IndexBuffer;

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
            effect.Parameters["Alpha"].SetValue(Alpha);
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["DiffuseMap"].SetValue(Texture);

            effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);
            effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
            effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);

            effect.CurrentTechnique = effect.Techniques[techniqueName];
        }
    }
}
