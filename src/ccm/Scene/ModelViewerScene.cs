using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Model;
using ccm.Input;
using ccm.Camera;
using ccm.Debug;

namespace ccm.Scene
{
    public class ModelViewerScene : SceneBase
    {
        BasicCamera camera = new BasicCamera() { Far = 300.0f };

        ModelViewerCameraUpdater cameraUpdater;

        IModel model;
        
        SimpleModelRenderParameter renderParam = new SimpleModelRenderParameter();

        DebugMenu debugMenu;

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer;

        public ModelViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "ModelViewer";

            cameraUpdater = new ModelViewerCameraUpdater(camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                InitRotX = -MathUtil.PiOver4,
                InitRotY = MathUtil.PiOver4,
                InitEyeZ = 30.0f,
                MaxEyeZ = 110.0f,
                MinEyeZ = 10.0f,
                EyeZInterval = 0.1f,
                //EnableCameraKey = true,
            };

            debugMenu = new DebugMenu("ModelViewerMenu");
            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
            debugMenuDrawer = new DefaultDebugMenuDrawer();
        }

        void UpdateStateInit()
        {
            InitCamera();

            InitRenderer();

            InitModel();

            InitDebugMenu();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitCamera()
        {
            cameraUpdater.Reset();
        }

        void InitRenderer()
        {
            renderParam.Camera = camera;
            renderParam.Transform = new AffineTransform();
            renderParam.Alpha = 1.0f;
            renderParam.AmbientLightColor = Vector3.One * 0.4f;
            renderParam.DirLight0Direction = Vector3.One * -1.0f;
            renderParam.DirLight0DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
        }

        void InitModel()
        {
            
        }

        void InitDebugMenu()
        {
            EnumerateModelNames().ForEach((name) =>
            {
                AddModel(Path.GetFileNameWithoutExtension(name));
            });

            debugMenu.Open();
        }

        List<string> EnumerateModelNames()
        {
            var result = new List<string>();

            var current = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(@"..\..\..\..\ccmContent");
            
            result.AddRange(Directory.GetFiles("Model", "*.x"));
            result.AddRange(Directory.GetFiles("Model", "*.fbx"));
            result.AddRange(Directory.GetFiles("Model", "*.pmd"));

            Directory.SetCurrentDirectory(current);

            return result; 
        }

        void AddModel(string name)
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = name,
                Selectable = true,
                ExecFunc = () =>
                {
                    LoadModel(name);
                }
            });
        }

        void LoadModel(string name)
        {
            model = ModelFactory.Instance.Create(name);
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

            cameraUpdater.Update(Vector3.Zero);

            debugMenuUpdater.Update();
        }

        void DrawStateMain()
        {
            if (model != null)
            {
                model.Render(renderParam);
            }

            debugMenu.Draw(debugMenuDrawer);
        }
    }
}
