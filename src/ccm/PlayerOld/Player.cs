using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MikuMikuDance.Core;
using MikuMikuDance.Core.Model;
using MikuMikuDance.Core.Motion;
using MikuMikuDance.Core.Accessory;
using MikuMikuDance.XNA.Misc;
using MikuMikuDance.XNA.Accessory;
using MikuMikuDance.XNA;
using ccm.CameraOld;

namespace ccm.PlayerOld
{
    /// <summary>
    /// プレイヤークラス
    /// </summary>
    public class Player : MyGameComponent
    {
        static Player instance = null;

        enum State
        {
            Alive,
        }

        enum Pose
        {
            Stand,
            Walk,
            Run,
            Crouch,
            Attack1,
        }

        float UpdateTimeScale { get; set; }

        float VelocityRun { get { return 0.5f * UpdateTimeScale; } }
        float VelocityWalk { get { return 0.1f * UpdateTimeScale; } }
        float VelocityRotate { get { return 20.0f * UpdateTimeScale; } }

        //プロパティには演算できないのでフィールドを明示的に定義
        Vector3 position = new Vector3();
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        Vector3 prevPosition;

        Vector3 direction = new Vector3();
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        Matrix RotatMatrix
        {
            get
            {
                return Matrix.CreateRotationY(MathHelper.ToRadians(rotDegreeY));
            }
        }

        Matrix WorldMatrix
        {
            get
            {
                var scaleMat = Matrix.CreateScale(Vector3.One);
                var rotatMat = RotatMatrix;
                var transMat = Matrix.CreateTranslation(position);
                return scaleMat * rotatMat * transMat;
            }
        }

        /// <summary>
        /// カメラの視線方向（移動の基準）
        /// </summary>
        Vector3 CameraEyeVector
        {
            get
            {
                var camera = CameraManager.GetInstance().Get(CameraLabel.Game);
                var at = camera.At;
                at.Y = 0.0f;
                var eye = camera.Eye;
                eye.Y = 0.0f;
                var result = at - eye;
                result.Normalize();
                return result;
            }
        }

        State state;
        Action<GameTime> updateFunc;
        Action<GameTime> drawFunc;

        float rotDegreeY;

        Pose pose;

        PlayerModel model;
        PlayerModelChangeMotionContext modelChangeMotionContext;
        PlayerModelUpdateContextMMD modelUpdateContext;
        PlayerModelDrawContextMMD modelDrawContext;

        // 攻撃コリジョン
        CollisionInfo attackCollision;
        float attackCount; // 攻撃判定持続時間
        SphereShapeParameter attackShape;
        PlayerAttackCollisionOpponent attackOpponent;

        // 存在コリジョン
        CollisionInfo bodyCollision;
        CylinderShapeParameter bodyShape;
        PlayerBodyCollisionOpponent bodyOpponent;

        public static void CreateInstance(Game game)
        {
            instance = new Player(game);
        }

        public static Player GetInstance()
        {
            return instance;
        }

        Player(Game game) : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.PLAYER;
            DrawOrder = (int)DrawOrderLabel.PLAYER;

            rotDegreeY = 0.0f;

            model = PlayerModel.CreateInstance(PlayerModelType.MMD);
            modelChangeMotionContext = new PlayerModelChangeMotionContext();
            modelUpdateContext = new PlayerModelUpdateContextMMD();
            modelDrawContext = new PlayerModelDrawContextMMD();

            attackCollision = new CollisionInfo();
            attackCount = 0.0f;
            attackShape = new SphereShapeParameter();
            attackOpponent = new PlayerAttackCollisionOpponent();

            bodyCollision = new CollisionInfo();
            bodyShape = new CylinderShapeParameter();
            bodyOpponent = new PlayerBodyCollisionOpponent();
        }

