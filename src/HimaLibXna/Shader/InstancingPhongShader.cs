using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Shader
{
    public class InstancingPhongShader : Graphics.GraphicsDeviceUser
    {
        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }
        
        public Matrix[] ModelBones { get; set; }
        
        public Matrix[] InstanceTransforms { get; set; }

        public Matrix View { get; set; }
        
        public Matrix Projection { get; set; }

        public Vector3 AmbientLightColor { get; set; }
        
        public Vector3 DirLight0Direction { get; set; }
        
        public Vector3 DirLight0DiffuseColor { get; set; }

        public Vector3 DirLight0SpecularColor { get; set; }

        public Vector3 EyePosition { get; set; }

        Effect effect;

        DynamicVertexBuffer instanceVertexBuffer;

        // インスタンス毎のトランスフォーム行列を頂点バッファーに保存するためにカスタムな頂点タイプを作成します。
        // これは 4x4 のマトリクスを 4 つの Vector4 型としてエンコードします。
        static VertexDeclaration instanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
        );

        public InstancingPhongShader()
        {
            var contentLoader = new Content.EffectLoader();
            effect = contentLoader.Load("Effect/HardwareInstancingPhong");
        }

        public void RenderModel()
        {
            if (InstanceTransforms.Length == 0)
                return;

            SetupVertexBuffer();

            SetupEffect("PixelLighting");

            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(part.VertexBuffer, part.VertexOffset, 0),
                        new VertexBufferBinding(instanceVertexBuffer, 0, 1)
                        );

                    GraphicsDevice.Indices = part.IndexBuffer;

                    CopyMaterial(part.Effect as BasicEffect);

                    effect.Parameters["World"].SetValue(ModelBones[mesh.ParentBone.Index]);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GraphicsDevice.DrawInstancedPrimitives(
                            PrimitiveType.TriangleList,
                            0,
                            0,
                            part.NumVertices,
                            part.StartIndex,
                            part.PrimitiveCount,
                            InstanceTransforms.Length);
                    }
                }
            }
        }

        public void RenderBillboard()
        {
        }

        void SetupVertexBuffer()
        {
            // 頂点バッファーに必要なインスタンスを保持するための容量が足りない場合、バッファー サイズを増やす。
            if ((instanceVertexBuffer == null) ||
                (InstanceTransforms.Length > instanceVertexBuffer.VertexCount))
            {
                if (instanceVertexBuffer != null)
                    instanceVertexBuffer.Dispose();

                instanceVertexBuffer = new DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration,
                                                               InstanceTransforms.Length, BufferUsage.WriteOnly);
            }

            // 最新のトランスフォーム行列を instanceVertexBuffer へコピーする。
            instanceVertexBuffer.SetData(InstanceTransforms, 0, InstanceTransforms.Length, SetDataOptions.Discard);
        }

        void SetupEffect(string techniqueName)
        {
            effect.CurrentTechnique = effect.Techniques[techniqueName];

            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);

            effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);
            effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
            effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);
            effect.Parameters["DirLight0SpecularColor"].SetValue(DirLight0SpecularColor);

            effect.Parameters["EyePosition"].SetValue(EyePosition);
        }

        void CopyMaterial(BasicEffect src)
        {
            if (src == null)
                return;

            effect.Parameters["DiffuseColor"].SetValue(src.DiffuseColor);
            effect.Parameters["Alpha"].SetValue(src.Alpha);
            effect.Parameters["EmissiveColor"].SetValue(src.EmissiveColor);
            effect.Parameters["SpecularColor"].SetValue(src.SpecularColor);
            effect.Parameters["SpecularPower"].SetValue(src.SpecularPower);
        }
    }
}
