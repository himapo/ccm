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
using ccm.Map;
using ccm.System;

namespace ccm.Scene
{
    public class DungeonScene : SceneBase
    {
        // プレイヤー
        Player.Player Player = new Player.Player();

        DungeonPlayerUpdater DungeonPlayerUpdater = new DungeonPlayerUpdater();

        PlayerDrawer PlayerDrawer = new PlayerDrawer();

        // 敵
        ccm.Enemy.EnemyManager EnemyManager;

        // 味方

        // マップ
        Dungeon Dungeon = new Dungeon();

        DungeonDrawer DungeonDrawer = new DungeonDrawer();

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

        public DungeonScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "DungeonScene";

            DungeonPlayerUpdater.CollisionManager = CollisionManager;
            DungeonPlayerUpdater.Camera = Camera;
            PlayerDrawer.Camera = Camera;

            var EnemyCreator = new Enemy.EnemyCreator()
            {
                UpdaterCreator = () =>
                {
                    return new DungeonEnemyUpdater()
                    {
                        EnemyManager = this.EnemyManager,
                        Player = this.Player,
                        CollisionManager = this.CollisionManager,
                    };
                },
                DrawerCreator = () =>
                {
                    return new EnemyDrawer()
                    {
                        Camera = this.Camera
                    };
                },
            };

            EnemyManager = new Enemy.EnemyManager()
            {
                Creator = EnemyCreator
            };

            DungeonDrawer.Camera = Camera;

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
            InitMap();
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

        void InitMap()
        {
            Dungeon.InitModel();
            Dungeon.Generate();
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
            EnemyManager.CreateEnemy(type, transform);
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

            DrawMap();

            DrawCollision();
        }

        void DrawCollision()
        {
            CollisionManager.Draw();
        }

        void DrawMap()
        {
            Dungeon.Draw(DungeonDrawer);
        }
    }
}
