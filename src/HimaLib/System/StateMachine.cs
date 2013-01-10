using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class StateMachine : IUpdater, IDrawer
    {
        protected Action UpdateState { get; set; }

        protected Action DrawState { get; set; }

        public StateMachine()
        {
            UpdateState = () => { };
            DrawState = () => { };
        }

        public void Update()
        {
            UpdateState();
        }

        public void Draw()
        {
            DrawState();
        }
    }
}
