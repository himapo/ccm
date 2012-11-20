using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public interface IColor
    {
        byte R { get; set; }

        byte G { get; set; }

        byte B { get; set; }

        byte A { get; set; }
    }
}
