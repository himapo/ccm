using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib
{
    public class DefaultGameInitializer : IGameInitializer
    {
        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        public bool MSAAEnable { get; set; }

        public int FPS { get; set; }

        public bool FixedFrameRate { get; set; }

        public bool MouseVisible { get; set; }

        public void Initialize()
        {
            ScreenWidth = 1280;
            ScreenHeight = 720;
            MSAAEnable = false;
            FPS = 60;
            FixedFrameRate = false;
            MouseVisible = true;
        }
    }
}