        public override void Initialize()
        {
            // 攻撃コリジョン情報設定
            attackCollision.Active = () => attackCount > 0.0f;
            attackCollision.ShapeParameter = attackShape;
            attackCollision.Group = () => CollisionGroup.PlayerAttack;
            attackCollision.Reaction = (arg, opponent, count) => { };
            attackCollision.ReactionArg = () => null;
            attackCollision.ToOpponent = attackOpponent;

            attackShape.Center = () =>
            {
                return Position + Direction * 3.0f + Vector3.UnitY * 5.0f; // 前方1.5のところに判定
            };
            attackShape.Radius = () => 3.0f;

            attackOpponent.Power = () => 1;

            GetService<ICollisionService>().Add(attackCollision);

            // 存在コリジョン情報設定
            bodyCollision.Active = () => Enabled;
            bodyCollision.ShapeParameter = bodyShape;
            bodyCollision.Group = () => CollisionGroup.PlayerBody;
            bodyCollision.Reaction = (arg, opponent, count) =>
            {
                Position = prevPosition;
            };
            bodyCollision.ReactionArg = () => null;
            bodyCollision.ToOpponent = bodyOpponent;

            bodyShape.Base = () => Position;
            bodyShape.Radius = () => 3.0f;
            bodyShape.Height = () => 12.0f;

            GetService<ICollisionService>().Add(bodyCollision);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            model.Load(new PlayerModelLoadContextMMD()
            {
                Game = this.Game,
            });
        }

        protected override void Dispose(bool disposing)
        {
            model.Dispose();
        }

        void SetState(State state)
        {
            if (state == State.Alive)
            {
                updateFunc = UpdateAlive;
                drawFunc = DrawAlive;
            }

            this.state = state;
        }

