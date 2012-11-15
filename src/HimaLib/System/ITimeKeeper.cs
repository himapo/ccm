using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib
{
    public interface ITimeKeeper
    {
        float FrameRate { get; set; }

        float RealFrameRate { get; }

        float LastFrameSeconds { get; }
        
        float LastTimeScale { get; }

        void Update();
    }
}
