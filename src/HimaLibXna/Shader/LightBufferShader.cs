using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class LightBufferShader
    {
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public Matrix World { get; set; }
        
        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public Vector3 DirLight0Direction { get; set; }

        public Vector3 DirLight0DiffuseColor { get; set; }

        public Vector3 PointLightPosition { get; set; }

        public float PointLightAttenuationBegin { get; set; }

        public float PointLightAttenuationEnd { get; set; }      

        public Vector3 PointLightColor { get; set; }

        public Texture2D NormalDepthMap { get; set; }

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        Effect Effect;

        HudBillboard HudBillboard = new HudBillboard();

        public LightBufferShader()
        {
            World = Matrix.Identity;
            View = Matrix.Identity;
            Projection = Matrix.Identity;

            var contentLoader = new Content.EffectLoader();
            Effect = contentLoader.Load("Effect/LightBuffer");
        }

        public void SetRenderTargetSize(float x, float y)
        {
            HudBillboard.RectSize = new Vector2(x, y);
            HudBillboard.UpdateVertices();
        }

        public void RenderPoint()
        {
            SetUpEffect("Point");

            Effect.Parameters["gPointLight"].StructureMembers["Position"].SetValue(PointLightPosition);
            Effect.Parameters["gPointLight"].StructureMembers["AttenuationBegin"].SetValue(PointLightAttenuationBegin);
            Effect.Parameters["gPointLight"].StructureMembers["Color"].SetValue(PointLightColor);
            Effect.Parameters["gPointLight"].StructureMembers["AttenuationEnd"].SetValue(PointLightAttenuationEnd);

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                    GraphicsDevice.Indices = part.IndexBuffer;

                    foreach (var pass in Effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                            0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public void RenderSpot()
        {
        }

        public void RenderDirectional()
        {
            SetUpEffect("Directional");

            Effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
            Effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);

            HudBillboard.Render(Effect);
        }

        void SetUpEffect(string techniqueName)
        {
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            Effect.Parameters["InvView"].SetValue(Matrix.Invert(View));
            Effect.Parameters["InvProj"].SetValue(Matrix.Invert(Projection));

            Effect.Parameters["NormalDepthMap"].SetValue(NormalDepthMap);

            Effect.CurrentTechnique = Effect.Techniques[techniqueName];
        }
    }
}
