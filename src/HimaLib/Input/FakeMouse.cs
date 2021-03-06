﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class FakeMouse : IMouse
    {
        public bool EnableOnBackGround { get; set; }

        public bool FixedAtCenter { get; set; }

        public bool Visible { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Wheel { get; set; }

        public bool LeftDown { get; set; }

        public bool RightDown { get; set; }

        public bool MiddleDown { get; set; }

        public bool IsLeftButtonDown()
        {
            return LeftDown;
        }

        public bool IsRightButtonDown()
        {
            return RightDown;
        }

        public bool IsMiddleButtonDown()
        {
            return MiddleDown;
        }
    }
}
