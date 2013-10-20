using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;

namespace HimaLib.Shader
{
    public class InstancedVertexBuffer
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        public DynamicVertexBuffer VertexBuffer { get; private set; }

        // インスタンス毎のトランスフォーム行列を頂点バッファーに保存するためにカスタムな頂点タイプを作成します。
        // これは 4x4 のマトリクスを 4 つの Vector4 型としてエンコードします。
        static VertexDeclaration instanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
        );

        public void Setup(Matrix[] instanceTransforms)
        {
            // 頂点バッファーに必要なインスタンスを保持するための容量が足りない場合、バッファー サイズを増やす。
            if ((VertexBuffer == null) ||
                (instanceTransforms.Length > VertexBuffer.VertexCount))
            {
                if (VertexBuffer != null)
                    VertexBuffer.Dispose();

                VertexBuffer = new DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration,
                                                               instanceTransforms.Length, BufferUsage.WriteOnly);
            }

            // 最新のトランスフォーム行列を InstanceVertexBuffer へコピーする。
            VertexBuffer.SetData(instanceTransforms, 0, instanceTransforms.Length, SetDataOptions.Discard);
        }
    }
}
