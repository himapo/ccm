using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IMouse
    {
        bool EnableOnBackGround { get; set; }

        /// <summary>
        /// ゲーム画面中心を原点とするピクセル座標を返す
        /// </summary>
        int X { get; set; }

        int Y { get; set; }

        int Wheel { get; set; }

        bool IsLeftButtonDown();

        bool IsRightButtonDown();

        bool IsMiddleButtonDown();
    }
}
