using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using ccm.Input;
using ccm.Camera;
using ccm.DungeonLogic;

namespace ccm.Scene
{
    public class MapViewerScene : SceneBase
    {
        SimpleInstancingRenderer renderer;

        BasicCamera camera;

        MapViewerCameraUpdater cameraUpdater;

        DungeonMap dungeonMap;

        HimaLib.Math.SystemRand rand;

        public MapViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "MapViewer";

            camera = new BasicCamera() { Near = 10.0f, Far = 10000.0f };
            cameraUpdater = new MapViewerCameraUpdater(camera, InputAccessor.GetController(ControllerLabel.Main));
            rand = new HimaLib.Math.SystemRand();
            rand.Init(Environment.TickCount);
            dungeonMap = new DungeonMap() { Rand = rand };
        }

        void UpdateStateInit()
        {
            InitRenderer();

            ResetMap();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitRenderer()
        {
            renderer = new SimpleInstancingRenderer();

            renderer.ModelName = "Model/cube000";
            renderer.Transforms = new List<HimaLib.Math.AffineTransform>();            
            renderer.Camera = camera;
        }

        void ResetMap()
        {
            dungeonMap.Generate();
            renderer.Transforms.Clear();

            var cubePosList = dungeonMap.GetCubePosList();
            foreach (var pos in cubePosList)
            {
                renderer.Transforms.Add(
                    new HimaLib.Math.AffineTransform(
                        HimaLib.Math.Vector3.One,
                        HimaLib.Math.Vector3.Zero,
                        pos));
            }
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
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

            cameraUpdater.Update();
        }

        void DrawStateMain()
        {
            renderer.Render();
        }

    }
}
