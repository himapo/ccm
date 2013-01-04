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
        Vector3 Eye { get; set; }

        Vector3 At { get; set; }
        
        Vector3 Up { get; set; }

        float FovY { get; set; }
        
        float Aspect { get; set; }
        
        float Near { get; set; }
        
        float Far { get; set; }
    }
}
