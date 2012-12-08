using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public static class LoadProfiler
    {
        static DebugSampleLoadProfiler instance = new DebugSampleLoadProfiler();

        public static ILoadProfiler GetInstance()
        {
            return instance;
        }

        public static void StartFrame()
        {
            GetInstance().StartFrame();
        }

        public static void BeginMark(string markerName)
        {
            GetInstance().BeginMark(markerName);
        }

        public static void EndMark()
        {
            GetInstance().EndMark();
        }
    }
}
