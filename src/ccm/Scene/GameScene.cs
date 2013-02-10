using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Math;
using HimaLib.Collision;
using ccm.Input;
using ccm.Player;
using ccm.Camera;
using ccm.Enemy;
using ccm.Dungeon;

namespace ccm.Scene
{
    public class GameScene : SceneBase
    {
        Player.Player Player = new Player.Player();

        DungeonPlayerUpdater DungeonPlayerUpdater = new DungeonPlayerUpdater();

        PlayerDrawer PlayerDrawer = new PlayerDrawer();

        BasicCamera Camera = new BasicCamera();

        ModelViewerCameraUpdater cameraUpdater;

        ccm.Dungeon.Dungeon Dungeon;

        public GameScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "GameScene";

            cameraUpdater = new ModelViewerCameraUpdater(Camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                //InitRotX = -MathUtil.PiOver4,
                //InitRotY = MathUtil.PiOver4,
                InitEyeZ = 60.0f,
                MaxEyeZ = 110.0f,
                MinEyeZ = 40.0f,
                EyeZInterval = 0.2f,
                EnableCameraKey = true,
            };

            Dungeon = new Dungeon.Dungeon()
            {
                Camera = this.Camera,
                Player = this.Player,
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

            DungeonPlayerUpdater.Camera = Camera;
        }

        void InitCamera()
        {
            cameraUpdater.Reset();
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

            Player.Update(DungeonPlayerUpdater);

            UpdateCollision();

            UpdateCamera();

            Dungeon.Update();
        }

        void UpdateCollision()
        {
            HimaLib.Collision.CollisionManager.Instance.Detect();
        }

        void UpdateCamera()
        {
            var playerPos = Player.Transform.Translation;
            playerPos.Y += 6.0f;
            cameraUpdater.Update(playerPos);
        }

        void DrawStateMain()
        {
            PlayerDrawer.Camera = Camera;
            Player.Draw(PlayerDrawer);

            Dungeon.Draw();
        }
    }
}
