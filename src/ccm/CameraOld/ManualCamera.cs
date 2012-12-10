using Microsoft.Xna.Framework;

namespace ccm.CameraOld
{
    class ManualCamera : CameraBase
    {
        float rotX;
        float rotY;
        float fovY;
        float initEyeZ; // カメラの注視点からの距離

        public ManualCamera(Game game)
            : base(game)
        {
            rotX = 0.0f;
            rotY = 0.0f;
            fovY = 0.0f;
            initEyeZ = 0.0f;
        }

        public override void Initialize()
        {
            ResetCamera();

            UpdateCamera();
        }

        public override void Update(GameTime gameTime)
        {
            var inputService = InputManager.GetInstance();

            if (inputService.IsPress(InputLabel.Camera))
            {
                if (inputService.IsPress(InputLabel.MouseSub))
                {
                    rotX += 0.04f * inputService.MouseMoveY;
                    const float ROT_X_MAX = 0.0f;
                    const float ROT_X_MIN = -MathHelper.PiOver2 * 0.99f;
                    rotX = MathHelper.Clamp(rotX, ROT_X_MIN, ROT_X_MAX);
                    rotY -= 0.04f * inputService.MouseMoveX;
                }
                initEyeZ -= 0.2f * inputService.MouseMoveWheel;
                initEyeZ = MathHelper.Clamp(initEyeZ, 40.0f, 110.0f);
            }

            UpdateCamera();

            base.Update(gameTime);
        }

        void UpdateCamera()
        {
            // カメラ初期パラメータ
            var INIT_EYE = new Vector4(0.0f, 0.0f, initEyeZ, 1.0f);
            var INIT_AT = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var INIT_UP = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

            var mat = Matrix.Identity;

            var rot = Matrix.CreateRotationX(rotX);
            mat *= rot;
            rot = Matrix.CreateRotationY(rotY);
            mat *= rot;

            // プレイヤーに追従
            var playerPos = PlayerOld.Player.GetInstance().Position;
            var playerTrans = Matrix.CreateTranslation(playerPos.X + 0.0f, playerPos.Y + 6.0f, playerPos.Z + 0.0f);
            mat *= playerTrans;

            var eye = Vector4.Transform(INIT_EYE, mat);
            var at = Vector4.Transform(INIT_AT, mat);
            var up = Vector4.Transform(INIT_UP, mat);

            Camera.Eye = new Vector3(eye.X, eye.Y, eye.Z);
            Camera.At = new Vector3(at.X, at.Y, at.Z);
            Camera.Up = new Vector3(up.X, up.Y, up.Z);

            Camera.Update();
        }

        void ResetCamera()
        {
            rotX = -MathHelper.PiOver4;
            rotY = MathHelper.PiOver4;

            fovY = GameProperty.fov;
            float aspect = (float)GameProperty.resolutionWidth / (float)GameProperty.resolutionHeight;
            float near = 10.0f;
            float far = 400.0f;

            initEyeZ = 60.0f;

            Camera.FovY = MathHelper.ToRadians(fovY);
            Camera.Aspect = aspect;
            Camera.Near = near;
            Camera.Far = far;
        }
    }
}
