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
using HimaLib.Texture;
using HimaLib.Model;
using HimaLib.Math;
using ccm.Render;
using ccm.Input;
using ccm.Sound;
using ccm.Game;
using ccm.Debug;

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

        // デバッグメニュー関連
        DebugMenu DebugMenu = new DebugMenu("グローバルデバッグメニュー");

        DebugMenuUpdater DebugMenuUpdater;

        DefaultDebugMenuDrawer DebugMenuDrawer = new DefaultDebugMenuDrawer();

        // レンダーターゲット描画関連
        IBillboard Billboard = BillboardFactory.Instance.Create();

        HudBillboardRenderParameter TargetRenderParam = new HudBillboardRenderParameter();

        bool ShowRenderTarget { get; set; }

        public RootObject()
        {
            gameController = new MainController(keyboard, mouse);
            debugController = new MainController(keyboard, mouse);

            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            DebugMenuUpdater = new DebugMenuUpdater(DebugMenu, BooleanDeviceLabel.GlobalDebugMenu);
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

            InitDebugMenu();

            InitRenderTargetHud();

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
            debugController.AddKeyboardKey(BooleanDeviceLabel.GlobalDebugMenu, KeyboardKeyLabel.F1);
            debugController.AddKeyboardKey(BooleanDeviceLabel.SceneDebugMenu, KeyboardKeyLabel.F2);

            InputAccessor.AddController(ControllerLabel.Debug, debugController, true);
        }

        void InitRender()
        {
            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.ShadowMap0,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight,
                SurfaceType.R32F,
                true, true);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.DiffuseLightMap,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight,
                SurfaceType.A8R8G8B8,
                true, true);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.SpecularLightMap,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight,
                SurfaceType.A8R8G8B8,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.GBuffer0,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight,
                SurfaceType.A8R8G8B8,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.GBuffer1,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight,
                SurfaceType.R32F,
                false, false);

            RenderSceneManager.Instance.AddPath(
                RenderPathType.SHADOW,
                new ShadowMapRenderPath()
                {
                    Name = "ShadowMap",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.ShadowMap0,
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.GBUFFER,
                new GBufferRenderPath()
                {
                    Name = "GBuffer",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndices = new int[]
                    {
                        (int)RenderTargetType.DiffuseLightMap,
                        (int)RenderTargetType.GBuffer0,
                        (int)RenderTargetType.GBuffer1,
                    }
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.LIGHTBUFFER,
                new LightBufferRenderPath()
                {
                    //Enabled = false,
                    Name = "LightBuffer",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndices = new int[]
                    {
                        (int)RenderTargetType.DiffuseLightMap,
                        (int)RenderTargetType.SpecularLightMap,
                    },
                    SphereModel = ModelFactory.Instance.Create("PointLightSphere"),
                    Billboard = BillboardFactory.Instance.Create(),
                    NormalMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer0),
                    DepthMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer1),
                    //ClearColor = Color.Gray,
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.DEFERRED,
                new DeferredRenderPath()
                {
                    Enabled = false,
                    Name = "Deferred",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                    Billboard = BillboardFactory.Instance.Create(),
                    AlbedoMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer0),
                    PositionMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer1),
                    NormalDepthMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer2),
                    ClearColor = Color.Gray,
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.OPAQUE,
                new OpaqueRenderPath()
                {
                    //Enabled = false,
                    Name = "Opaque",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                    ShadowMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0),
                    DiffuseLightMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.DiffuseLightMap),
                    SpecularLightMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.SpecularLightMap),
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.DEBUG,
                new DebugRenderPath()
                {
                    //Enabled = false,
                    Name = "Debug",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.TRANSLUCENT,
                new TranslucentRenderPath()
                {
                    Enabled = false,
                    Name = "Translucent",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                });

            RenderSceneManager.Instance.AddPath(
                RenderPathType.HUD,
                new HudRenderPath()
                {
                    Name = "HUD",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                });
        }

        void InitDebugMenu()
        {
            var nodeShowTarget = new DebugMenuNodeSelectable()
            {
                Label = "レンダーターゲット表示",
            };

            nodeShowTarget.AddChoice("なし", () =>
            {
                ShowRenderTarget = false;
            });

            nodeShowTarget.AddChoice("シャドウマップ", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
            });

            nodeShowTarget.AddChoice("拡散反射マップ", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.DiffuseLightMap);
            });

            nodeShowTarget.AddChoice("鏡面反射マップ", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.SpecularLightMap);
            });

            nodeShowTarget.AddChoice("Gバッファ0", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer0);
            });

            nodeShowTarget.AddChoice("Gバッファ1", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer1);
            });
            /*
            nodeShowTarget.AddChoice("Gバッファ2", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer2);
            });

            nodeShowTarget.AddChoice("Gバッファ3", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer3);
            });
            */
            DebugMenu.AddChild(DebugMenu.RootNode.Label, nodeShowTarget);
        }

        void InitRenderTargetHud()
        {
            TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
            TargetRenderParam.Alpha = 1.0f;
            TargetRenderParam.Transform = new AffineTransform(
                Vector3.One * 0.4f,
                Vector3.Zero,
                new Vector3(640.0f - 256.0f - 10.0f, 360.0f - 144.0f - 10.0f, 0.0f)).WorldMatrix;
            TargetRenderParam.IsTranslucent = false;
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugMenuUpdater.Update();

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

            if (ShowRenderTarget)
            {
                RenderSceneManager.Instance.RenderBillboard(Billboard, TargetRenderParam);
            }

            RenderSceneManager.Instance.Render();

            DrawDebugFPS();

            DebugMenu.Draw(DebugMenuDrawer);

            DebugFont.Draw();

            LoadProfiler.EndMark();
        }

        void DrawDebugFPS()
        {
            DebugFont.Add(string.Format("{0:f2}FPS", TimeKeeper.Instance.AverageFrameRate), 0.0f, 0.0f);
        }
    }
}
