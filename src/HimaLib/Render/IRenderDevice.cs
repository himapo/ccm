using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    /// <summary>
    /// プラットフォーム依存の描画機能を抽象化するインターフェース
    /// </summary>
    public interface IRenderDevice
    {
        void ClearDepth();

        void SetDepthState(bool depthTestEnabled, bool depthWriteEnabled);

        void SetRenderTarget(int index);
    }
}