        /// <summary>
        /// ゲームシーン開始時の状態にするメソッド
        /// </summary>
        void OnStartGame()
        {
            position.X = 0.0f;
            position.Y = 0.0f;
            position.Z = 0.0f;

            rotDegreeY = 0.0f;

            SetState(State.Alive);

            model.Reset();

            ChangeMotion("stand", 0.1f);
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.GAME_SCENE)
            {
                Enabled = true;
                Visible = true;
                OnStartGame();
            }
            else
            {
                Enabled = false;
                Visible = false;

                // コリジョンを消す
                attackCount = 0.0f;
            }
        }

        public override void OnSceneEnd(SceneLabel sceneLabel)
        {
            
        }

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

        void UpdateAlive(GameTime gameTime)
        {
            UpdateTimeScale = GameProperty.GetUpdateScale(gameTime);
          
            UpdateAttack();

            pose = Pose.Run;
            prevPosition = Position;

            UpdatePositionAndRotation();

            model.Transform = WorldMatrix;

            Direction = Vector3.Transform(Vector3.UnitZ, RotatMatrix);

            UpdateMotion();

            modelUpdateContext.GameTime = gameTime;
            model.Update(modelUpdateContext);


            // デバッグ出力
            var debugFont = DebugFontManager.GetInstance();
            var outputPos = new Vector2(900.0f, 0.0f);

            var camera = CameraManager.GetInstance().Get(CameraLabel.Game);
            var outputString = String.Format("camera pos ({0}, {1}, {2})", camera.Eye.X, camera.Eye.Y, camera.Eye.Z);
            debugFont.DrawString(new DebugFontInfo(outputString, outputPos));
            outputPos.Y += 22.0f;
            outputString = String.Format("player pos ({0}, {1}, {2})", position.X, position.Y, position.Z);
            debugFont.DrawString(new DebugFontInfo(outputString, outputPos));
            outputPos.Y += 22.0f;
            outputString = String.Format("rotation degree {0}", rotDegreeY);
            debugFont.DrawString(new DebugFontInfo(outputString, outputPos));
            outputPos.Y += 22.0f;
        }

        void UpdateAttack()
        {
            // 攻撃コリジョンの更新
            if (attackCollision.Active())
            {
                attackCount -= UpdateTimeScale;
            }

            // 攻撃
            if (InputManager.GetInstance().IsPush(InputLabel.MouseMain) 
                && !attackCollision.Active())
            {
                //var enemyService = GetService<IEnemyService>();
                //enemyService.DamageRange(1, position, 10.0f);

                // 攻撃コリジョンを有効にする
                attackCount = 20.0f;
                GetService<ICollisionService>().Reset(attackCollision.ID);
            }
        }

        void UpdatePositionAndRotation()
        {
            var input = InputManager.GetInstance();

            var up = input.IsPress(InputLabel.Up);
            var down = input.IsPress(InputLabel.Down);
            var left = input.IsPress(InputLabel.Left);
            var right = input.IsPress(InputLabel.Right);
            var walk = input.IsPress(InputLabel.Walk);
            var crouch = input.IsPress(InputLabel.Crouch);
            var jump = input.IsPress(InputLabel.Jump);

            if (attackCount > 0.0f)
            {
                pose = Pose.Attack1;
            }
            else
            {
                if (crouch)
                    pose = Pose.Crouch;
                else if (walk)
                    pose = Pose.Walk;

                var moveAngle = 0.0f; // 視線方向を0とした移動方向の角度

                var isMove = true;

                if (up && left)
                    moveAngle = 45.0f;
                else if (up && right)
                    moveAngle = -45.0f;
                else if (down && left)
                    moveAngle = 135.0f;
                else if (down && right)
                    moveAngle = -135.0f;
                else if (up && !down)
                    moveAngle = 0.0f;
                else if (down && !up)
                    moveAngle = 180.0f;
                else if (left && !right)
                    moveAngle = 90.0f;
                else if (right && !left)
                    moveAngle = -90.0f;
                else
                {
                    isMove = false;
                    if (!crouch)
                        pose = Pose.Stand;
                }

                // 方向入力が入っていればキャラを回転させる
                if (isMove)
                {
                    var rotMat = Matrix.CreateRotationY(MathHelper.ToRadians(moveAngle));
                    var move = Vector3.Transform(CameraEyeVector, rotMat);

                    if (pose == Pose.Walk)
                    {
                        position += move * VelocityWalk;
                    }
                    else if (pose == Pose.Run)
                    {
                        position += move * VelocityRun;
                    }

                    // 進行方向ベクトルとキャラの向きベクトルの角度差を求める
                    var eyeAngle = 0.0f;
                    if (Math.Abs(move.Z) < 0.001f)
                    {
                        if (move.X > 0.0f)
                            eyeAngle = 90.0f;
                        else
                            eyeAngle = -90.0f;
                    }
                    else
                    {
                        eyeAngle = MathHelper.ToDegrees((float)Math.Atan2(move.X, move.Z));
                    }

                    var rotDiff = eyeAngle - rotDegreeY;
                    if (rotDiff > 180.0f)
                        rotDiff -= 360.0f;
                    else if (rotDiff < -180.0f)
                        rotDiff += 360.0f;

                    var rotAngle = 0.0f;
                    if (rotDiff > 0.0f)
                    {
                        if (rotDiff < VelocityRotate)
                            rotAngle = rotDiff;
                        else
                            rotAngle = VelocityRotate;
                    }
                    else if (rotDiff < 0.0f)
                    {
                        if (rotDiff > -VelocityRotate)
                            rotAngle = rotDiff;
                        else
                            rotAngle = -VelocityRotate;
                    }

                    rotDegreeY += rotAngle;
                }

                while (rotDegreeY < -180.0f)
                    rotDegreeY += 360.0f;
                while (rotDegreeY > 180.0f)
                    rotDegreeY -= 360.0f;
            }
        }

        void UpdateMotion()
        {
            // モーション変更
            if (pose == Pose.Stand)
            {
                ChangeMotion("stand", 0.2f);
            }
            else if (pose == Pose.Crouch)
            {
                ChangeMotion("crouch", 0.2f);
            }
            else if (pose == Pose.Walk)
            {
                ChangeMotion("walk", 0.2f);
            }
            else if (pose == Pose.Run)
            {
                ChangeMotion("run", 0.2f);
            }
            else if (pose == Pose.Attack1)
            {
                ChangeMotion("attack1", 0.01f);
            }
        }

        void ChangeMotion(string motionName, float shiftTime)
        {
            modelChangeMotionContext.MotionName = motionName;
            modelChangeMotionContext.ShiftTime = shiftTime;
            model.ChangeMotion(modelChangeMotionContext);
        }

        void DrawAlive(GameTime gameTime)
        {
            modelDrawContext.GraphicsDevice = Game.GraphicsDevice;
            model.Draw(modelDrawContext);
        }
    }
}
