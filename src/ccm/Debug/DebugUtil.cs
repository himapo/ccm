using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ccm
{
    static class DebugUtil
    {
        [Conditional("DEBUG")]
        public static void PrintLine(string value)
        {
            Console.WriteLine(value);
        }

        [Conditional("DEBUG")]
        public static void PrintLine(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }

        [Conditional("DEBUG")]
        public static void PrintLine(string format, object arg0, object arg1)
        {
            Console.WriteLine(format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public static void PrintLine(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(format, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")]
        public static void PrintLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Console.WriteLine(format, arg0, arg1, arg2, arg3);
        }
    }
}
