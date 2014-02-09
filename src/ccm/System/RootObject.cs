using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        // トーンマッピング関連
        ToneMappingRenderParameter ToneMappingRenderParameter;

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
            GameRand.Instance.Init(Environment.TickCount);
            DrawRand.Instance.Init(Environment.TickCount);

            keyboard.Game = Game;
            mouse.Game = Game;

            InitSystem();

            InitController();

            SoundManager.Initialize();

            InitRender();

            InitDebugFont();

            InitDebugMenu();

            InitRenderTargetHud();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitSystem()
        {
            GraphicsOption.Create();
        }

        void InitController()
        {
            mouse.FixedAtCenter = true;
            mouse.Visible = false;

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

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.HDRBuffer,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight,
                SurfaceType.A8R8G8B8,
                true, true);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.ScaledBuffer,
                GameProperty.resolutionWidth / 4,
                GameProperty.resolutionHeight / 4,
                SurfaceType.A8R8G8B8,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.LuminanceBuffer64,
                64,
                64,
                SurfaceType.R16F,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.LuminanceBuffer16,
                16,
                16,
                SurfaceType.R16F,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.LuminanceBuffer4,
                4,
                4,
                SurfaceType.R16F,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.LuminanceBuffer1,
                1,
                1,
                SurfaceType.R16F,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.AdaptedLuminanceBuffer0,
                1,
                1,
                SurfaceType.R16F,
                false, false);

            RenderTargetManager.Instance.AddRenderTarget(
                (int)RenderTargetType.AdaptedLuminanceBuffer1,
                1,
                1,
                SurfaceType.R16F,
                false, false);

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.SHADOW,
                new ShadowMapRenderPath()
                {
                    //Enabled = false,
                    Name = "ShadowMap",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.ShadowMap0,
                });

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.GBUFFER,
                new GBufferRenderPath()
                {
                    //Enabled = false,
                    Name = "GBuffer",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndices = new int[]
                    {
                        (int)RenderTargetType.DiffuseLightMap,
                        (int)RenderTargetType.GBuffer0,
                        (int)RenderTargetType.GBuffer1,
                    }
                });

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.LIGHTBUFFER,
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

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.DEFERRED,
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

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.OPAQUE,
                new OpaqueRenderPath()
                {
                    //Enabled = false,
                    Name = "Opaque",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.HDRBuffer,
                    ShadowMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0),
                    DiffuseLightMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.DiffuseLightMap),
                    SpecularLightMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.SpecularLightMap),
                });

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.DEBUG,
                new DebugRenderPath()
                {
                    //Enabled = false,
                    Name = "Debug",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.HDRBuffer,
                });

            ToneMappingRenderParameter = new ToneMappingRenderParameter()
            {
                HDRScene = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.HDRBuffer),
                ScaledBufferIndex = (int)RenderTargetType.ScaledBuffer,
                ScaledBuffer = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ScaledBuffer),
                LuminanceBufferIndices = new int[]
                {
                    (int)RenderTargetType.LuminanceBuffer64,
                    (int)RenderTargetType.LuminanceBuffer16,
                    (int)RenderTargetType.LuminanceBuffer4,
                    (int)RenderTargetType.LuminanceBuffer1,
                },
                LuminanceBuffers = new ITexture[]
                {
                    TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.LuminanceBuffer64),
                    TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.LuminanceBuffer16),
                    TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.LuminanceBuffer4),
                    TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.LuminanceBuffer1),
                },
                AdaptedLuminanceBufferIndices = new int[]
                {
                    (int)RenderTargetType.AdaptedLuminanceBuffer0,
                    (int)RenderTargetType.AdaptedLuminanceBuffer1,
                },
                AdaptedLuminanceBuffers = new ITexture[]
                {
                    TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.AdaptedLuminanceBuffer0),
                    TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.AdaptedLuminanceBuffer1),
                },
                RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                Exposure = 0.8f,
            };

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.TONEMAPPING,
                new ToneMappingRenderPath()
                {
                    //Enabled = false,
                    Name = "ToneMapping",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                    Billboard = BillboardFactory.Instance.Create(),
                    RenderParam = ToneMappingRenderParameter,
                });

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.TRANSLUCENT,
                new TranslucentRenderPath()
                {
                    //Enabled = false,
                    Name = "Translucent",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                });

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.HUD,
                new HudRenderPath()
                {
                    //Enabled = false,
                    Name = "HUD",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                });

            RenderManagerAccessor.Instance.AddPath(
                (int)RenderPathType.DEBUGFONT,
                new FontRenderPath()
                {
                    //Enabled = false,
                    Name = "DebugFont",
                    RenderDevice = RenderDeviceFactory.Instance.Create(),
                    RenderTargetIndex = (int)RenderTargetType.BackBuffer,
                });

            FrameCacheData.Create();
        }

        void InitDebugFont()
        {
            DebugFont.Create();
            DebugFontBase.Instance.RenderManager = RenderManagerAccessor.Instance;
            DebugFontBase.Instance.FontName = "SpriteFont/Debug";
        }

        void InitDebugMenu()
        {
            InitDebugMenuGraphicsOption();

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

            var nodeExposure = new DebugMenuNodeTunableFloat()
            {
                Label = "HDR露出",
                Getter = () => { return ToneMappingRenderParameter.Exposure; },
                Setter = (value) => { ToneMappingRenderParameter.Exposure = value; },
                Interval = 0.1f,
            };

            DebugMenu.AddChild(DebugMenu.RootNode.Label, nodeExposure);
        }

        void InitDebugMenuGraphicsOption()
        {
            var graphicsOptionNode = new DebugMenuNodeInternal()
            {
                Label = "描画オプション",
                ExecFunc = GraphicsOptionBase.Instance.GetCurrentSetting,
            };

            DebugMenu.AddChild(DebugMenu.RootNode.Label, graphicsOptionNode);

            var graphicsOptionLabel = DebugMenu.RootNode.Label + "." + graphicsOptionNode.Label;

            DebugMenu.AddChild(graphicsOptionLabel, new DebugMenuNodeExecutable()
            {
                Label = "適用",
                ExecFunc = GraphicsOptionBase.Instance.Apply,
            });

            DebugMenu.AddChild(graphicsOptionLabel, new DebugMenuNodeTunableBool
            {
                Label = "VSync有効",
                Getter = () => { return GraphicsOptionBase.Instance.VSyncEnable; },
                Setter = (value) => { GraphicsOptionBase.Instance.VSyncEnable = value; },
            });
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
            LoadProfiler.BeginMark("UpdateMain");
            
            FrameCacheDataBase.Instance.Clear();

            RenderManagerAccessor.Instance.StartRender();

            DebugMenuUpdater.Update();

            TimeKeeper.Instance.Update();

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
            LoadProfiler.BeginMark("DrawMain");

            CurrentScene.Draw();

            if (ShowRenderTarget)
            {
                RenderManagerAccessor.Instance.RenderBillboard(Billboard, TargetRenderParam);
            }

            DrawDebugFPS();

            DebugMenu.Draw(DebugMenuDrawer);

            LoadProfiler.EndMark();

            LoadProfiler.BeginMark("WaitRender");

            RenderManagerAccessor.Instance.WaitRender();

            LoadProfiler.EndMark();
        }

        void DrawDebugFPS()
        {
            DebugFontBase.Instance.Draw(string.Format("{0:f2}FPS", TimeKeeper.Instance.AverageFrameRate), 0.0f, 0.0f);
        }
    }
}
