using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;

namespace ccm.Game
{
    public class Reticule
    {
        public void Draw()
        {
            DebugFont.Add("+",
                GameProperty.resolutionWidth * 0.5f - 7.0f,
                GameProperty.resolutionHeight * 0.4f - 12.0f
                );
        }
    }
}
