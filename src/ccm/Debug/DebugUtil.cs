using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    static class DebugUtil
    {
        public static void PrintLine(string value)
        {
#if DEBUG
            Console.WriteLine(value);
#endif
        }

        public static void PrintLine(string format, object arg0)
        {
#if DEBUG
            Console.WriteLine(format, arg0);
#endif
        }

        public static void PrintLine(string format, object arg0, object arg1)
        {
#if DEBUG
            Console.WriteLine(format, arg0, arg1);
#endif
        }

        public static void PrintLine(string format, object arg0, object arg1, object arg2)
        {
#if DEBUG
            Console.WriteLine(format, arg0, arg1, arg2);
#endif
        }

        public static void PrintLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
#if DEBUG
            Console.WriteLine(format, arg0, arg1, arg2, arg3);
#endif
        }
    }
}
