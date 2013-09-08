using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Debug;
using HimaLib.Model;
using HimaLib.Texture;
using ccm.Debug;
using ccm.Input;
using ccm.Render;

namespace ccm.Scene
{
    public class BootScene : SceneBase
    {
        IBillboard billboard = BillboardFactory.Instance.Create();

        HudBillboardRenderParameter renderParam = new HudBillboardRenderParameter();

        DebugMenu debugMenu;

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer;

        public BootScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "BootScene";

            debugMenu = new DebugMenu("起動メニュー");
            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
            debugMenuDrawer = new DefaultDebugMenuDrawer();
        }

        void UpdateStateInit()
        {
            InitHud();

            InitRenderer();

            InitDebugMenu();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitHud()
        {
            renderParam.Texture = TextureFactory.Instance.CreateFromImage("Texture/miki");
            renderParam.Alpha = 1.0f;
            renderParam.Transform = new AffineTransform(
                Vector3.One,
                Vector3.Zero,
                Vector3.Zero);
            renderParam.IsTranslucent = false;
        }

        void InitRenderer()
        {
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "ゲームシーン",
                Selectable = true,
                ExecFunc = () =>
                {
                    Console.WriteLine("Execute Test Game");
                    ChangeScene(new HomeScene());
                }
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "メインシーケンス",
                Selectable = true,
                ExecFunc = () => { Console.WriteLine("Execute Main Game"); }
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "モデルビューア",
                Selectable = true,
                ExecFunc = () =>
                {
                    Console.WriteLine("Execute Model Viewer");
                    ChangeScene(new ModelViewerScene());
                }
            });
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeExecutable()
            {
                Label = "マップビューア",
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

            DebugFont.Add("●「記号いける」", 200.0f, 20.0f);

            var color = HimaLib.Math.Color.Yellow;
            DebugFont.Add(color.ToString(), 200.0f, 36.0f);

            ShowMousePosition();
        }

        void DrawStateMain()
        {
            RenderSceneManager.Instance.RenderBillboard(billboard, renderParam);

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
