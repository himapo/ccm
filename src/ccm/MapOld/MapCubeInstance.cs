using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class MapCubeInstance : MyGameComponent
    {
        public Matrix Transform { get; set; }

        public MapCubeInstance(Game game)
            : base(game)
        {
        }
    }
}
