using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Math;
using HimaLib.Collision;
using HimaLib.Camera;
using HimaLib.Light;
using HimaLib.Render;
using HimaLib.Model;
using HimaLib.Texture;
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
using ccm.Render;

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
        PerspectiveCamera Camera = new PerspectiveCamera();

        TPSCameraUpdater cameraUpdater;

        // ライト
        DirectionalLight DirectionalLight0 = new DirectionalLight();

        List<PointLight> PointLights = new List<PointLight>();

        List<Vector3> PointLightPositions = new List<Vector3>();

        // デコ
        ccm.Deco.DecoManager DecoManager = new Deco.DecoManager();

        // サウンド
        SoundManager SoundManager { get { return SoundManager.Instance; } }

        // HUD

        // デバッグメニュー
        DebugMenu debugMenu = new DebugMenu("DungeonScene デバッグメニュー");

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer = new DefaultDebugMenuDrawer();

        // その他
        //int Floor = 1;

        int Frame = 0;

        bool lightUpdate = false;

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
            DungeonPlayerUpdater.Dungeon = Dungeon;
            DungeonPlayerUpdater.Camera = Camera;
            DungeonPlayerUpdater.DecoManager = DecoManager;
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

            Dungeon.CollisionManager = CollisionManager;
            DungeonDrawer.Camera = Camera;

            cameraUpdater = new TPSCameraUpdater(Camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                InitRotX = -MathUtil.PiOver4,
                InitRotY = MathUtil.PiOver4,
                InitEyeZ = 40.0f,
                MaxEyeZ = 110.0f,
                MinEyeZ = 40.0f,
                EyeZInterval = 0.2f,
                RotInterval = 0.02f,
                PanInterval = 0.2f,
                MaxRotX = MathUtil.ToRadians(30.0f),
            };

            debugMenuUpdater = new DebugMenuUpdater(debugMenu, BooleanDeviceLabel.SceneDebugMenu);

            Dungeon.Drawable = true;
            CollisionManager.Drawable = false;
        }

        void UpdateStateInit()
        {
            InitPlayer();
            InitEnemy();
            InitAlly();
            InitMap();
            InitCollision();
            InitCamera();
            InitLight();
            InitDebugMenu();
            InitRender();
            InitStage();

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
        void InitEnemy()
        {
            for (var i = 0; i < 6; ++i)
            {
                EnemyManager.CreateEnemy(EnemyType.Cube, CalcEnemyAppearPosition());
            }
        }

        void InitAlly()
        {
            for (var i = 0; i < 3; ++i)
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
            Dungeon.GenerateFloorCollision();
            Dungeon.GenerateWallCollision();
        }

        void InitCollision()
        {
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerBody, (int)Collision.CollisionGroup.EnemyBody);
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerGround, (int)Collision.CollisionGroup.Floor);
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerBody, (int)Collision.CollisionGroup.Wall);
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.PlayerAttack, (int)Collision.CollisionGroup.EnemyDamage);
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.EnemyAttack, (int)Collision.CollisionGroup.PlayerDamage);
            CollisionManager.Drawer = new WireCollisionDrawer()
            {
                RenderScene = RenderSceneManager.Instance.RenderScene,
            };
        }

        void InitCamera()
        {
            Camera.Near = 20.0f;
            Camera.Far = 200.0f;

            cameraUpdater.Reset();
            RenderSceneManager.Instance.GetPath(RenderPathType.SHADOW).Camera = Camera;
            RenderSceneManager.Instance.GetPath(RenderPathType.GBUFFER).Camera = Camera;
            RenderSceneManager.Instance.GetPath(RenderPathType.LIGHTBUFFER).Camera = Camera;
            RenderSceneManager.Instance.GetPath(RenderPathType.OPAQUE).Camera = Camera;
            RenderSceneManager.Instance.GetPath(RenderPathType.DEBUG).Camera = Camera;
            RenderSceneManager.Instance.GetPath(RenderPathType.TRANSLUCENT).Camera = Camera;
        }

        void InitLight()
        {
            RenderSceneManager.Instance.ClearDirectionalLight();

            DirectionalLight0.Direction = -Vector3.One;
            DirectionalLight0.Color = new Color(2.0f, 2.0f, 2.0f, 1.0f);
            RenderSceneManager.Instance.AddDirectionalLight(DirectionalLight0);

            RenderSceneManager.Instance.ClearPointLight();

            for (var i = 0; i < 16; ++i)
            {
                PointLightPositions.Add(new Vector3(Rand.NextFloat(-40.0f, 40.0f), Rand.NextFloat(0.0f, 20.0f), Rand.NextFloat(-40.0f, 40.0f)));

                var light = new PointLight()
                {
                    Position = Player.Transform.Translation + PointLightPositions[i],
                    Color = new Color(Rand.NextFloat(100.5f, 120.0f), Rand.NextFloat(100.5f, 120.0f), Rand.NextFloat(100.5f, 120.0f)),
                    AttenuationBegin = Rand.NextFloat(10.0f, 18.0f),
                    AttenuationEnd = Rand.NextFloat(20.0f, 30.0f),
                };
                PointLights.Add(light);
            }

            PointLights.ForEach((light) => { RenderSceneManager.Instance.AddPointLight(light); });
        }

        void InitDebugMenu()
        {
            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeTunableBool()
            {
                Label = "マップ描画",
                Selectable = true,
                Getter = () => { return Dungeon.Drawable; },
                Setter = (b) => { Dungeon.Drawable = b; },
            });

            debugMenu.AddChild(debugMenu.RootNode.Label, new HimaLib.Debug.DebugMenuNodeTunableBool()
            {
                Label = "コリジョン描画",
                Selectable = true,
                Getter = () => { return CollisionManager.Drawable; },
                Setter = (b) => { CollisionManager.Drawable = b; },
            });
        }

        void InitRender()
        {
        }

        void InitStage()
        {
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            debugMenuUpdater.Update();

            DebugFont.Add(Name, 50.0f, 60.0f);

            Player.Update(DungeonPlayerUpdater);

            EnemyManager.Update();

            AllyManager.Update();

            ItemWindow.Update();

            DecoManager.Update();

            if (IsTimeToCreateEnemy())
            {
                //CreateEnemy(EnemyType.Cube, CalcEnemyAppearPosition());
            }

            UpdateCollision();

            UpdateCamera();

            UpdateLight();

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                UpdateState = UpdateStateTerm;
                DrawState = DrawStateTerm;
                return;
            }

            // TODO : この判定の仕方はない
            if (DungeonPlayerUpdater.HitPoint <= 0)
            {
                UpdateState = UpdateStateTerm;
                DrawState = DrawStateTerm;
                return;
            }
        }

        bool IsTimeToCreateEnemy()
        {
            if (++Frame >= 300)
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
            playerPos.Y += 11.0f;   // プレイヤーの頭上を見る
            cameraUpdater.Update(playerPos);
        }

        void UpdateLight()
        {
            if (lightUpdate)
                return;

            lightUpdate = true;

            foreach (var item in PointLights.Select((light, index) => new { light, index }))
            {
                item.light.Position = Player.Transform.Translation + PointLightPositions[item.index];
            }
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
            ChangeScene(new HomeScene());
        }

        void DrawStateTerm()
        {
        }
    }
}
