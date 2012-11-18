using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class NullDebugPrint : IDebugPrint
    {
        public void PrintLine(string value)
        {
        }

        public void PrintLine(string format, params object[] arg)
        {
        }

        public void PrintLine(string format, object arg0)
        {
        }

        public void PrintLine(string format, object arg0, object arg1)
        {
        }

        public void PrintLine(string format, object arg0, object arg1, object arg2)
        {
        }

        public void PrintLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
        }
    }
}
