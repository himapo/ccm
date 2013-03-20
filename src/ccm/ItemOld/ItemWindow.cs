using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ccm.CameraOld;


namespace ccm
{
    enum ItemIconState
    {
        Default,    // 普通にウィンドウに置かれている
        Drag,       // ドラッグされている
        Drop,       // ドロップされそうになってる
    }

    class ItemIconInfo
    {
        public int Type { get; set; }
        public ItemIconState State { get; set; }
    }

    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class ItemWindow : MyGameComponent
    {
        enum State
        {
            Closed,
            Opening,
            Open,
            Closing,
        }

        State state;

        Action<GameTime> updateFunc;
        Action<GameTime> drawFunc;

        Texture2D windowTexture;
        Texture2D iconTexture;

        bool drag = false;

        Vector3 position;
        public Vector3 Position { get { return position; } set { position = value; } }

        float rot = 0.0f;

        List<ItemIconInfo> iconInfoList;

        public ItemWindow(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.UI;

            position = new Vector3(0.0f, 0.0f, 0.0f);

            iconInfoList = new List<ItemIconInfo>();

            // TODO: ここで子コンポーネントを作成します。
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            SetState(State.Closed);

            iconInfoList.Add(new ItemIconInfo { Type = 0, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 1, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 2, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 0, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 1, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 2, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 0, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 1, State = ItemIconState.Default });
            iconInfoList.Add(new ItemIconInfo { Type = 2, State = ItemIconState.Default });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            windowTexture = ResourceManager.GetInstance().Load<Texture2D>("Texture/ItemWindow000");
            iconTexture = ResourceManager.GetInstance().Load<Texture2D>("Texture/ItemIcon000");

            base.LoadContent();
        }

        void SetState(State state)
        {
            if (state == State.Closed)
            {
                updateFunc = UpdateClosed;
                drawFunc = DrawClosed;
            }
            else if (state == State.Opening)
            {
                updateFunc = UpdateOpening;
                drawFunc = DrawOpening;
            }
            else if (state == State.Open)
            {
                updateFunc = UpdateOpen;
                drawFunc = DrawOpen;
            }
            else if (state == State.Closing)
            {
                updateFunc = UpdateClosing;
                drawFunc = DrawClosing;
            }

            this.state = state;
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.GAME_SCENE)
            {
                Enabled = true;
                Visible = true;
            }
            else
            {
                Enabled = false;
                Visible = false;
            }

            SetState(State.Closed);
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。
            updateFunc(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            drawFunc(gameTime);

            base.Draw(gameTime);
        }

        void UpdateClosed(GameTime gameTime)
        {
            var inputService = InputManager.GetInstance();

            // 開く
            if (inputService.IsPush(InputLabel.ItemWindow))
            {
                SetState(State.Opening);
            }
        }

        void DrawClosed(GameTime gameTime)
        {
        }

        void UpdateOpening(GameTime gameTime)
        {
            SetState(State.Open);
        }

        void DrawOpening(GameTime gameTime)
        {
        }

        void UpdateOpen(GameTime gameTime)
        {
            var inputService = InputManager.GetInstance();

            // ドラッグ処理
            if (inputService.IsPush(InputLabel.MouseMain))
            {
                drag = false;

                // ドラッグ領域のウィンドウ中心からの相対位置
                const float DRAG_RECT_TOP = 128.0f;
                const float DRAG_RECT_BOTTOM = DRAG_RECT_TOP - 52.0f;
                const float DRAG_RECT_LEFT = -128.0f;
                const float DRAG_RECT_RIGHT = 128.0f;

                var x = (float)inputService.MouseX;
                var y = (float)inputService.MouseY;

                if (x > Position.X + DRAG_RECT_LEFT
                    && x < Position.X + DRAG_RECT_RIGHT
                    && y < Position.Y + DRAG_RECT_TOP
                    && y > Position.Y + DRAG_RECT_BOTTOM)
                {
                    drag = true;
                }
            }
            else if (drag && inputService.IsPress(InputLabel.MouseMain))
            {
                position.X += inputService.MouseMoveX;
                position.Y += inputService.MouseMoveY;
            }

            // アイコンドラッグ処理
            DragIcon();

            // 閉じる
            if (inputService.IsPush(InputLabel.ItemWindow))
            {
                SetState(State.Closing);
            }
        }

        void DragIcon()
        {
            var inputService = InputManager.GetInstance();

            var pushMain = inputService.IsPush(InputLabel.MouseMain);
            var pushSub = inputService.IsPush(InputLabel.MouseSub);

            var mouseOnIcon = -1;   // マウスが乗ってるアイコン番号

            for (var i = 0; i < iconInfoList.Count; ++i)
            {
                var iconInfo = iconInfoList[i];

                var x = (float)inputService.MouseX;
                var y = (float)inputService.MouseY;

                var iconPos = new Vector2();
                iconPos.X = Position.X + 51.0f * (i % 5 - 2);
                iconPos.Y = Position.Y - 51.0f * (i / 5 - 1);

                const float ICON_WIDTH_HALF = 25.0f;

                if (x > iconPos.X - ICON_WIDTH_HALF
                    && x < iconPos.X + ICON_WIDTH_HALF
                    && y < iconPos.Y + ICON_WIDTH_HALF
                    && y > iconPos.Y - ICON_WIDTH_HALF)
                {
                    mouseOnIcon = i;
                }
                else if (iconInfo.State == ItemIconState.Drop)
                {
                    iconInfo.State = ItemIconState.Default;
                }
            }

            // ドラッグされているものがあればそれを処理
            for (var i = 0; i < iconInfoList.Count; ++i)
            {
                if(i == mouseOnIcon)
                    continue;

                var iconInfo = iconInfoList[i];
                if (iconInfo.State == ItemIconState.Drag)
                {
                    if (mouseOnIcon == -1)
                    {
                        if (pushSub)
                        {
                            // 移動キャンセル
                            iconInfo.State = ItemIconState.Default;
                        }
                    }
                    else
                    {
                        if (pushMain)
                        {
                            // 入れ替え
                            iconInfo.State = ItemIconState.Default;
                            iconInfoList[mouseOnIcon].State = ItemIconState.Default;
                            GeneralUtil.Swap<ItemIconInfo>(iconInfoList, i, mouseOnIcon);
                        }
                        else if (pushSub)
                        {
                            // 移動キャンセル
                            iconInfo.State = ItemIconState.Default;
                            iconInfoList[mouseOnIcon].State = ItemIconState.Default;
                        }
                        else
                        {
                            // ドラッグアイコンが上に乗ってたらドロップ状態にする
                            iconInfoList[mouseOnIcon].State = ItemIconState.Drop;
                        }
                    }

                    return;
                }
            }

            // ドラッグされているものがなければ新たにドラッグされたか判定
            if (pushMain && mouseOnIcon != -1)
            {
                iconInfoList[mouseOnIcon].State = ItemIconState.Drag;
            }
        }

        void DrawOpen(GameTime gameTime)
        {
            var inputService = InputManager.GetInstance();

            // ウィンドウの描画
            {
                //rot += GameProperty.GetUpdateScale(gameTime) * 1.0f;
                var renderParam = new UIRenderParameter();
                renderParam.cameraLabel = CameraLabel.UI;
                renderParam.DiffuseMap = windowTexture;
                renderParam.world =
                    Matrix.CreateScale(1.0f) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(rot)) *
                    Matrix.CreateTranslation(Position);
                renderParam.renderer = RendererLabel.UI;
                renderParam.cullEnable = false;

                renderParam.RectOffset = new Vector2(0.0f, 0.0f);
                renderParam.RectSize = new Vector2(256.0f, 256.0f);
                renderParam.Alpha = 1.0f;

                RenderManager.GetInstance().Register(renderParam);
            }

            // アイコンの描画
            for (var i = 0; i < iconInfoList.Count; ++i)
            {
                var iconInfo = iconInfoList[i];

                var renderParam = new UIRenderParameter();
                renderParam.cameraLabel = CameraLabel.UI;
                renderParam.DiffuseMap = iconTexture;
                var pos = new Vector3();

                if (iconInfo.State == ItemIconState.Default)
                {
                    pos.X = Position.X + 51.0f * (i % 5 - 2);
                    pos.Y = Position.Y - 51.0f * (i / 5 - 1);
                    pos.Z = 0.0f;
                }
                else if (iconInfo.State == ItemIconState.Drag)
                {
                    pos.X = inputService.MouseX;
                    pos.Y = inputService.MouseY;
                    pos.Z = -0.5f;
                }
                else if (iconInfo.State == ItemIconState.Drop)
                {
                    pos.X = Position.X + 51.0f * (i % 5 - 2) + 8.0f;
                    pos.Y = Position.Y - 51.0f * (i / 5 - 1) + 8.0f;
                    pos.Z = -0.1f;
                }

                renderParam.world = Matrix.CreateTranslation(pos);
                renderParam.renderer = RendererLabel.UI;
                renderParam.cullEnable = false;

                renderParam.RectOffset = new Vector2(
                    1.0f + 51.0f * (iconInfo.Type % 5),
                    1.0f + 51.0f * (iconInfo.Type / 5));
                renderParam.RectSize = new Vector2(50.0f, 50.0f);
                renderParam.Alpha = 0.5f;

                RenderManager.GetInstance().Register(renderParam);
            }
        }

        void UpdateClosing(GameTime gameTime)
        {
            SetState(State.Closed);
        }

        void DrawClosing(GameTime gameTime)
        {
        }
    }
}
