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

        ViewerCameraUpdater cameraUpdater;

        // ライト
        DirectionalLight DirectionalLight0 = new DirectionalLight();

        PointLight PointLight0 = new PointLight();

        List<PointLight> PointLights = new List<PointLight>();

        // デコ
        ccm.Deco.DecoManager DecoManager = new Deco.DecoManager();

        // サウンド
        SoundManager SoundManager { get { return SoundManager.Instance; } }

        // HUD

        // デバッグメニュー
        DebugMenu debugMenu = new DebugMenu("DungeonMenu");

        DebugMenuUpdater debugMenuUpdater;

        DefaultDebugMenuDrawer debugMenuDrawer = new DefaultDebugMenuDrawer();

        // その他
        //int Floor = 1;

        int Frame = 0;

        IBillboard Billboard = BillboardFactory.Instance.Create();

        HudBillboardRenderParameter TargetRenderParam = new HudBillboardRenderParameter();

        bool ShowRenderTarget{ get; set; }

        IModel StageModel;

        SimpleModelRenderParameter StageRenderParam = new SimpleModelRenderParameter();

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

            DungeonDrawer.Camera = Camera;

            cameraUpdater = new ViewerCameraUpdater(Camera, InputAccessor.GetController(ControllerLabel.Main))
            {
                InitRotX = -MathUtil.PiOver4,
                InitRotY = MathUtil.PiOver4,
                InitEyeZ = 60.0f,
                MaxEyeZ = 110.0f,
                MinEyeZ = 40.0f,
                EyeZInterval = 0.2f,
                RotInterval = 0.04f,
                PanInterval = 0.2f,
                MaxRotX = 0.0f,
                EnableCameraKey = true,
                EnablePan = false,
            };

            debugMenuUpdater = new DebugMenuUpdater(debugMenu);
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
            InitShadowMapHud();
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
            for (var i = 0; i < 20; ++i)
            {
                EnemyManager.CreateEnemy(EnemyType.Cube, CalcEnemyAppearPosition());
            }
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
            CollisionManager.AddGroupPair((int)Collision.CollisionGroup.EnemyAttack, (int)Collision.CollisionGroup.PlayerDamage);
            CollisionManager.Drawer = new WireCollisionDrawer(Camera);
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
        }

        void InitLight()
        {
            RenderSceneManager.Instance.ClearDirectionalLight();

            DirectionalLight0.Direction = -Vector3.One;
            DirectionalLight0.Color = new Color(0.5f, 0.5f, 0.5f);
            RenderSceneManager.Instance.AddDirectionalLight(DirectionalLight0);

            RenderSceneManager.Instance.ClearPointLight();

            PointLight0.Position = new Vector3(0.0f, 20.0f, 0.0f);
            PointLight0.Color = Color.White;
            PointLight0.AttenuationBegin = 2.0f;
            PointLight0.AttenuationEnd = 30.0f;
            //RenderSceneManager.Instance.AddPointLight(PointLight0);

            PointLights.Add(PointLight0);

            for (var i = 0; i < 7; ++i)
            {
                var light = new PointLight()
                {
                    Position = new Vector3(Rand.NextFloat(-50.0f, 50.0f), Rand.NextFloat(5.0f, 30.0f), Rand.NextFloat(-50.0f, 50.0f)),
                    Color = new Color(Rand.Next(128, 256), Rand.Next(128, 256), Rand.Next(128, 256)),
                    AttenuationBegin = Rand.NextFloat(0.0f, 10.0f),
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

            var nodeShowTarget = new DebugMenuNodeSelectable()
            {
                Label = "レンダーターゲット表示",
            };

            nodeShowTarget.AddChoice("なし", () =>
            {
                ShowRenderTarget = false;
            });

            nodeShowTarget.AddChoice("シャドウマップ", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
            });

            nodeShowTarget.AddChoice("拡散反射マップ", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.DiffuseLightMap);
            });
            
            nodeShowTarget.AddChoice("鏡面反射マップ", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.SpecularLightMap);
            });
            
            nodeShowTarget.AddChoice("Gバッファ0", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer0);
            });
            
            nodeShowTarget.AddChoice("Gバッファ1", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer1);
            });
            /*
            nodeShowTarget.AddChoice("Gバッファ2", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer2);
            });

            nodeShowTarget.AddChoice("Gバッファ3", () =>
            {
                ShowRenderTarget = true;
                TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.GBuffer3);
            });
            */
            debugMenu.AddChild(debugMenu.RootNode.Label, nodeShowTarget);
        }

        void InitRender()
        {
        }

        void InitShadowMapHud()
        {
            TargetRenderParam.Texture = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
            TargetRenderParam.Alpha = 1.0f;
            TargetRenderParam.Transform = new AffineTransform(
                Vector3.One * 0.4f,
                Vector3.Zero,
                new Vector3(640.0f - 256.0f - 10.0f, 360.0f - 144.0f - 10.0f, 0.0f));
            TargetRenderParam.IsTranslucent = false;
        }

        void InitStage()
        {
            StageModel = ModelFactory.Instance.Create("cube000");

            StageRenderParam.Alpha = 1.0f;
            StageRenderParam.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
            StageRenderParam.IsShadowCaster = false;
            StageRenderParam.IsShadowReceiver = true;
            StageRenderParam.Transform = new AffineTransform(
                Vector3.One * 30.0f,
                Vector3.Zero,
                Vector3.UnitY * (-45.0f)).WorldMatrix;
            StageRenderParam.Camera = Camera;
            StageRenderParam.ShadowMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
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

            RenderSceneManager.Instance.RenderModel(StageModel, StageRenderParam);

            if(ShowRenderTarget)
            {
                RenderSceneManager.Instance.RenderBillboard(Billboard, TargetRenderParam);
            }

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
