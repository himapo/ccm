using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class NullDigitalDevice : IDigitalDevice
    {
        public int Value { get { return 0; } }

        public int Delta { get { return 0; } }

        public void Update() { }
    }
}
