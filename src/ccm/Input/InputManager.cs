using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class InputManager : MyGameComponent
    {
        static InputManager instance;

        enum MouseButtons
        {
            Left,
            Right,
            Middle,
            X1,
            X2,
        }

        KeyboardState oldKeyState;
        MouseState oldMouseState;
        Dictionary<InputLabel, List<InputMode>> modeMap;
        Dictionary<InputLabel, Keys> virtualKeyMap;
        Dictionary<InputLabel, MouseButtons> virtualMouseMap;
        Dictionary<InputLabel, bool> pressMap;
        Dictionary<InputLabel, bool> pushMap;
        Dictionary<InputLabel, bool> releaseMap;

        public InputMode Mode { get; set; }

        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public int MouseMoveX { get; set; }
        public int MouseMoveY { get; set; }
        public int MouseMoveWheel { get; set; }

        public static void CreateInstance(Game game)
        {
            instance = new InputManager(game);
        }

        public static InputManager GetInstance()
        {
            return instance;
        }

        InputManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.INPUT;

            modeMap = new Dictionary<InputLabel, List<InputMode>>();
            virtualKeyMap = new Dictionary<InputLabel, Keys>();
            virtualMouseMap = new Dictionary<InputLabel, MouseButtons>();
            pressMap = new Dictionary<InputLabel, bool>();
            pushMap = new Dictionary<InputLabel, bool>();
            releaseMap = new Dictionary<InputLabel, bool>();

            Mode = InputMode.Game;

            MouseMoveX = 0;
            MouseMoveY = 0;
            MouseMoveWheel = 0;
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。

            RegisterKey(InputLabel.Up, Keys.W);
            RegisterMode(InputLabel.Up, InputMode.Game);
            RegisterKey(InputLabel.Down, Keys.S);
            RegisterMode(InputLabel.Down, InputMode.Game);
            RegisterKey(InputLabel.Left, Keys.A);
            RegisterMode(InputLabel.Left, InputMode.Game);
            RegisterKey(InputLabel.Right, Keys.D);
            RegisterMode(InputLabel.Right, InputMode.Game);

            RegisterMouse(InputLabel.MouseMain, MouseButtons.Left);
            RegisterMode(InputLabel.MouseMain, InputMode.Game);
            RegisterMouse(InputLabel.MouseSub, MouseButtons.Right);
            RegisterMode(InputLabel.MouseSub, InputMode.Game);
            RegisterMouse(InputLabel.MouseMiddle, MouseButtons.Middle);
            RegisterMode(InputLabel.MouseMiddle, InputMode.Game);

            RegisterKey(InputLabel.Walk, Keys.LeftShift);
            RegisterMode(InputLabel.Walk, InputMode.Game);
            RegisterKey(InputLabel.Crouch, Keys.LeftControl);
            RegisterMode(InputLabel.Crouch, InputMode.Game);
            RegisterKey(InputLabel.Jump, Keys.Space);
            RegisterMode(InputLabel.Jump, InputMode.Game);
            RegisterKey(InputLabel.ItemWindow, Keys.Tab);
            RegisterMode(InputLabel.ItemWindow, InputMode.Game);
            RegisterKey(InputLabel.Camera, Keys.LeftAlt);
            RegisterMode(InputLabel.Camera, InputMode.Game);
            RegisterKey(InputLabel.Exit, Keys.Q);
            RegisterMode(InputLabel.Exit, InputMode.Game);

            RegisterKey(InputLabel.OK, Keys.Z);
            RegisterMode(InputLabel.OK, InputMode.Game);
            RegisterKey(InputLabel.Cancel, Keys.X);
            RegisterMode(InputLabel.Cancel, InputMode.Game);

            RegisterKey(InputLabel.OpenDebugMenu, Keys.F1);
            RegisterMode(InputLabel.OpenDebugMenu, InputMode.Game);
            RegisterKey(InputLabel.CloseDebugMenu, Keys.F1);
            RegisterMode(InputLabel.CloseDebugMenu, InputMode.DebugMenu);
            RegisterKey(InputLabel.DebugMenuOK, Keys.Z);
            RegisterMode(InputLabel.DebugMenuOK, InputMode.DebugMenu);
            RegisterKey(InputLabel.DebugMenuCancel, Keys.X);
            RegisterMode(InputLabel.DebugMenuCancel, InputMode.DebugMenu);
            RegisterKey(InputLabel.DebugMenuUp, Keys.W);
            RegisterMode(InputLabel.DebugMenuUp, InputMode.DebugMenu);
            RegisterKey(InputLabel.DebugMenuDown, Keys.S);
            RegisterMode(InputLabel.DebugMenuDown, InputMode.DebugMenu);
            RegisterKey(InputLabel.DebugMenuLeft, Keys.A);
            RegisterMode(InputLabel.DebugMenuLeft, InputMode.DebugMenu);
            RegisterKey(InputLabel.DebugMenuRight, Keys.D);
            RegisterMode(InputLabel.DebugMenuRight, InputMode.DebugMenu);

            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();

            MouseMoveX = 0;
            MouseMoveY = 0;

            base.Initialize();
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。
            var newKeyState = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            // キーボードの状態記録
            foreach (KeyValuePair<InputLabel, Keys> pair in virtualKeyMap)
            {
                if (newKeyState.IsKeyDown(pair.Value))
                {
                    pressMap[pair.Key] = true;
                    pushMap[pair.Key] = oldKeyState.IsKeyUp(pair.Value);
                    releaseMap[pair.Key] = false;
                }
                else
                {
                    pressMap[pair.Key] = false;
                    pushMap[pair.Key] = false;
                    releaseMap[pair.Key] = oldKeyState.IsKeyDown(pair.Value);
                }
            }

            // マウス(中心を原点にしてY軸反転)
            MouseX = newMouseState.X - (int)GameProperty.resolutionWidth / 2;
            MouseY = -newMouseState.Y + (int)GameProperty.resolutionHeight / 2;
            MouseMoveX = newMouseState.X - oldMouseState.X;
            MouseMoveY = -newMouseState.Y + oldMouseState.Y;
            MouseMoveWheel = newMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue;
            foreach (KeyValuePair<InputLabel, MouseButtons> pair in virtualMouseMap)
            {
                ButtonState oldButtonState;
                ButtonState newButtonState;
                switch (pair.Value)
                {
                    case MouseButtons.Left:
                        oldButtonState = oldMouseState.LeftButton;
                        newButtonState = newMouseState.LeftButton;
                        break;
                    case MouseButtons.Right:
                        oldButtonState = oldMouseState.RightButton;
                        newButtonState = newMouseState.RightButton;
                        break;
                    case MouseButtons.Middle:
                        oldButtonState = oldMouseState.MiddleButton;
                        newButtonState = newMouseState.MiddleButton;
                        break;
                    case MouseButtons.X1:
                        oldButtonState = oldMouseState.XButton1;
                        newButtonState = newMouseState.XButton1;
                        break;
                    case MouseButtons.X2:
                        oldButtonState = oldMouseState.XButton2;
                        newButtonState = newMouseState.XButton2;
                        break;
                    default:
                        oldButtonState = oldMouseState.LeftButton;
                        newButtonState = newMouseState.LeftButton;
                        break;
                }

                // スクリーン外に出ていたら押していないことにする
                var newPress = 
                    (newButtonState == ButtonState.Pressed
                    && newMouseState.X >= 0
                    && newMouseState.X < GameProperty.resolutionWidth
                    && newMouseState.Y >= 0
                    && newMouseState.Y < GameProperty.resolutionHeight);

                var oldPress =
                    (oldButtonState == ButtonState.Pressed
                    && oldMouseState.X >= 0
                    && oldMouseState.X < GameProperty.resolutionWidth
                    && oldMouseState.Y >= 0
                    && oldMouseState.Y < GameProperty.resolutionHeight);

                if (newPress)
                {
                    pressMap[pair.Key] = true;
                    pushMap[pair.Key] = !oldPress;
                    releaseMap[pair.Key] = false;
                }
                else
                {
                    pressMap[pair.Key] = false;
                    pushMap[pair.Key] = false;
                    releaseMap[pair.Key] = oldPress;
                }
            }
            
            // Update saved state.
            oldKeyState = newKeyState;
            oldMouseState = newMouseState;

            // デバッグ出力
            var debugFont = DebugFontManager.GetInstance();
            var outputPos = new Vector2(900.0f, 22.0f * 6);

            var outputString = String.Format("mouse pos ({0}, {1})", MouseX, MouseY);
            debugFont.DrawString(new DebugFontInfo(outputString, outputPos));

            base.Update(gameTime);
        }

        void RegisterMode(InputLabel label, InputMode mode)
        {
            if (!modeMap.ContainsKey(label))
            {
                modeMap[label] = new List<InputMode>();
            }

            modeMap[label].Add(mode);
        }

        void RegisterKey(InputLabel label, Keys key)
        {
            virtualKeyMap[label] = key;
        }

        void RegisterMouse(InputLabel label, MouseButtons button)
        {
            virtualMouseMap[label] = button;
        }

        bool IsEnableLabel(InputLabel label)
        {
            return modeMap.ContainsKey(label) && modeMap[label].Contains(Mode);
        }

        public bool IsPress(InputLabel key)
        {
            return IsEnableLabel(key) && pressMap[key];
        }

        public bool IsPush(InputLabel key)
        {
            return IsEnableLabel(key) && pushMap[key];
        }

        public bool IsRelease(InputLabel key)
        {
            return IsEnableLabel(key) && releaseMap[key];
        }
    }
}
