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
    public class ViewerCameraUpdater
    {
        ICamera camera;

        IController controller;

        float rotX;
        
        float rotY;

        float eyeZ;

        Vector3 pan;

        public float InitRotX { get; set; }

        public float InitRotY { get; set; }

        public Vector3 InitPan { get; set; }

        public float InitEyeZ; // カメラの注視点からの距離

        public float MaxEyeZ { get; set; }

        public float MinEyeZ { get; set; }

        public float EyeZInterval { get; set; }

        public float RotInterval { get; set; }

        public float PanInterval { get; set; }

        public float MaxRotX { get; set; }

        public float MinRotX { get; set; }

        public bool EnableCameraKey { get; set; }

        public bool EnablePan { get; set; }

        public ViewerCameraUpdater(ICamera camera, IController controller)
        {
            this.camera = camera;
            this.controller = controller;

            InitEyeZ = 30.0f;
            MaxEyeZ = 110.0f;
            MinEyeZ = 10.0f;
            EyeZInterval = 0.1f;

            RotInterval = 0.04f;
            PanInterval = 0.2f;

            MaxRotX = MathUtil.PiOver2 * 0.99f;
            MinRotX = -MathUtil.PiOver2 * 0.99f;

            EnableCameraKey = false;
            EnablePan = false;

            Reset();
        }

        public void Update(Vector3 at)
        {
            CheckInput(at);
            UpdateCamera(at);
        }

        void CheckInput(Vector3 at)
        {
            if (EnableCameraKey && !controller.IsPress((int)BooleanDeviceLabel.Camera))
            {
                return;
            }

            var lookat = GetLookAtVector(at);
            var horizontal = GetHorizontalVector(lookat);
            var vertical = Vector3.Cross(lookat, horizontal);

            // rotate
            if (controller.IsPress((int)BooleanDeviceLabel.MouseSub))
            {
                rotX += RotInterval * controller.GetMoveY((int)PointingDeviceLabel.Mouse0);
                rotX = MathUtil.Clamp(rotX, MinRotX, MaxRotX);
                rotY -= RotInterval * controller.GetMoveX((int)PointingDeviceLabel.Mouse0);
            }
            else if (EnablePan && controller.IsPress((int)BooleanDeviceLabel.MouseMiddle))
            {
                pan -= horizontal * (PanInterval * controller.GetMoveX((int)PointingDeviceLabel.Mouse0));
                pan += vertical * (PanInterval * controller.GetMoveY((int)PointingDeviceLabel.Mouse0));
            }
            else
            {
                eyeZ -= EyeZInterval * controller.GetDigitalDelta((int)DigitalDeviceLabel.MouseWheel0);
                eyeZ = MathUtil.Clamp(eyeZ, MinEyeZ, MaxEyeZ);
            }

            if (controller.IsPress((int)BooleanDeviceLabel.MouseMain) && controller.IsPress((int)BooleanDeviceLabel.MouseSub))
            {
                Reset();
            }
        }

        public void Reset()
        {
            rotX = InitRotX;
            rotY = InitRotY;
            eyeZ = InitEyeZ;
            pan = InitPan;
        }

        void UpdateCamera(Vector3 atPosition)
        {
            // カメラ初期パラメータ
            var INIT_EYE = new Vector4(0.0f, 0.0f, eyeZ, 1.0f);
            var INIT_AT = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var INIT_UP = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

            var mat = Matrix.Identity;
            mat *= Matrix.CreateRotationX(rotX);
            mat *= Matrix.CreateRotationY(rotY);
            mat *= Matrix.CreateTranslation(atPosition + pan);

            var eye = Vector4.Transform(INIT_EYE, mat);
            var at = Vector4.Transform(INIT_AT, mat);
            var up = Vector4.Transform(INIT_UP, mat);

            camera.Eye = new Vector3(eye.X, eye.Y, eye.Z);
            camera.At = new Vector3(at.X, at.Y, at.Z);
            camera.Up = new Vector3(up.X, up.Y, up.Z);
        }

        Vector3 GetLookAtVector(Vector3 atPosition)
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
