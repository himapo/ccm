﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using HimaLib.System;

namespace HimaLib.Input
{
    public class DefaultMouse : IMouse
    {
        public bool EnableOnBackGround { get; set; }

        public Game Game { get; set; }

        public int X
        {
            get
            {
                return CheckActive(Mouse.GetState().X - SystemProperty.ScreenWidth / 2);
            }
            set
            {
            }
        }

        public int Y
        {
            get
            {
                return CheckActive(-Mouse.GetState().Y + SystemProperty.ScreenHeight / 2);
            }
            set
            {
            }
        }

        public int Wheel
        {
            get
            {
                return CheckActive(Mouse.GetState().ScrollWheelValue);
            }
            set
            {
            }
        }

        public DefaultMouse()
        {
            EnableOnBackGround = false;
        }

        public bool IsLeftButtonDown()
        {
            return CheckActive(Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        public bool IsRightButtonDown()
        {
            return CheckActive(Mouse.GetState().RightButton == ButtonState.Pressed);
        }

        public bool IsMiddleButtonDown()
        {
            return CheckActive(Mouse.GetState().MiddleButton == ButtonState.Pressed);
        }

        bool CheckActive(bool b)
        {
            return (EnableOnBackGround || Game.IsActive) && b;
        }

        int CheckActive(int n)
        {
            return (EnableOnBackGround || Game.IsActive) ? n : 0;
        }
    }
}
