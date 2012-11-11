using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class FreeLookCamera : CameraBase
    {
        float rotX;
        float rotY;
        Vector3 pan;

        public FreeLookCamera(Game game)
            : base(game)
        {
            rotX = 0.0f;
            rotY = 0.0f;
            pan = Vector3.Zero;
        }

        public override void Initialize()
        {
            ResetCamera();

            UpdateCamera();
        }

        public override void Update(GameTime gameTime)
        {
            var inputService = InputManager.GetInstance();

            var lookat = GetLookAtVector();
            var horizontal = GetHorizontalVector(lookat);
            var vertical = Vector3.Cross(lookat, horizontal);

            if (inputService.IsPress(InputLabel.MouseSub))
            {
                rotX += 0.01f * inputService.MouseMoveY;
                const float ROT_X_MAX = MathHelper.PiOver2 * 0.99f;
                const float ROT_X_MIN = -MathHelper.PiOver2 * 0.99f;
                rotX = MathHelper.Clamp(rotX, ROT_X_MIN, ROT_X_MAX);
                rotY -= 0.01f * inputService.MouseMoveX;
            }
            else if (inputService.IsPress(InputLabel.MouseMiddle))
            {
                pan -= horizontal * (0.2f * inputService.MouseMoveX);
                pan += vertical * (0.2f * inputService.MouseMoveY);
            }
            else
            {
                pan += lookat * (0.2f * inputService.MouseMoveWheel);
            }

            UpdateCamera();
        }

        void UpdateCamera()
        {
            // カメラ初期パラメータ
            var INIT_EYE = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            var INIT_AT = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var INIT_UP = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

            var mat = Matrix.Identity;
            mat *= Matrix.CreateRotationX(rotX);
            mat *= Matrix.CreateRotationY(rotY);
            mat *= Matrix.CreateTranslation(pan);

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
            rotX = -MathHelper.PiOver2 * 0.99f;
            rotY = 0;

            float aspect = (float)GameProperty.resolutionWidth / (float)GameProperty.resolutionHeight;
            float near = 10.0f;
            float far = 10000.0f;

            pan = Vector3.Up * 3000.0f;

            Camera.FovY = MathHelper.ToRadians(GameProperty.fov);
            Camera.Aspect = aspect;
            Camera.Near = near;
            Camera.Far = far;
        }

        Vector3 GetLookAtVector()
        {
            var INIT_EYE = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            var INIT_AT = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

            var mat = Matrix.Identity;

            mat *= Matrix.CreateRotationX(rotX);
            mat *= Matrix.CreateRotationY(rotY);

            var eye = Vector4.Transform(INIT_EYE, mat);
            var at = Vector4.Transform(INIT_AT, mat);

            var lookat = at - eye;

            return new Vector3(lookat.X, lookat.Y, lookat.Z);
        }

        Vector3 GetHorizontalVector(Vector3 v)
        {
            return Vector3.Cross(v, Vector3.Up);
        }
    }
}
