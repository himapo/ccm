using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.System;

namespace ccm.System
{
    public class RootObject : StateMachine
    {
        public RootObject()
        {
            ChangeUpdateState(UpdateFuncInit);
            ChangeDrawState(DrawFuncInit);
        }

        void UpdateFuncInit(ITimeKeeper timeKeeper)
        {
            ChangeUpdateState(UpdateFuncMain);
        }

        void DrawFuncInit(ITimeKeeper timeKeeper)
        {
            ChangeDrawState(DrawFuncMain);
        }

        void UpdateFuncMain(ITimeKeeper timeKeeper)
        {
        }

        void DrawFuncMain(ITimeKeeper timeKeeper)
        {
        }
    }
}
