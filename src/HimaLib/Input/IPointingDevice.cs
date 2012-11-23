using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IPointingDevice
    {
        int X { get; }

        int Y { get; }

        int MoveX { get; }

        int MoveY { get; }

        void Update();
    }
}
