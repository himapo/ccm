using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IKeyboard
    {
        bool EnableOnBackGround { get; set; }

        bool IsKeyDown(KeyboardKeyLabel key);
    }
}
