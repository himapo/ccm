using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Debug;
using ccm.Debug;
using ccm.Input;

namespace ccm.Scene
{
    public class HomeScene : SceneBase
    {
        // デバッグメニュー
        DebugMenu debugMenu = new DebugMenu("Home Debug Menu");

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer = new DefaultDebugMenuDrawer();

        public HomeScene()
        {
            Name = "HomeScene";

            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
        }

        void UpdateStateInit()
        {
            InitDebugMenu();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "Go to Dungeon",
                Selectable = true,
                ExecFunc = () =>
                {
                    InputAccessor.SwitchController(ControllerLabel.Main, true);
                    ChangeScene(new DungeonScene());
                },
            });

            debugMenu.Open();
        }

        void UpdateStateMain()
        {
            debugMenuUpdater.Update();

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                UpdateState = UpdateStateTerm;
                DrawState = DrawStateTerm;
                return;
            }
        }

        void UpdateStateTerm()
        {
            ChangeScene(new BootScene());
        }

        void DrawStateInit()
        {
        }

        void DrawStateMain()
        {
            debugMenu.Draw(debugMenuDrawer);
        }

        void DrawStateTerm()
        {
        }
    }
}
