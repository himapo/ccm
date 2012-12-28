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

        ModelViewerCameraUpdater cameraUpdater;

        public GameScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "GameScene";

            cameraUpdater = new ModelViewerCameraUpdater(Camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                InitEyeZ = 50.0f,
                EnableCameraKey = true
            };
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
            Player.InitModel();
            Player.AddAttachment("bonbon");
            Player.AddAttachment("negi");
        }

        void InitCamera()
        {
            //Camera.Eye.Y = 6.0f;
            //Camera.Eye.Z = 50.0f;
            //Camera.At.Y = 6.0f;

            cameraUpdater.MinEyeZ = 40.0f;
            cameraUpdater.MaxEyeZ = 110.0f;
            cameraUpdater.EyeZInterval = 0.2f;
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

            var playerPos = Player.Transform.Translation;
            playerPos.Y += 6.0f;
            cameraUpdater.Update(playerPos);
            
            Player.Update();
        }

        void DrawStateMain()
        {
            PlayerDrawer.Camera = Camera;
            Player.Draw(PlayerDrawer);
        }
    }
}
