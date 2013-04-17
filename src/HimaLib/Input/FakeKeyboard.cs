using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class FakeKeyboard : IKeyboard
    {
        public bool EnableOnBackGround { get; set; }

        public Dictionary<KeyboardKeyLabel, bool> KeyDownState { get; set; }

        public FakeKeyboard()
        {
            KeyDownState = new Dictionary<KeyboardKeyLabel, bool>();
        }

        public bool IsKeyDown(KeyboardKeyLabel key)
        {
            return KeyDownState[key];
        }
    }
}
