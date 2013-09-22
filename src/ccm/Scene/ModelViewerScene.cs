using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Camera;
using HimaLib.Light;
using ccm.Input;
using ccm.Camera;
using ccm.Debug;
using ccm.Render;

namespace ccm.Scene
{
    public class ModelViewerScene : SceneBase
    {
        PerspectiveCamera camera = new PerspectiveCamera() { Far = 300.0f };

        ViewerCameraUpdater cameraUpdater;

        DirectionalLight DirectionalLight0 = new DirectionalLight();

        IModel model;

        ModelRenderParameter renderParam;

        SimpleModelRenderParameter simpleRenderParam = new SimpleModelRenderParameter();

        ToonModelRenderParameter toonRenderParam = new ToonModelRenderParameter();

        DefaultModelRenderParameter defaultRenderParam = new DefaultModelRenderParameter();

        DebugMenu debugMenu;

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer;

        string ModelName = "Not Loaded";

        string MotionName = "Not Loaded";

        string RendererName = "Simple";

        public ModelViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "ModelViewer";

            cameraUpdater = new ViewerCameraUpdater(camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                InitRotX = -MathUtil.PiOver4,
                InitRotY = MathUtil.PiOver4,
                InitEyeZ = 30.0f,
                //InitPan = Vector3.Up * 6.0f,
                MaxEyeZ = 200.0f,
                MinEyeZ = 1.0f,
                EyeZInterval = 0.05f,
                PanInterval = 0.2f,
                EnableCameraKey = false,
                EnablePan = true,
            };

            debugMenu = new DebugMenu("ModelViewerMenu");
            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
            debugMenuDrawer = new DefaultDebugMenuDrawer();
        }

        void UpdateStateInit()
        {
            InitCamera();

            InitLight();

            InitRenderer();

            InitModel();

            InitDebugMenu();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitCamera()
        {
            cameraUpdater.Reset();
            RenderSceneManager.Instance.GetPath(RenderPathType.OPAQUE).Camera = camera;
        }

        void InitLight()
        {
            RenderSceneManager.Instance.ClearDirectionalLight();

            DirectionalLight0.Direction = -Vector3.One;
            DirectionalLight0.Direction.Normalize();
            DirectionalLight0.Color = new Color(0.8f, 0.9f, 0.7f);
            RenderSceneManager.Instance.AddDirectionalLight(DirectionalLight0);
        }

        void InitRenderer()
        {
            InitSimpleRenderer();
            InitToonRenderer();
            InitDefaultRenderer();

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

        void InitDefaultRenderer()
        {
            //defaultRenderParam.ParametersVector3["Light1Direction"] = Vector3.One * 1.0f;
            //defaultRenderParam.ParametersVector3["Light1Color"] = new Vector3(0.8f, 0.9f, 0.7f);
            defaultRenderParam.ParametersVector3["Light2Direction"] = Vector3.One * -1.0f;
            defaultRenderParam.ParametersVector3["Light2Color"] = new Vector3(0.7f, 0.7f, 0.4f);
            defaultRenderParam.ParametersVector3["AmbientColor"] = new Vector3(0.1f, 0.1f, 0.1f);
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
                Label = "Motion"
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
            AddDefaultRenderer();

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

            MotionName = model.CurrentMotionName;

            debugMenu.ClearChildren("ModelViewerMenu.Motion");

            foreach(var motionName in model.MotionNames)
            {
                AddMotion(motionName);
            }
        }

        void AddMotion(string name)
        {
            debugMenu.AddChild("ModelViewerMenu.Motion", new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = name,
                ExecFunc = () =>
                {
                    ChangeMotion(name);
                    MotionName = name;
                }
            });
        }

        void ChangeMotion(string name)
        {
            if (model != null)
            {
                model.ChangeMotion(name, 0.0f);
            }
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

        void AddDefaultRenderer()
        {
            var rendererName = debugMenu.RootNode.Label + ".Renderer";

            debugMenu.AddChild(rendererName, new DebugMenuNodeInternal()
            {
                Label = "Default"
            });

            debugMenu.AddChild(rendererName + ".Default", new DebugMenuNodeExecutable()
            {
                Label = "Apply",
                ExecFunc = () =>
                {
                    renderParam = defaultRenderParam;
                    RendererName = "Default";
                }
            });
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Add(Name, 50.0f, 60.0f);

            DebugFont.Add("モデル     : " + ModelName, 800.0f, 60.0f);
            DebugFont.Add("モーション : " + MotionName, 800.0f, 82.0f);
            DebugFont.Add("レンダラ   : " + RendererName, 800.0f, 104.0f);

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                ChangeScene(new BootScene());
                return;
            }

            cameraUpdater.Update(Vector3.Zero);

            UpdateDefaultRenderer();

            if (model != null)
            {
                model.Update(0.008333f);
            }

            debugMenuUpdater.Update();
        }

        void UpdateDefaultRenderer()
        {
            defaultRenderParam.ParametersMatrix["World"] = Matrix.CreateRotationX(MathUtil.ToRadians(-90.0f));
        }

        void DrawStateMain()
        {
            if (model != null)
            {
                RenderSceneManager.Instance.RenderModel(model, renderParam);
            }

            debugMenu.Draw(debugMenuDrawer);
        }
    }
}
