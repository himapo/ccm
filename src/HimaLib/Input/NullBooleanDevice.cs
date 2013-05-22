using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class NullBooleanDevice : IBooleanDevice
    {
        public void Update() { }

        public bool IsPush() { return false; }

        public bool IsPress() { return false; }

        public bool IsRelease() { return false; }
    }
}
