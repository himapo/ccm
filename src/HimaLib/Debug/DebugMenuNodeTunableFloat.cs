using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeTunableFloat : DebugMenuNodeTunable<float>
    {
        public float Interval { get; set; }

        public DebugMenuNodeTunableFloat()
        {
        }

        public override void OnPushLeft()
        {
            Val -= Interval;
        }

        public override void OnPushRight()
        {
            Val += Interval;
        }
    }
}
