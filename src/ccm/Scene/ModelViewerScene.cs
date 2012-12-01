using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using HimaLib.Math;
using ccm.Input;
using ccm.Camera;

namespace ccm.Scene
{
    public class ModelViewerScene : SceneBase
    {
        SimpleModelRenderer renderer;

        BasicCamera camera;

        ModelViewerCameraUpdater cameraUpdater;
        
        public ModelViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "ModelViewer";

            camera = new BasicCamera();
            cameraUpdater = new ModelViewerCameraUpdater(camera, InputAccessor.GetController(ControllerLabel.Main));
        }

        void UpdateStateInit()
        {
            InitCamera();

            InitRenderer();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitCamera()
        {
            camera.Eye.Z = 30.0f;
            camera.Far = 300.0f;
        }

        void InitRenderer()
        {
            renderer = new SimpleModelRenderer();

            renderer.ModelName = "Model/cube003";
            renderer.Transform = new AffineTransform(Vector3.One, Vector3.Zero, Vector3.Zero);
            renderer.Camera = camera;
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

            cameraUpdater.Update();
        }

        void DrawStateMain()
        {
            renderer.Render();
        }
    }
}
