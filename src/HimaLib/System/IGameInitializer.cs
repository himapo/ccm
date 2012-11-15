using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib
{
    public interface IGameInitializer
    {
        int ScreenWidth { get; set; }

        int ScreenHeight { get; set; }

        bool MSAAEnable { get; set; }

        int FPS { get; set; }

        bool FixedFrameRate { get; set; }

        bool MouseVisible { get; set; }

        void Initialize();
    }
}
