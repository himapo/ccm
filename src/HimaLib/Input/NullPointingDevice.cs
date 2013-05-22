using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class NullPointingDevice : IPointingDevice
    {
        public int X { get { return 0; } }

        public int Y { get { return 0; } }

        public int MoveX { get { return 0; } }

        public int MoveY { get { return 0; } }

        public void Update() { }
    }
}
