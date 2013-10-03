using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    /// <summary>
    /// プラットフォーム依存の描画機能を抽象化するインターフェース
    /// </summary>
    public interface IRenderDevice
    {
        void ClearAll(Color color);

        void ClearColor(Color color);

        void ClearDepth();

        void SetDepthState(bool depthTestEnabled, bool depthWriteEnabled);

        void SetRenderTarget(int index);

        void SetRenderTargets(int[] indices);
    }
}
