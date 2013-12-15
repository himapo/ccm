using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using HimaLib.Model;
using HimaLib.Math;
using HimaLib.Camera;
using HimaLib.Light;
using ccm.Input;
using ccm.Camera;
using ccm.DungeonLogic;
using ccm.Debug;
using ccm.Render;

namespace ccm.Scene
{
    public class MapViewerScene : SceneBase
    {
        PerspectiveCamera Camera = new PerspectiveCamera() { Near = 10.0f, Far = 10000.0f };

        ViewerCameraUpdater cameraUpdater;

        DirectionalLight DirectionalLight0 = new DirectionalLight();

        DungeonMap dungeonMap;

        HimaLib.Math.SystemRand rand = new HimaLib.Math.SystemRand();

        SimpleInstancingRenderParameter renderParam = new SimpleInstancingRenderParameter();

        IModel dungeonCubeModel;

        DebugMenu debugMenu = new DebugMenu("MapViewer");

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer = new DefaultDebugMenuDrawer();

        bool Drawable = true;

        public MapViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "MapViewer";

            cameraUpdater = new ViewerCameraUpdater(Camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                InitRotX = -MathUtil.PiOver4,
                InitRotY = MathUtil.PiOver4,
                InitEyeZ = 3000.0f,
                MaxEyeZ = 5000.0f,
                MinEyeZ = 1.0f,
                EyeZInterval = 1.0f,
                RotInterval = 0.04f,
                PanInterval = 10.0f,
                EnablePan = true,
            };

            debugMenuUpdater = new DebugMenuUpdater(debugMenu, BooleanDeviceLabel.SceneDebugMenu);
        }

        void UpdateStateInit()
        {
            InitCamera();

            InitLight();

            InitRenderer();

            InitModel();

            InitDungeon();

            InitDebugMenu();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitCamera()
        {
            cameraUpdater.Reset();
            cameraUpdater.Update(Vector3.Zero);
            RenderManager.Instance.GetPath(RenderPathType.SHADOW).Camera = Camera;
            RenderManager.Instance.GetPath(RenderPathType.GBUFFER).Camera = Camera;
            RenderManager.Instance.GetPath(RenderPathType.LIGHTBUFFER).Camera = Camera;
            RenderManager.Instance.GetPath(RenderPathType.OPAQUE).Camera = Camera;
            RenderManager.Instance.GetPath(RenderPathType.DEBUG).Camera = Camera;
            RenderManager.Instance.GetPath(RenderPathType.TRANSLUCENT).Camera = Camera;
        }

        void InitLight()
        {
            RenderManager.Instance.ClearDirectionalLight();

            DirectionalLight0.Direction = -Vector3.One;
            DirectionalLight0.Direction.Normalize();
            DirectionalLight0.Color = new Color(1.5f, 1.6f, 1.8f);
            RenderManager.Instance.AddDirectionalLight(DirectionalLight0);
        }

        void InitRenderer()
        {
            renderParam.Transform = Matrix.Identity;
            renderParam.Camera = Camera;
            renderParam.InstanceTransforms = new List<Matrix>();
            renderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            renderParam.IsShadowReceiver = false;
        }

        void InitModel()
        {
            dungeonCubeModel = ModelFactory.Instance.Create("cube000");
        }

        void InitDungeon()
        {
            rand.Init(Environment.TickCount);
            dungeonMap = new DungeonMap() { Rand = rand };
            ResetMap();
        }

        void ResetMap()
        {
            dungeonMap.Generate();

            var instanceTransforms = new List<Matrix>();

            var cubePosList = dungeonMap.GetCubePosList();
            foreach (var pos in cubePosList)
            {
                instanceTransforms.Add(Matrix.CreateTranslation(pos));
            }

            renderParam.InstanceTransforms = instanceTransforms;

            renderParam.TransformsUpdated = true;
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeTunableBool()
            {
                Label = "Draw Enable",
                Selectable = true,
                Getter = () => { return Drawable; },
                Setter = (b) => { Drawable = b; },
            });

            InputAccessor.SwitchController(ControllerLabel.Debug, true);
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            debugMenuUpdater.Update();

            DebugFont.Add(Name, 50.0f, 60.0f);

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                ChangeScene(new BootScene());
                return;
            }

            renderParam.TransformsUpdated = false;

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Jump))
            {
                ResetMap();
            }

            cameraUpdater.Update(Vector3.Zero);
        }

        void DrawStateMain()
        {
            if (Drawable)
            {
                RenderManager.Instance.RenderModel(dungeonCubeModel, renderParam);
            }

            debugMenu.Draw(debugMenuDrawer);
        }

    }
}
