using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class Resolution
    {
        public int Width;

        public int Height;

        public float AspectRatio
        {
            get { return (float)Width / Height; }
            private set { }
        }

        public Resolution()
        {
        }
    }
}
