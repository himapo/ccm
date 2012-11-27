using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using HimaLib.System;

namespace HimaLib.Input
{
    public class DefaultMouse : IMouse
    {
        public int X
        {
            get
            {
                return Mouse.GetState().X - SystemProperty.ScreenWidth / 2;
            }
            set
            {
            }
        }

        public int Y
        {
            get
            {
                return -Mouse.GetState().Y + SystemProperty.ScreenHeight / 2;
            }
            set
            {
            }
        }

        public int Wheel
        {
            get
            {
                return Mouse.GetState().ScrollWheelValue;
            }
            set
            {
            }
        }

        public bool IsLeftButtonDown()
        {
            return (Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        public bool IsRightButtonDown()
        {
            return (Mouse.GetState().RightButton == ButtonState.Pressed);
        }

        public bool IsMiddleButtonDown()
        {
            return (Mouse.GetState().MiddleButton == ButtonState.Pressed);
        }
    }
}
