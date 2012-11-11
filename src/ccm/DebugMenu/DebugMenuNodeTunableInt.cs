using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class DebugMenuNodeTunableInt : DebugMenuNodeTunable<int>
    {
        public DebugMenuNodeTunableInt(Game game, int initial)
            : base(game, initial)
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
