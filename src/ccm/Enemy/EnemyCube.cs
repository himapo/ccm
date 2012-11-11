using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class EnemyCube : MyGameComponent
    {
        public enum State
        {
            Init,
            Alive,
            Dead,
            Empty,
        }

        public Vector3 Position { get { return position; } set { position = value; } }
        private Vector3 position;

        public Vector3 prevPosition;

        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        private Vector3 rotation;

        public State NowState { get; private set; }

        public int HitPoint { get; set; }

        Action<GameTime> updateFunc;
        Action<GameTime> drawFunc;
        Model model;
        CubeRenderParameter renderParam;
        List<CubeShaderParameter> shaderParamList;

        float speed;
        float distance; // プレイヤーとの距離

        CollisionInfo damageCollision;
        SphereShapeParameter damageShape;
        EnemyDamageCollisionOpponent damageOpponent;
        float damageCount;  // ダメージ無敵時間

        // 存在コリジョン
        CollisionInfo bodyCollision;
        CylinderShapeParameter bodyShape;
        EnemyBodyCollisionOpponent bodyOpponent;

        public EnemyCube(Game game)
            : base(game)
        {
            HitPoint = 0;

            speed = GameProperty.gameRand.NextFloat() * 0.2f + 0.4f;
            distance = GameProperty.gameRand.NextFloat() * 40.0f + 10.0f;

            renderParam = new CubeRenderParameter();
            shaderParamList = new List<CubeShaderParameter>();

            damageCollision = new CollisionInfo();
            damageShape = new SphereShapeParameter();
            damageOpponent = new EnemyDamageCollisionOpponent();
            damageCount = 0.0f;

            bodyCollision = new CollisionInfo();
            bodyShape = new CylinderShapeParameter();
            bodyOpponent = new EnemyBodyCollisionOpponent();

            SetState(State.Empty);
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // マテリアルのシェーダと色を適当に設定
            for (var i = 0; i < 6; ++i)
            {
                var shaderParam = new CubeShaderParameter();

                shaderParam.ShaderType = CubeShaderType.Lambert;

                shaderParam.DiffuseColor = new Vector3(0.8f, 0.2f, 0.2f);
                shaderParam.Alpha = 1.0f;
                shaderParam.EmissiveColor = Vector3.Zero;
                shaderParam.SpecularColor = new Vector3(0.6f, 0.6f, 0.6f);
                shaderParam.SpecularPower = 4.0f;

                shaderParamList.Add(shaderParam);
            }

            var collisionService = GetService<ICollisionService>();

            // 食らいコリジョン情報設定
            damageCollision.ID = 0;
            damageCollision.Active = () =>
            {
                return NowState == State.Alive && damageCount <= 0.0f;
            };
            damageCollision.ShapeParameter = damageShape;
            damageCollision.Group = () => CollisionGroup.EnemyDamage;
            damageCollision.Reaction = (arg, opponent, count) =>
            {
                var playerAttack = opponent as PlayerAttackCollisionOpponent;
                if (playerAttack != null && count == 1)
                {
                    HitPoint -= playerAttack.Power();
                    if (HitPoint <= 0)
                    {
                        SetState(State.Dead);
                        GetService<IDecoService>().Add(
                            new DecoInfo
                            {
                                Type = DecoLabel.Prototype,
                                Position = this.Position,
                                ScriptClass = ""
                            }
                        );
                    }
                    else
                    {
                        damageCount = 30.0f;
                    }
                }
            };
            damageCollision.ReactionArg = () => null;
            damageCollision.ToOpponent = damageOpponent;

            damageShape.Center = () => Position;
            damageShape.Radius = () => 2.0f;

            collisionService.Add(damageCollision);

            // 存在コリジョン情報設定
            bodyCollision.ID = 0;
            bodyCollision.Active = () => { return NowState == State.Alive; };
            bodyCollision.ShapeParameter = bodyShape;
            bodyCollision.Group = () => CollisionGroup.EnemyBody;
            bodyCollision.Reaction = (arg, opponent, count) =>
            {
                Position = prevPosition;
            };
            bodyCollision.ReactionArg = () => null;
            bodyCollision.ToOpponent = bodyOpponent;

            bodyShape.Base = () =>
            {
                // TODO : コスト高い
                return new Vector3(Position.X, Position.Y - 2.0f, Position.Z);
            };
            bodyShape.Radius = () => 2.0f;
            bodyShape.Height = () => 4.0f;

            collisionService.Add(bodyCollision);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = ResourceManager.GetInstance().Load<Model>("Model/cube003");

            base.LoadContent();
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            updateFunc(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            drawFunc(gameTime);

            base.Draw(gameTime);
        }

        void SetState(State state)
        {
            if (state == State.Init)
            {
                updateFunc = UpdateInit;
                drawFunc = DrawInit;
            }
            else if (state == State.Alive)
            {
                updateFunc = UpdateAlive;
                drawFunc = DrawAlive;
            }
            else if (state == State.Dead)
            {
                updateFunc = UpdateDead;
                drawFunc = DrawDead;
            }
            else if (state == State.Empty)
            {
                updateFunc = UpdateEmpty;
                drawFunc = DrawEmpty;
            }

            NowState = state;
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            Disappear();
        }

        /// <summary>
        /// 出現
        /// </summary>
        public void Appear()
        {
            SetState(State.Init);
        }

        /// <summary>
        /// 消滅
        /// </summary>
        public void Disappear()
        {
            SetState(State.Empty);
        }

        /// <summary>
        /// ダメージ
        /// </summary>
        /// <param name="damage"></param>
        public void Damage(int damage)
        {
            HitPoint -= damage;
        }

        public void MoveUp()
        {
            new CylinderUpdater(
                 GetService<IUpdaterService>(),
                 2000.0f,
                 0.0f,
                 360.0f,
                 new Vector2(position.X, position.Z - 10.0f),
                 new Vector2(10.0f, 10.0f),
                 (a) => { position.X = a; },
                 (a) => { position.Z = a; },
                 position.Y,
                 position.Y + 10.0f,
                 (a) => { position.Y = a; },
                 MoveDown);
        }

        public void MoveDown()
        {
            new CylinderUpdater(
                GetService<IUpdaterService>(),
                2000.0f,
                0.0f,
                360.0f,
                new Vector2(position.X, position.Z - 10.0f),
                new Vector2(10.0f, 10.0f),
                (a) => { position.X = a; },
                (a) => { position.Z = a; },
                position.Y,
                position.Y - 10.0f,
                (a) => { position.Y = a; },
                MoveUp);
        }

        void UpdateInit(GameTime gameTime)
        {
            position.X = GameProperty.gameRand.NextFloat() * 200.0f - 100.0f;
            position.Y = 1.5f;
            position.Z = GameProperty.gameRand.NextFloat() * 200.0f - 100.0f;
            rotation = Vector3.Zero;

            HitPoint = 1;

            //MoveUp();

            InitializeRenderParameter();

            SetState(State.Alive);
        }

        void DrawInit(GameTime gameTime)
        {
        }

        void UpdateAlive(GameTime gameTime)
        {
            prevPosition = Position;

            if (HitPoint <= 0)
            {
                SetState(State.Dead);
            }
            if (damageCount > 0.0f)
            {
                damageCount -= GameProperty.GetUpdateScale(gameTime);
            }

            // プレイヤーに向かって移動
            var player = Player.GetInstance();
            var vecToPlayer = new Vector2(player.Position.X - Position.X, player.Position.Z - Position.Z);
            var distanceSquare = vecToPlayer.LengthSquared();

            if (distanceSquare > distance * distance)
            {
                vecToPlayer.Normalize();

                position.X += vecToPlayer.X * speed;
                position.Z += vecToPlayer.Y * speed;
            }
        }

        void DrawAlive(GameTime gameTime)
        {
            renderParam.world =
                Matrix.CreateScale(1.5f) *
                Matrix.CreateRotationX(rotation.X) *
                Matrix.CreateRotationY(rotation.Y) *
                Matrix.CreateRotationZ(rotation.Z) *
                Matrix.CreateTranslation(position);

            RenderManager.GetInstance().Register(renderParam);
        }

        void UpdateDead(GameTime gameTime)
        {
            SetState(State.Empty);
        }

        void DrawDead(GameTime gameTime)
        {
        }

        void UpdateEmpty(GameTime gameTime)
        {
        }

        void DrawEmpty(GameTime gameTime)
        {
        }

        void InitializeRenderParameter()
        {
            renderParam.cameraLabel = CameraLabel.Game;
            renderParam.model = model;
            renderParam.renderer = RendererLabel.Cube;
            renderParam.world = Matrix.Identity;
            renderParam.DirectionalLight0 = GetService<ILightService>().Get(LightAttribute.StageMain);
            renderParam.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
            renderParam.ShaderParamList = shaderParamList;
        }
    }
}
