using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class StateMachine : IUpdater, IDrawer
    {
        Action<ITimeKeeper> UpdateFunc { get; set; }

        Action<ITimeKeeper> DrawFunc { get; set; }

        public StateMachine()
        {
        }

        public void Update(ITimeKeeper timeKeeper)
        {
            UpdateFunc(timeKeeper);
        }

        public void Draw(ITimeKeeper timeKeeper)
        {
            DrawFunc(timeKeeper);
        }

        protected void ChangeUpdateState(Action<ITimeKeeper> updateFunc)
        {
            UpdateFunc = updateFunc;
        }

        protected void ChangeDrawState(Action<ITimeKeeper> drawFunc)
        {
            DrawFunc = drawFunc;
        }
    }
}
