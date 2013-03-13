using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public class DebugMenuNodeTunable<T> : DebugMenuNode
    {
        public Func<T> Getter { get; set; }

        public Action<T> Setter { get; set; }

        T val;
        protected T Val
        {
            get { return Getter(); }
            set { Setter(value); }
        }

        string label;

        public override string Label
        {
            get { return label + " : " + GetValString(); }
            set { label = value; }
        }

        public DebugMenuNodeTunable()
        {
            Getter = () => { return val; };
            Setter = (v) => { val = v; };
        }

        protected virtual string GetValString()
        {
            return Val.ToString();
        }
    }
}
