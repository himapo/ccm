using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    class MapGenerator
    {
        static readonly MapGenerator instance = new MapGenerator();

        MapGenerator()
        {
        }

        public static MapGenerator GetInstance()
        {
            return instance;
        }

        public DungeonMap Generate()
        {
            return new DungeonMap();
        }
    }
}
