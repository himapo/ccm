﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeTunableInt : DebugMenuNodeTunable<int>
    {
        public DebugMenuNodeTunableInt()
        {
        }

        public override void OnPushLeft()
        {
            --Val;
        }

        public override void OnPushRight()
        {
            ++Val;
        }
    }
}
