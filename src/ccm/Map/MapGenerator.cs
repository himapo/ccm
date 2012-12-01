using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccm.DungeonLogic;

namespace ccm
{
    class MapGenerator
    {
        static readonly MapGenerator instance = new MapGenerator();

        HimaLib.Math.SystemRand rand;

        DungeonMap dungeonMap;

        MapGenerator()
        {
            rand = new HimaLib.Math.SystemRand();
            rand.Init(Environment.TickCount);
            dungeonMap = new DungeonMap() { Rand = rand };
        }

        public static MapGenerator GetInstance()
        {
            return instance;
        }

        public DungeonMap Generate()
        {
            dungeonMap.Generate();
            return dungeonMap;
        }
    }
}
