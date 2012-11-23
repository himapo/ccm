using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Debug;
using HimaLib.System;
using HimaLib.Input;
using ccm.Input;

namespace ccm.System
{
    public class RootObject : StateMachine
    {
        public Scene.SceneBase CurrentScene { get; set; }

        DefaultKeyboard keyboard;

        MainController gameController;

        MainController debugController;

        public RootObject()
        {
            keyboard = new DefaultKeyboard();
            gameController = new MainController(keyboard);
            debugController = new MainController(keyboard);

            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;
        }

        void UpdateStateInit()
        {
            DebugFont.Initialize("SpriteFont/Kootenay");

            InitController();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitController()
        {
            // TODO : コンフィグファイルから設定
            gameController.AddKeyboardKey(VirtualKeyLabel.Up, KeyboardKeyLabel.W);
            gameController.AddKeyboardKey(VirtualKeyLabel.Down, KeyboardKeyLabel.S);
            gameController.AddKeyboardKey(VirtualKeyLabel.Left, KeyboardKeyLabel.A);
            gameController.AddKeyboardKey(VirtualKeyLabel.Right, KeyboardKeyLabel.D);
            gameController.AddKeyboardKey(VirtualKeyLabel.OK, KeyboardKeyLabel.Z);
            gameController.AddKeyboardKey(VirtualKeyLabel.Cancel, KeyboardKeyLabel.X);
            
            InputAccessor.AddController(ControllerLabel.Main, gameController, false);

            debugController.AddKeyboardKey(VirtualKeyLabel.Up, KeyboardKeyLabel.W);
            debugController.AddKeyboardKey(VirtualKeyLabel.Down, KeyboardKeyLabel.S);
            debugController.AddKeyboardKey(VirtualKeyLabel.Left, KeyboardKeyLabel.A);
            debugController.AddKeyboardKey(VirtualKeyLabel.Right, KeyboardKeyLabel.D);
            debugController.AddKeyboardKey(VirtualKeyLabel.OK, KeyboardKeyLabel.Z);
            debugController.AddKeyboardKey(VirtualKeyLabel.Cancel, KeyboardKeyLabel.X);
            debugController.AddKeyboardKey(VirtualKeyLabel.ToggleDebugMenu, KeyboardKeyLabel.F1);

            InputAccessor.AddController(ControllerLabel.Debug, debugController, true);
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Clear();

            InputAccessor.Update();

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
