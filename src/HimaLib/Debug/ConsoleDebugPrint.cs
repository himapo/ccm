using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class ConsoleDebugPrint : IDebugPrint
    {
        public void PrintLine(string value)
        {
            Console.WriteLine(value);
        }

        public void PrintLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        public void PrintLine(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }

        public void PrintLine(string format, object arg0, object arg1)
        {
            Console.WriteLine(format, arg0, arg1);
        }

        public void PrintLine(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(format, arg0, arg1, arg2);
        }

        public void PrintLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Console.WriteLine(format, arg0, arg1, arg2, arg3);
        }
    }
}
