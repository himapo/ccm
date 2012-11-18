using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public interface IDebugPrint
    {
        void PrintLine(string value);

        void PrintLine(string format, params object[] arg);

        void PrintLine(string format, object arg0);

        void PrintLine(string format, object arg0, object arg1);

        void PrintLine(string format, object arg0, object arg1, object arg2);

        void PrintLine(string format, object arg0, object arg1, object arg2, object arg3);
    }
}
