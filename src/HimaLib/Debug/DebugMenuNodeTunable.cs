using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeTunable<T> : DebugMenuNode
    {
        protected T val;

        string label;

        public override string Label
        {
            get { return label + " : " + GetValString(); }
            set { label = value; }
        }

        public DebugMenuNodeTunable(T initial)
        {
            val = initial;
        }

        protected virtual string GetValString()
        {
            return val.ToString();
        }
    }
}
