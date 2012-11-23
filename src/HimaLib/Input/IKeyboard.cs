using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IKeyboard
    {
        bool IsKeyDown(KeyboardKeyLabel key);
    }
}
