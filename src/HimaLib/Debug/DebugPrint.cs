using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public static class DebugPrint
    {
#if DEBUG
        static ConsoleDebugPrint instance = new ConsoleDebugPrint();
#else
        static NullDebugPrint instance = new NullDebugPrint();
#endif

        static IDebugPrint GetInstance()
        {
            return instance;
        }

        public static void PrintLine(string value)
        {
            GetInstance().PrintLine(value);
        }

        public static void PrintLine(string format, params object[] arg)
        {
            GetInstance().PrintLine(format, arg);
        }

        public static void PrintLine(string format, object arg0)
        {
            GetInstance().PrintLine(format, arg0);
        }

        public static void PrintLine(string format, object arg0, object arg1)
        {
            GetInstance().PrintLine(format, arg0, arg1);
        }

        public static void PrintLine(string format, object arg0, object arg1, object arg2)
        {
            GetInstance().PrintLine(format, arg0, arg1, arg2);
        }

        public static void PrintLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
            GetInstance().PrintLine(format, arg0, arg1, arg2, arg3);
        }
    }
}
