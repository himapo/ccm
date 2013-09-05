using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib;
using HimaLib.Debug;
using HimaLib.System;
using HimaLib.Input;
using HimaLib.Updater;
using HimaLib.Render;
using ccm.Render;
using ccm.Input;
using ccm.Sound;

namespace ccm.System
{
    public class RootObject : StateMachine
    {
        public HimaLib.System.Game Game { get; set; }

        public Scene.SceneBase CurrentScene { get; set; }

        DefaultKeyboard keyboard = new DefaultKeyboard();

        DefaultMouse mouse = new DefaultMouse();

        MainController gameController;

        MainController debugController;

        SoundManager SoundManager { get { return SoundManager.Instance; } }

        public RootObject()
        {
            gameController = new MainController(keyboard, mouse);
            debugController = new MainController(keyboard, mouse);

            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;
        }

        void UpdateStateInit()
        {
            DebugFont.Initialize("SpriteFont/Debug");

            GameRand.Instance.Init(Environment.TickCount);
            DrawRand.Instance.Init(Environment.TickCount);

            keyboard.Game = Game;
            mouse.Game = Game;

            InitController();

            SoundManager.Initialize();

            InitRender();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitController()
        {
            // TODO : コンフィグファイルから設定
            gameController.AddKeyboardKey(BooleanDeviceLabel.Up, KeyboardKeyLabel.W);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Down, KeyboardKeyLabel.S);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Left, KeyboardKeyLabel.A);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Right, KeyboardKeyLabel.D);
            gameController.AddKeyboardKey(BooleanDeviceLabel.OK, KeyboardKeyLabel.Z);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Cancel, KeyboardKeyLabel.X);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Exit, KeyboardKeyLabel.Q);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Camera, KeyboardKeyLabel.LeftAlt);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Jump, KeyboardKeyLabel.Space);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Crouch, KeyboardKeyLabel.LeftControl);
            gameController.AddKeyboardKey(BooleanDeviceLabel.Step, KeyboardKeyLabel.LeftShift);
            gameController.AddKeyboardKey(BooleanDeviceLabel.ItemWindow, KeyboardKeyLabel.Tab);

            gameController.AddMouseButton(BooleanDeviceLabel.MouseMain, MouseButtonLabel.Left);
            gameController.AddMouseButton(BooleanDeviceLabel.MouseSub, MouseButtonLabel.Right);
            gameController.AddMouseButton(BooleanDeviceLabel.MouseMiddle, MouseButtonLabel.Middle);

            gameController.AddMouseAxis();
            gameController.AddMouseWheel();            

            InputAccessor.AddController(ControllerLabel.Main, gameController, true);

            debugController.AddKeyboardKey(BooleanDeviceLabel.Up, KeyboardKeyLabel.W);
            debugController.AddKeyboardKey(BooleanDeviceLabel.Down, KeyboardKeyLabel.S);
            debugController.AddKeyboardKey(BooleanDeviceLabel.Left, KeyboardKeyLabel.A);
            debugController.AddKeyboardKey(BooleanDeviceLabel.Right, KeyboardKeyLabel.D);
            debugController.AddKeyboardKey(BooleanDeviceLabel.OK, KeyboardKeyLabel.Z);
            debugController.AddKeyboardKey(BooleanDeviceLabel.Cancel, KeyboardKeyLabel.X);
            debugController.AddKeyboardKey(BooleanDeviceLabel.ToggleDebugMenu, KeyboardKeyLabel.F1);

            InputAccessor.AddController(ControllerLabel.Debug, debugController, true);
        }

        void InitRender()
        {
            RenderSceneManager.Instance.AddPath(
                RenderPathType.OPAQUE,
                RenderPathFactory.Instance.CreatePath("Opaque"));

            //RenderSceneManager.Instance.AddPath(
            //    RenderPathType.TRANSLUCENT,
            //    RenderPathFactory.Instance.CreatePath("Translucent",
            //        true, true, false, false));

            //RenderSceneManager.Instance.AddPath(
            //    RenderPathType.HUD,
            //    RenderPathFactory.Instance.CreatePath("HUD",
            //        false, true, true, true));
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            LoadProfiler.StartFrame();

            LoadProfiler.BeginMark("Update");

            TimeKeeper.Instance.Update();

            DebugFont.Clear();

            InputAccessor.Update();

            SoundManager.Update();

            HimaLib.Updater.UpdaterManager.Instance.Update(TimeKeeper.Instance.LastFrameMilliSeconds);

            CurrentScene.Update();

            if (CurrentScene.IsChange)
            {
                CurrentScene = CurrentScene.NextScene;
            }

            LoadProfiler.EndMark();
        }

        void DrawStateMain()
        {
            LoadProfiler.BeginMark("Draw");

            CurrentScene.Draw();

            DrawDebugFPS();

            RenderSceneManager.Instance.Render();

            DebugFont.Draw();

            LoadProfiler.EndMark();
        }

        void DrawDebugFPS()
        {
            DebugFont.Add(string.Format("{0:f2}FPS", TimeKeeper.Instance.AverageFrameRate), 0.0f, 0.0f);
        }
    }
}
