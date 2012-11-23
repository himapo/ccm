using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IMouse
    {
        int X { get; set; }

        int Y { get; set; }

        bool IsLeftButtonDown();

        bool IsRightButtonDown();

        bool IsMiddleButtonDown();
    }
}
