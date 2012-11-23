using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Debug;
using HimaLib.System;
using HimaLib.Input;

namespace ccm.System
{
    public class RootObject : StateMachine
    {
        public Scene.SceneBase CurrentScene { get; set; }

        DefaultKeyboard keyboard;

        Input.MainController gameController;

        Input.MainController debugController;

        public RootObject()
        {
            keyboard = new DefaultKeyboard();
            gameController = new Input.MainController(keyboard);
            debugController = new Input.MainController(keyboard);

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
            gameController.AddKeyboardKey(Input.VirtualKeyLabel.Up, KeyboardKeyLabel.W);
            gameController.AddKeyboardKey(Input.VirtualKeyLabel.Down, KeyboardKeyLabel.S);
            gameController.AddKeyboardKey(Input.VirtualKeyLabel.Left, KeyboardKeyLabel.A);
            gameController.AddKeyboardKey(Input.VirtualKeyLabel.Right, KeyboardKeyLabel.D);
            gameController.AddKeyboardKey(Input.VirtualKeyLabel.OK, KeyboardKeyLabel.Z);
            gameController.AddKeyboardKey(Input.VirtualKeyLabel.Cancel, KeyboardKeyLabel.X);

            HimaLib.Input.Input.AddController((int)Input.ControllerLabel.Main, gameController, true);

            debugController.AddKeyboardKey(Input.VirtualKeyLabel.Up, KeyboardKeyLabel.W);
            debugController.AddKeyboardKey(Input.VirtualKeyLabel.Down, KeyboardKeyLabel.S);
            debugController.AddKeyboardKey(Input.VirtualKeyLabel.Left, KeyboardKeyLabel.A);
            debugController.AddKeyboardKey(Input.VirtualKeyLabel.Right, KeyboardKeyLabel.D);
            debugController.AddKeyboardKey(Input.VirtualKeyLabel.OK, KeyboardKeyLabel.Z);
            debugController.AddKeyboardKey(Input.VirtualKeyLabel.Cancel, KeyboardKeyLabel.X);

            HimaLib.Input.Input.AddController((int)Input.ControllerLabel.Debug, debugController, false);
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Clear();

            HimaLib.Input.Input.Update();

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
