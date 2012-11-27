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

        DefaultMouse mouse;

        MainController gameController;

        MainController debugController;

        public RootObject()
        {
            keyboard = new DefaultKeyboard();
            mouse = new DefaultMouse();
            gameController = new MainController(keyboard, mouse);
            debugController = new MainController(keyboard, mouse);

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
            gameController.AddKeyboardKey(DigitalDeviceLabel.Up, KeyboardKeyLabel.W);
            gameController.AddKeyboardKey(DigitalDeviceLabel.Down, KeyboardKeyLabel.S);
            gameController.AddKeyboardKey(DigitalDeviceLabel.Left, KeyboardKeyLabel.A);
            gameController.AddKeyboardKey(DigitalDeviceLabel.Right, KeyboardKeyLabel.D);
            gameController.AddKeyboardKey(DigitalDeviceLabel.OK, KeyboardKeyLabel.Z);
            gameController.AddKeyboardKey(DigitalDeviceLabel.Cancel, KeyboardKeyLabel.X);
            gameController.AddKeyboardKey(DigitalDeviceLabel.Exit, KeyboardKeyLabel.Q);
            gameController.AddKeyboardKey(DigitalDeviceLabel.Camera, KeyboardKeyLabel.LeftAlt);

            gameController.AddMouseButton(DigitalDeviceLabel.MouseMain, MouseButtonLabel.Left);
            gameController.AddMouseButton(DigitalDeviceLabel.MouseSub, MouseButtonLabel.Right);
            gameController.AddMouseButton(DigitalDeviceLabel.MouseMiddle, MouseButtonLabel.Middle);

            gameController.AddMouseAxis(PointingDeviceLabel.Mouse0);

            InputAccessor.AddController(ControllerLabel.Main, gameController, true);

            debugController.AddKeyboardKey(DigitalDeviceLabel.Up, KeyboardKeyLabel.W);
            debugController.AddKeyboardKey(DigitalDeviceLabel.Down, KeyboardKeyLabel.S);
            debugController.AddKeyboardKey(DigitalDeviceLabel.Left, KeyboardKeyLabel.A);
            debugController.AddKeyboardKey(DigitalDeviceLabel.Right, KeyboardKeyLabel.D);
            debugController.AddKeyboardKey(DigitalDeviceLabel.OK, KeyboardKeyLabel.Z);
            debugController.AddKeyboardKey(DigitalDeviceLabel.Cancel, KeyboardKeyLabel.X);
            debugController.AddKeyboardKey(DigitalDeviceLabel.ToggleDebugMenu, KeyboardKeyLabel.F1);

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

            if (CurrentScene.IsChange)
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
