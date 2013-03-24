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
using ccm.Debug;
using ccm.Deco;
using ccm.Ally;
using ccm.Item;
using ccm.Sound;

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
        ccm.Ally.AllyManager AllyManager;

        // アイテム
        ccm.Item.ItemWindow ItemWindow = new Item.ItemWindow();

        // マップ
        Dungeon Dungeon = new Dungeon();

        DungeonDrawer DungeonDrawer = new DungeonDrawer();

        // コリジョン
        HimaLib.Collision.CollisionManager CollisionManager = new HimaLib.Collision.CollisionManager();

        // カメラ
        BasicCamera Camera = new BasicCamera();

        ModelViewerCameraUpdater cameraUpdater;

        // デコ
        ccm.Deco.DecoManager DecoManager = new Deco.DecoManager();

        // サウンド
        SoundManager SoundManager { get { return SoundManager.Instance; } }

        // HUD

        // デバッグメニュー
        DebugMenu debugMenu = new DebugMenu("Dungeon Debug Menu");

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer = new DefaultDebugMenuDrawer();

        // その他
        //int Floor = 1;

        int Frame = 0;

        HimaLib.Math.IRand Rand
        {
            get { return GameRand.Instance; }
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
                        Camera = this.Camera,
                        DecoManager = this.DecoManager,
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

            var AllyCreator = new Ally.AllyCreator()
            {
                UpdaterCreator = () =>
                {
                    return new DungeonAllyUpdater()
                    {
                        AllyManager = this.AllyManager,
                        Player = this.Player,
                        CollisionManager = this.CollisionManager,
                        Camera = this.Camera,
                        DecoManager = this.DecoManager,
                    };
                },
                DrawerCreator = () =>
                {
                    return new AllyDrawer()
                    {
                        Camera = this.Camera
                    };
                },
            };

            AllyManager = new Ally.AllyManager()
            {
                Creator = AllyCreator
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

            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
        }

        void UpdateStateInit()
        {
            InitPlayer();
            InitAlly();
            InitMap();
            InitCollision();
            InitCamera();
            InitDebugMenu();

            //SoundManager.PlaySoundStream("-Blue Time-");

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitPlayer()
        {
            Player.InitModel();
            Player.AddAttachment("bonbon");
            Player.AddAttachment("negi");
        }

        void InitAlly()
        {
            for (var i = 0; i < 6; ++i)
            {
                AllyManager.CreateAlly(AllyType.Cube, CreateAllyTransform());
            }
        }

        AffineTransform CreateAllyTransform()
        {
            return new AffineTransform(
                Vector3.One,
                Vector3.Zero,
                new Vector3(Rand.NextFloat(-100.0f, 100.0f), 1.5f, Rand.NextFloat(-100.0f, 100.0f)));
        }

        void InitMap()
        {
            Dungeon.InitModel();
            Dungeon.Generate();
        }

        void InitCollision()
        {
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerBody, (int)Collision.CollisionGroup.EnemyBody);
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerAttack, (int)Collision.CollisionGroup.EnemyDamage);
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerAttack, (int)Collision.CollisionGroup.EnemyBody);
            CollisionManager.Drawer = new WireCollisionDrawer(Camera);
        }

        void InitCamera()
        {
            cameraUpdater.Reset();
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeTunableBool()
            {
                Label = "Draw Map",
                Selectable = true,
                Getter = () => { return Dungeon.Drawable; },
                Setter = (b) => { Dungeon.Drawable = b; },
            });
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            debugMenuUpdater.Update();

            DebugFont.Add(Name, 50.0f, 60.0f);

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                UpdateState = UpdateStateTerm;
                DrawState = DrawStateTerm;
                return;
            }

            Player.Update(DungeonPlayerUpdater);

            EnemyManager.Update();

            AllyManager.Update();

            ItemWindow.Update();

            DecoManager.Update();

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

            AllyManager.Draw();

            ItemWindow.Draw();

            DecoManager.Draw();

            DrawMap();

            DrawCollision();

            debugMenu.Draw(debugMenuDrawer);
        }

        void DrawCollision()
        {
            CollisionManager.Draw();
        }

        void DrawMap()
        {
            Dungeon.Draw(DungeonDrawer);
        }

        void UpdateStateTerm()
        {
            SoundManager.StopSoundStream("-Blue Time-");
            ChangeScene(new BootScene());
        }

        void DrawStateTerm()
        {
        }
    }
}
