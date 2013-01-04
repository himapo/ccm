using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using HimaLib.Input;
using HimaLib.Camera;
using ccm.Input;

namespace ccm.Player
{
    public class Player
    {
        enum Pose
        {
            Stand,
            Walk,
            Run,
            Crouch,
            Attack1,
        }

        Pose pose;

        string modelName = "petit_miku_mix2";

        IModel model;

        AffineTransform transform = new AffineTransform();
        public AffineTransform Transform { get { return transform; } }

        float attackCount;

        public ICamera Camera { get; set; }

        Vector3 position = new Vector3();
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        Vector3 CameraEyeVector
        {
            get
            {
                var at = Camera.At;
                at.Y = 0.0f;
                var eye = Camera.Eye;
                eye.Y = 0.0f;
                var result = new Vector3(at.X - eye.X, at.Y - eye.Y, at.Z - eye.Z);
                result.Normalize();
                return result;
            }
        }

        float VelocityRun { get { return 0.5f; } }
        float VelocityWalk { get { return 0.1f; } }
        float VelocityRotate { get { return 20.0f; } }

        float rotDegreeY;

        public Player()
        {
        }

        public void InitModel()
        {
            model = ModelFactory.Instance.Create(modelName);
            model.ChangeMotion("stand", 0.01f);
        }

        public void AddAttachment(string attachmentName)
        {
            model.AddAttachment(attachmentName);
        }

        public void RemoveAttackment(string attachmentName)
        {
            model.RemoveAttachment(attachmentName);
        }

        public void Update()
        {
            UpdateCollision();
            UpdatePosition();
            UpdateMotion();
        }

        void UpdateCollision()
        {
        }

        void UpdatePosition()
        {
            pose = Pose.Run;

            var up = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Up);
            var down = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Down);
            var left = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Left);
            var right = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Right);
            var walk = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Walk);
            var crouch = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Crouch);
            var jump = InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Jump);

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
                    var rotMat = Matrix.CreateRotationY(MathUtil.ToRadians(moveAngle));
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
                        eyeAngle = MathUtil.ToDegrees((float)Math.Atan2(move.X, move.Z));
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
            var rot = transform.Rotation;
            rot.Y = MathUtil.ToRadians(rotDegreeY);
            transform.Rotation = rot;
            transform.Translation = position;
        }

        void UpdateMotion()
        {
            if (pose == Pose.Stand)
            {
                model.ChangeMotion("stand", 0.2f);
            }
            else if (pose == Pose.Crouch)
            {
                model.ChangeMotion("crouch", 0.2f);
            }
            else if (pose == Pose.Walk)
            {
                model.ChangeMotion("walk", 0.2f);
            }
            else if (pose == Pose.Run)
            {
                model.ChangeMotion("run", 0.2f);
            }
            else if (pose == Pose.Attack1)
            {
                model.ChangeMotion("attack1", 0.01f);
            }

            model.Update(TimeKeeper.Instance.LastFrameSeconds);
        }

        public void Draw(IPlayerDrawer drawer)
        {
            drawer.Draw(model, transform);
        }
    }
}
