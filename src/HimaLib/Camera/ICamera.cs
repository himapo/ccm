using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Math;

namespace HimaLib.Camera
{
    public interface ICamera
    {
        IVector3 Eye { get; set; }

        IVector3 At { get; set; }
        
        IVector3 Up { get; set; }

        float FovY { get; set; }
        
        float Aspect { get; set; }
        
        float Near { get; set; }
        
        float Far { get; set; }
    }
}
