using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeExecutable : DebugMenuNode
    {
        public Action ExecFunc { get; set; }

        public DebugMenuNodeExecutable()
        {
        }

        public override void OnPushOK()
        {
            ExecFunc();
        }
    }
}
