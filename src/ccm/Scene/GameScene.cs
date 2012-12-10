using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using ccm.Input;
using ccm.Player;
using ccm.Camera;

namespace ccm.Scene
{
    public class GameScene : SceneBase
    {
        Player.Player Player = new Player.Player();

        PlayerDrawer PlayerDrawer = new PlayerDrawer();

        BasicCamera Camera = new BasicCamera();

        public GameScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "GameScene";
        }

        void UpdateStateInit()
        {
            InitPlayer();
            InitCamera();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitPlayer()
        {
            Player.AddAttachment("bonbon");
            Player.AddAttachment("negi");
        }

        void InitCamera()
        {
            Camera.Eye.Y = 6.0f;
            Camera.Eye.Z = 50.0f;
            Camera.At.Y = 6.0f;
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
        }

        void DrawStateMain()
        {
            PlayerDrawer.Camera = Camera;
            Player.Draw(PlayerDrawer);
        }
    }
}
