using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class MouseButton : IBooleanDevice
    {
        IMouse mouse;

        MouseButtonLabel button;

        bool prevPressed;

        bool nowPressed;

        public MouseButton(IMouse mouse, MouseButtonLabel button)
        {
            this.mouse = mouse;
            this.button = button;
            prevPressed = false;
            nowPressed = false;
        }

        public void Update()
        {
            prevPressed = nowPressed;

            switch (button)
            {
                case MouseButtonLabel.Left:
                    nowPressed = mouse.IsLeftButtonDown();
                    break;
                case MouseButtonLabel.Right:
                    nowPressed = mouse.IsRightButtonDown();
                    break;
                case MouseButtonLabel.Middle:
                    nowPressed = mouse.IsMiddleButtonDown();
                    break;
            }
        }

        public bool IsPush()
        {
            return nowPressed && !prevPressed;
        }

        public bool IsPress()
        {
            return nowPressed;
        }

        public bool IsRelease()
        {
            return !nowPressed && prevPressed;
        }
    }
}
