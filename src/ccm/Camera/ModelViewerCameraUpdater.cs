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
    public class ModelViewerCameraUpdater
    {
        ICamera camera;

        IController controller;

        float rotX;
        
        float rotY;
        
        float initEyeZ; // カメラの注視点からの距離

        public ModelViewerCameraUpdater(ICamera camera, IController controller)
        {
            this.camera = camera;
            this.controller = controller;

            Reset();
        }

        public void Update()
        {
            CheckInput();
            UpdateCamera();
        }

        void CheckInput()
        {
            // rotate
            if (controller.IsPress((int)BooleanDeviceLabel.MouseSub))
            {
                rotX += 0.04f * controller.GetMoveY((int)PointingDeviceLabel.Mouse0);
                const float ROT_X_MAX = 0.0f;
                const float ROT_X_MIN = -MathUtil.PiOver2 * 0.99f;
                rotX = MathUtil.Clamp(rotX, ROT_X_MIN, ROT_X_MAX);
                rotY -= 0.04f * controller.GetMoveX((int)PointingDeviceLabel.Mouse0);
            }

            // zoom
            initEyeZ -= 0.1f * controller.GetDigitalDelta((int)DigitalDeviceLabel.MouseWheel0);
            initEyeZ = MathUtil.Clamp(initEyeZ, 10.0f, 110.0f);

            // reset
            if (controller.IsPush((int)BooleanDeviceLabel.MouseMiddle))
            {
                Reset();
            }
        }

        void Reset()
        {
            rotX = 0.0f;
            rotY = 0.0f;
            initEyeZ = 30.0f;
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

            // 注視点
            var trans = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            mat *= trans;

            var eye = Vector4.Transform(INIT_EYE, mat);
            var at = Vector4.Transform(INIT_AT, mat);
            var up = Vector4.Transform(INIT_UP, mat);

            camera.Eye = new Vector3(eye.X, eye.Y, eye.Z);
            camera.At = new Vector3(at.X, at.Y, at.Z);
            camera.Up = new Vector3(up.X, up.Y, up.Z);
        }
    }
}
