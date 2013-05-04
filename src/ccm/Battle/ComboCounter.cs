using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;

namespace ccm.Battle
{
    /// <summary>
    /// ふらふら状態とコンボの管理
    /// キャラごとに一つインスタンスを持たせる
    /// </summary>
    public class ComboCounter
    {
        static readonly int ShockLimit = 1000;

        /// <summary>
        /// ふらふら中（コンボ継続状態）か
        /// </summary>
        public bool Shocked
        {
            get
            {
                return Shock > ShockLimit;
            }
        }

        /// <summary>
        /// ヒット数
        /// </summary>
        public int Count { get; private set; }

        float Shock;

        public ComboCounter()
        {
        }

        public void Update(float elapsedFrame)
        {
            if (Shock > 0.0f)
            {
                Shock -= elapsedFrame;
            }
        }

        public void Damage(int shock)
        {
            if (Shocked)
            {
                Count++;
            }
            Shock += shock;
            DebugPrint.PrintLine("{0}Hits. (Shock {1})", Count + 1, Shock);
        }

        public void Guard(int shock)
        {
            Shock += shock;
        }
    }
}
