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

        void UpdateStateMain()
        {
            CurrentScene.Update();

            if (CurrentScene.ChangeScene)
            {
                CurrentScene = CurrentScene.NextScene;
            }
        }

        void DrawStateMain()
        {
            CurrentScene.Draw();
        }
    }
}
