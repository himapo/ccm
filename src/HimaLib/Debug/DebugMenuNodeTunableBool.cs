using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeTunableBool : DebugMenuNodeTunable<bool>
    {
        public DebugMenuNodeTunableBool()
        {
        }

        public override void OnPushLeft()
        {
            Val = false;
        }

        public override void OnPushRight()
        {
            Val = true;
        }
    }
}
