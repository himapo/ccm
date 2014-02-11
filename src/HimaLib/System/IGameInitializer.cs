using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public interface IGameInitializer
    {
        int ScreenWidth { get; }

        int ScreenHeight { get; }

        bool MSAAEnable { get; }

        int FPS { get; }

        bool FixedFrameRate { get; }

        bool VSyncEnable { get; }

        bool MouseVisible { get; }

        bool IsFullScreen { get; }

        void Initialize();
    }
}
