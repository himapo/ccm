using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.System
{
    public class DrawRand : HimaLib.Math.SystemRand
    {
        static readonly DrawRand instance = new DrawRand();

        public static DrawRand Instance { get { return instance; } private set { } }

        DrawRand()
        {
        }
    }
}
