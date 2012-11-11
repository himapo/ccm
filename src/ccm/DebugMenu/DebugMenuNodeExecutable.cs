using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// 任意のメソッドを実行できる葉ノード
    /// </summary>
    class DebugMenuNodeExecutable : DebugMenuNode
    {
        public Action ExecFunc { get; set; }

        public DebugMenuNodeExecutable(Game game)
            : base(game)
        {
        }

        public override void OnPushOK()
        {
            ExecFunc();
        }
    }
}
