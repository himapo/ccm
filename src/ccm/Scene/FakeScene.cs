using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace ccm.Scene
{
    public class FakeScene : SceneBase
    {
        public FakeScene()
        {
            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void UpdateStateMain(ITimeKeeper timeKeeper)
        {
        }

        void DrawStateMain(ITimeKeeper timeKeeper)
        {
        }
    }
}
