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
        /// <summary>
        /// 攻撃１発のよろけ値の基本値
        /// </summary>
        static readonly float ShockBase = 100;

        /// <summary>
        /// ガードしたときのよろけ値倍率
        /// </summary>
        static readonly float GuardFactor = 0.5f;

        /// <summary>
        /// ふらふらになるよろけ値のしきい値
        /// </summary>
        static readonly float ShockLimit = 120;

        /// <summary>
        /// ふらふら状態になったときの開始値
        /// </summary>
        static readonly float ShockStart = 200;

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

        /// <summary>
        /// よろけ値の現在値
        /// </summary>
        float Shock;

        public ComboCounter()
        {
            Reset();
        }

        public void Reset()
        {
            Shock = 0.0f;
            Count = 1;
        }

        public void Update(float elapsedFrame)
        {
            if (Shocked)
            {
                Shock -= elapsedFrame;
                if (!Shocked)
                {
                    Reset();
                }
            }
            else if (Shock > 0.0f)
            {
                Shock -= elapsedFrame;
            }
        }

        public void Damage(float shock)
        {
            if (Shocked)
            {
                Count++;
                Shock += shock * ShockBase;
            }
            else
            {
                Shock += shock * ShockBase;
                if (Shocked)
                {
                    Shock = ShockStart;
                }
            }
            DebugPrint.PrintLine("{0}Hits. (Shock {1})", Count, Shock);
        }

        public void Guard(float shock)
        {
            Shock += shock * ShockBase * GuardFactor;
            if (Shocked)
            {
                Shock = ShockStart;
                DebugPrint.PrintLine("Guard crash. (Shock {0})", Shock);
            }
        }
    }
}
