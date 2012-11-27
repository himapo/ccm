using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class KeyboardKey : IBooleanDevice
    {
        IKeyboard keyboard;

        KeyboardKeyLabel key;

        bool prevPressed;

        bool nowPressed;

        public KeyboardKey(IKeyboard keyboard, KeyboardKeyLabel key)
        {
            this.keyboard = keyboard;
            this.key = key;
            prevPressed = false;
            nowPressed = false;
        }

        public void Update()
        {
            prevPressed = nowPressed;

            nowPressed = keyboard.IsKeyDown(key);
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
