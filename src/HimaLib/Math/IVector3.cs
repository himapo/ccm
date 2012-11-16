using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public interface IVector3
    {
        float X { get; set; }
        
        float Y { get; set; }

        float Z { get; set; }

        float Length();

        float LengthSquared();

        void Normalize();
    }
}
