using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Input;
using HimaLib.Math;
using ccm.Input;

namespace ccm.Camera
{
    public class MapViewerCameraUpdater
    {
        ICamera camera;

        IController controller;

        float rotX;
        
        float rotY;

        Vector3 pan;

        public MapViewerCameraUpdater(ICamera camera, IController controller)
        {
            this.camera = camera;
            this.controller = controller;

            Reset();
            UpdateCamera();
        }

        public void Update()
        {
            CheckInput();
            UpdateCamera();
        }

        void CheckInput()
        {
            var lookat = GetLookAtVector();
            var horizontal = GetHorizontalVector(lookat);
            var vertical = Vector3.Cross(lookat, horizontal);

            // rotate
            if (controller.IsPress((int)BooleanDeviceLabel.MouseSub))
            {
                rotX += 0.04f * controller.GetMoveY((int)PointingDeviceLabel.Mouse0);
                const float ROT_X_MAX = 0.0f;
                const float ROT_X_MIN = -MathUtil.PiOver2 * 0.99f;
                rotX = MathUtil.Clamp(rotX, ROT_X_MIN, ROT_X_MAX);
                rotY -= 0.04f * controller.GetMoveX((int)PointingDeviceLabel.Mouse0);
            }
            else if (controller.IsPress((int)BooleanDeviceLabel.MouseMiddle))
            {
                pan -= horizontal * (0.2f * controller.GetMoveX((int)PointingDeviceLabel.Mouse0));
                pan += vertical * (0.2f * controller.GetMoveY((int)PointingDeviceLabel.Mouse0));
            }
            else
            {
                pan += lookat * (0.2f * controller.GetDigitalDelta((int)DigitalDeviceLabel.MouseWheel0));
            }
        }

        void Reset()
        {
            rotX = -MathUtil.PiOver2 * 0.99f; ;
            rotY = 0.0f;
            pan = Vector3.Up * 3000.0f;
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

            camera.Eye = new Vector3(eye.X, eye.Y, eye.Z);
            camera.At = new Vector3(at.X, at.Y, at.Z);
            camera.Up = new Vector3(up.X, up.Y, up.Z);
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
