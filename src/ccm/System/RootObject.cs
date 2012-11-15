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
        public Scene.SceneBase CurrentScene { get; set; }

        public RootObject()
        {
            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void UpdateStateMain(ITimeKeeper timeKeeper)
        {
            CurrentScene.Update(timeKeeper);

            if (CurrentScene.ChangeScene)
            {
                CurrentScene = CurrentScene.NextScene;
            }
        }

        void DrawStateMain(ITimeKeeper timeKeeper)
        {
            CurrentScene.Draw(timeKeeper);
        }
    }
}
