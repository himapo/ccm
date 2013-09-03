using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Debug
{
    public class NullDebugFont : IDebugFont
    {
        public void Initialize(string fontName)
        {
        }

        public void Clear()
        {
        }

        public void Add(string output, float x, float y)
        {
        }

        public void Add(string output, float x, float y, Color fontColor, Color bgColor)
        {
        }

        public void Draw()
        {
        }
    }
}
