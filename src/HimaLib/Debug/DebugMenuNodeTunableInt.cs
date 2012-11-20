using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeTunableInt : DebugMenuNodeTunable<int>
    {
        public DebugMenuNodeTunableInt(int initial)
            : base(initial)
        {
        }

        public override void OnPushLeft()
        {
            --val;
        }

        public override void OnPushRight()
        {
            ++val;
        }
    }
}
