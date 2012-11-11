using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// 左右キーで値の調節ができる葉ノード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DebugMenuNodeTunable<T> : DebugMenuNode
    {
        protected T val;

        string label;

        public override string Label
        {
            get { return label + " : " + GetValString(); }
            set { label = value; }
        }

        public DebugMenuNodeTunable(Game game, T initial)
            : base(game)
        {
            val = initial;
        }

        protected virtual string GetValString()
        {
            return val.ToString();
        }
    }
}
