using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using ccm.Input;

namespace ccm.Scene
{
    public class GameScene : SceneBase
    {
        public GameScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "GameScene";
        }

        void UpdateStateInit()
        {
            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Add(Name, 50.0f, 60.0f);

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                ChangeScene(new BootScene());
                return;
            }
        }

        void DrawStateMain()
        {
        }
    }
}
