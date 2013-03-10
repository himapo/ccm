using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccm.DungeonLogic;

namespace ccm
{
    public class MapGenerator
    {
        static readonly MapGenerator instance = new MapGenerator();

        public static MapGenerator Instance { get { return instance; } private set { } }

        HimaLib.Math.SystemRand rand;

        DungeonMap dungeonMap;

        MapGenerator()
        {
            rand = new HimaLib.Math.SystemRand();
            rand.Init(Environment.TickCount);
            dungeonMap = new DungeonMap() { Rand = rand };
        }

        public DungeonMap Generate()
        {
            dungeonMap.Generate();
            return dungeonMap;
        }
    }
}
