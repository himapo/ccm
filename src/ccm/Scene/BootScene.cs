using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Debug;
using ccm.Debug;
using ccm.Input;

namespace ccm.Scene
{
    public class BootScene : SceneBase
    {
        UIBillboardRenderer renderer;

        DebugMenu debugMenu;

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer;

        public BootScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "BootScene";

            debugMenu = new DebugMenu("BootMenu");
            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
            debugMenuDrawer = new DefaultDebugMenuDrawer();
        }

        void UpdateStateInit()
        {
            InitRenderer();

            InitDebugMenu();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitRenderer()
        {
            // コンストラクタではContentの初期化ができてないのでここで
            renderer = new UIBillboardRenderer();

            renderer.TextureName = "Texture/miki";
            renderer.Alpha = 1.0f;
            renderer.Scale = 1.0f;
            renderer.Rotation = new Vector3(0.0f);
            renderer.Position = new Vector3(0.0f);
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "Test Game",
                Selectable = true,
                ExecFunc = () =>
                {
                    Console.WriteLine("Execute Test Game");
                    ChangeScene(new HomeScene());
                }
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "Main Game",
                Selectable = true,
                ExecFunc = () => { Console.WriteLine("Execute Main Game"); }
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "Model Viewer",
                Selectable = true,
                ExecFunc = () =>
                {
                    Console.WriteLine("Execute Model Viewer");
                    ChangeScene(new ModelViewerScene());
                }
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "Map Viewer",
                Selectable = true,
                ExecFunc = () =>
                {
                    Console.WriteLine("Execute Map Viewer");
                    ChangeScene(new MapViewerScene());
                }
            });

            debugMenu.Open();
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            debugMenuUpdater.Update();

            DebugFont.Add(Name, 50.0f, 60.0f);

            DebugFont.Add("●γ「」ゑ", 200.0f, 20.0f);

            ShowMousePosition();
        }

        void DrawStateMain()
        {
            renderer.Render();

            debugMenu.Draw(debugMenuDrawer);
        }

        void ShowMousePosition()
        {
            var outputString = String.Format("mouse pos ({0}, {1})",
                InputAccessor.GetX(ControllerLabel.Main, PointingDeviceLabel.Mouse0),
                InputAccessor.GetY(ControllerLabel.Main, PointingDeviceLabel.Mouse0));
            DebugFont.Add(outputString, 900.0f, 22.0f * 6);
        }
    }
}
