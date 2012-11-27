using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public interface IVector4
    {
        float W { get; set; }

        float X { get; set; }

        float Y { get; set; }

        float Z { get; set; }

        float Length();

        float LengthSquared();

        void Normalize();
    }
}
