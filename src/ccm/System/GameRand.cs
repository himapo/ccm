using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.System
{
    public class GameRand : HimaLib.Math.SystemRand
    {
        static readonly GameRand instance = new GameRand();

        public static GameRand Instance { get { return instance; } private set { } }

        GameRand()
        {
        }
    }
}
