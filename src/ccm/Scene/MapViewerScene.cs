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

        DungeonMap dungeonMap;

        HimaLib.Math.SystemRand rand;

        public MapViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "MapViewer";

            camera = new BasicCamera();
            rand = new HimaLib.Math.SystemRand();
            rand.Init(Environment.TickCount);
            dungeonMap = new DungeonMap() { Rand = rand };
        }

        void UpdateStateInit()
        {
            InitCamera();

            InitRenderer();

            ResetMap();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitCamera()
        {
            camera.Eye.Z = 10.0f;
            camera.Far = 10000.0f;

            camera.At = new HimaLib.Math.Vector3(0.0f, 0.0f, 1.0f);
            camera.Eye = HimaLib.Math.Vector3.Up * 3000.0f;
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
        }

        void DrawStateMain()
        {
            renderer.Render();
        }

    }
}
