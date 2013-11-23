using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public static class LoadProfiler
    {
        static DebugSampleLoadProfiler instance = new DebugSampleLoadProfiler();

        public static ILoadProfiler Instance
        {
            get { return instance; }
        }

        public static void StartFrame()
        {
            Instance.StartFrame();
        }

        public static void BeginMark(string markerName)
        {
            Instance.BeginMark(markerName);
        }

        public static void EndMark()
        {
            Instance.EndMark();
        }
    }
}
