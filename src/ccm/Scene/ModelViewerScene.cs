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

        IModelRenderParameter renderParam;

        SimpleModelRenderParameter simpleRenderParam = new SimpleModelRenderParameter();

        ToonModelRenderParameter toonRenderParam = new ToonModelRenderParameter();

        DebugMenu debugMenu;

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer;

        string ModelName = "Not Loaded";

        string RendererName = "Simple";

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
            InitSimpleRenderer();
            InitToonRenderer();

            renderParam = simpleRenderParam;
        }

        void InitSimpleRenderer()
        {
            simpleRenderParam.Camera = camera;
        }

        void InitToonRenderer()
        {
            toonRenderParam.Camera = camera;
        }

        void InitModel()
        {
            
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new DebugMenuNodeInternal()
            {
                Label = "Model"
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new DebugMenuNodeInternal()
            {
                Label = "Renderer"
            });

            EnumerateModelNames().ForEach((name) =>
            {
                AddModel(Path.GetFileNameWithoutExtension(name));
            });

            AddSimpleRenderer();
            AddToonRenderer();

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
            debugMenu.AddChild("ModelViewerMenu.Model", new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = name,
                ExecFunc = () =>
                {
                    LoadModel(name);
                    ModelName = name;
                }
            });
        }

        void LoadModel(string name)
        {
            model = ModelFactory.Instance.Create(name);
        }

        void AddSimpleRenderer()
        {
            var rendererName = debugMenu.RootNode.Label + ".Renderer";

            debugMenu.AddChild(rendererName, new DebugMenuNodeInternal()
            {
                Label = "Simple"
            });

            debugMenu.AddChild(rendererName + ".Simple", new DebugMenuNodeExecutable()
            {
                Label = "Apply",
                ExecFunc = () =>
                {
                    renderParam = simpleRenderParam;
                    RendererName = "Simple";
                }
            });
        }

        void AddToonRenderer()
        {
            var rendererName = debugMenu.RootNode.Label + ".Renderer";

            debugMenu.AddChild(rendererName, new DebugMenuNodeInternal()
            {
                Label = "Toon"
            });

            debugMenu.AddChild(rendererName + ".Toon", new DebugMenuNodeExecutable()
            {
                Label = "Apply",
                ExecFunc = () =>
                {
                    renderParam = toonRenderParam;
                    RendererName = "Toon";
                }
            });
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Add(Name, 50.0f, 60.0f);

            DebugFont.Add("Model    : " + ModelName, 800.0f, 60.0f);
            DebugFont.Add("Renderer : " + RendererName, 800.0f, 82.0f);

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
