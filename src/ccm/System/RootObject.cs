using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Debug;
using HimaLib.System;

namespace ccm.System
{
    public class RootObject : StateMachine
    {
        public Scene.SceneBase CurrentScene { get; set; }

        public RootObject()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;
        }

        void UpdateStateInit()
        {
            DebugFont.Initialize("SpriteFont/Kootenay");

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Clear();

            CurrentScene.Update();

            if (CurrentScene.ChangeScene)
            {
                CurrentScene = CurrentScene.NextScene;
            }
        }

        void DrawStateMain()
        {
            CurrentScene.Draw();

            DebugFont.Draw();
        }
    }
}
