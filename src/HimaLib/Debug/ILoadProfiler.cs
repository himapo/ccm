using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public interface ILoadProfiler
    {
        void StartFrame();

        void BeginMark(string markerName);

        void EndMark();
    }
}
