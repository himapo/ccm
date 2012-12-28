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

        float eyeZ;
        
        public float InitEyeZ; // カメラの注視点からの距離

        public float MaxEyeZ { get; set; }

        public float MinEyeZ { get; set; }

        public float EyeZInterval { get; set; }

        public float MaxRotX { get; set; }

        public float MinRotX { get; set; }

        public bool EnableCameraKey { get; set; }

        public ModelViewerCameraUpdater(ICamera camera, IController controller)
        {
            this.camera = camera;
            this.controller = controller;

            InitEyeZ = 30.0f;
            MaxEyeZ = 110.0f;
            MinEyeZ = 10.0f;
            EyeZInterval = 0.1f;

            MaxRotX = 0.0f;
            MinRotX = -MathUtil.PiOver2 * 0.99f;

            EnableCameraKey = false;

            Reset();
        }

        public void Update(Vector3 at)
        {
            CheckInput();
            UpdateCamera(at);
        }

        void CheckInput()
        {
            if (EnableCameraKey && !controller.IsPress((int)BooleanDeviceLabel.Camera))
            {
                return;
            }

            // rotate
            if (controller.IsPress((int)BooleanDeviceLabel.MouseSub))
            {
                rotX += 0.04f * controller.GetMoveY((int)PointingDeviceLabel.Mouse0);
                
                rotX = MathUtil.Clamp(rotX, MinRotX, MaxRotX);
                rotY -= 0.04f * controller.GetMoveX((int)PointingDeviceLabel.Mouse0);
            }

            // zoom
            eyeZ -= EyeZInterval * controller.GetDigitalDelta((int)DigitalDeviceLabel.MouseWheel0);
            eyeZ = MathUtil.Clamp(eyeZ, MinEyeZ, MaxEyeZ);

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
            eyeZ = InitEyeZ;
        }

        void UpdateCamera(Vector3 atPosition)
        {
            // カメラ初期パラメータ
            var INIT_EYE = new Vector4(0.0f, 0.0f, eyeZ, 1.0f);
            var INIT_AT = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var INIT_UP = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

            var mat = Matrix.Identity;

            var rot = Matrix.CreateRotationX(rotX);
            mat *= rot;
            rot = Matrix.CreateRotationY(rotY);
            mat *= rot;

            // 注視点
            var trans = Matrix.CreateTranslation(atPosition);
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
