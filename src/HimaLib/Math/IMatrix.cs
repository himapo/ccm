using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public interface IMatrix
    {
        float M11 { get; set; }
        float M12 { get; set; }
        float M13 { get; set; }
        float M14 { get; set; }
        float M21 { get; set; }
        float M22 { get; set; }
        float M23 { get; set; }
        float M24 { get; set; }
        float M31 { get; set; }
        float M32 { get; set; }
        float M33 { get; set; }
        float M34 { get; set; }
        float M41 { get; set; }
        float M42 { get; set; }
        float M43 { get; set; }
        float M44 { get; set; }
    }
}
