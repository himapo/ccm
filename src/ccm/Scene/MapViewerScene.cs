using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using HimaLib.Model;
using HimaLib.Math;
using ccm.Input;
using ccm.Camera;
using ccm.DungeonLogic;
using ccm.Debug;

namespace ccm.Scene
{
    public class MapViewerScene : SceneBase
    {
        BasicCamera camera = new BasicCamera() { Near = 10.0f, Far = 10000.0f };

        ViewerCameraUpdater cameraUpdater;

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

            cameraUpdater = new ViewerCameraUpdater(camera, InputAccessor.GetController(ControllerLabel.Main))
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

            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
        }

        void UpdateStateInit()
        {
            InitCamera();

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
        }

        void InitRenderer()
        {
            renderParam.Camera = camera;
            renderParam.Transforms = new List<AffineTransform>();
            renderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            renderParam.DirLight0Direction = -Vector3.One;
            renderParam.DirLight0Direction.Normalize();
            renderParam.DirLight0DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
            renderParam.DirLight0SpecularColor = Vector3.One;
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
            renderParam.Transforms.Clear();

            var cubePosList = dungeonMap.GetCubePosList();
            foreach (var pos in cubePosList)
            {
                renderParam.Transforms.Add(
                    new HimaLib.Math.AffineTransform(
                        HimaLib.Math.Vector3.One,
                        HimaLib.Math.Vector3.Zero,
                        pos));
            }

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
                dungeonCubeModel.Render(renderParam);
                renderParam.TransformsUpdated = false;
            }

            debugMenu.Draw(debugMenuDrawer);
        }

    }
}
