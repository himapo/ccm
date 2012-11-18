using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class DefaultGameInitializer : IGameInitializer
    {
        public int ScreenWidth { get; private set; }

        public int ScreenHeight { get; private set; }

        public bool MSAAEnable { get; private set; }

        public int FPS { get; private set; }

        public bool FixedFrameRate { get; private set; }

        public bool VSyncEnable { get; private set; }

        public bool MouseVisible { get; private set; }

        public void Initialize()
        {
            // TODO : デバイスの性能取得

            ScreenWidth = 1280;
            ScreenHeight = 720;
            MSAAEnable = false;
            FPS = 60;
            FixedFrameRate = false;
            VSyncEnable = false;
            MouseVisible = true;
        }
    }
}
