using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class StateMachine : IUpdater, IDrawer
    {
        protected Action<ITimeKeeper> UpdateState { get; set; }

        protected Action<ITimeKeeper> DrawState { get; set; }

        public StateMachine()
        {
        }

        public void Update(ITimeKeeper arg)
        {
            UpdateState(arg);
        }

        public void Draw(ITimeKeeper arg)
        {
            DrawState(arg);
        }
    }
}
