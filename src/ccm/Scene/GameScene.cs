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

namespace ccm.Scene
{
    public class GameScene : SceneBase
    {
        // プレイヤー
        Player.Player Player = new Player.Player();

        DungeonPlayerUpdater DungeonPlayerUpdater = new DungeonPlayerUpdater();

        PlayerDrawer PlayerDrawer = new PlayerDrawer();

        // 敵
        EnemyCreator EnemyCreator = new EnemyCreator();

        ccm.Enemy.EnemyManager EnemyManager = new ccm.Enemy.EnemyManager();

        EnemyDrawer EnemyDrawer = new EnemyDrawer();

        // 味方

        // マップ

        // コリジョン
        HimaLib.Collision.CollisionManager CollisionManager = new HimaLib.Collision.CollisionManager();

        // カメラ
        BasicCamera Camera = new BasicCamera();

        ModelViewerCameraUpdater cameraUpdater;

        // デコ

        // HUD

        // その他
        int Floor = 1;

        int Frame = 0;

        IRand Rand
        {
            get { return GameProperty.gameRand; }
            set { }
        }

        public GameScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "GameScene";

            DungeonPlayerUpdater.CollisionManager = CollisionManager;
            DungeonPlayerUpdater.Camera = Camera;
            PlayerDrawer.Camera = Camera;

            EnemyDrawer.Camera = Camera;

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
        }

        void UpdateStateInit()
        {
            InitPlayer();
            InitCollision();
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

        void InitCollision()
        {
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerBody, (int)Collision.CollisionGroup.EnemyBody);
            CollisionManager.Drawer = new WireCollisionDrawer(Camera);
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

            EnemyManager.Update();

            if (IsTimeToCreateEnemy())
            {
                CreateEnemy(EnemyType.Cube, CalcEnemyAppearPosition());
            }

            UpdateCollision();

            UpdateCamera();
        }

        bool IsTimeToCreateEnemy()
        {
            if (++Frame >= 120)
            {
                Frame = 0;
                return true;
            }
            return false;
        }

        AffineTransform CalcEnemyAppearPosition()
        {
            return new AffineTransform(
                Vector3.One * 1.5f,
                Vector3.Zero,
                new Vector3(Rand.NextFloat(-100.0f, 100.0f), 1.5f, Rand.NextFloat(-100.0f, 100.0f)));
        }

        void CreateEnemy(EnemyType type, AffineTransform transform)
        {
            var enemy = EnemyCreator.Create(
                type,
                transform,
                new DungeonEnemyUpdater()
                {
                    Player = this.Player,
                    CollisionManager = this.CollisionManager,
                },
                EnemyDrawer);

            EnemyManager.Add(enemy);
        }

        void UpdateCollision()
        {
            CollisionManager.Detect();
        }

        void UpdateCamera()
        {
            var playerPos = Player.Transform.Translation;
            playerPos.Y += 6.0f;
            cameraUpdater.Update(playerPos);
        }

        void DrawStateMain()
        {
            Player.Draw(PlayerDrawer);

            EnemyManager.Draw();

            DrawCollision();
        }

        void DrawCollision()
        {
            CollisionManager.Draw();
        }
    }
}
