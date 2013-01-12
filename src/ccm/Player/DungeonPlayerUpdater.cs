using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using HimaLib.Camera;
using ccm.Input;

namespace ccm.Player
{
    public class DungeonPlayerUpdater : StateMachine, IPlayerUpdater
    {
        public ICamera Camera { get; set; }

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

        bool PressUp { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Up); } }
        bool PressDown { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Down); } }
        bool PressLeft { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Left); } }
        bool PressRight { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Right); } }
        bool PressWalk { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Walk); } }
        bool PressCrouch { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Crouch); } }
        bool PressJump { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Jump); } }
        bool PressAttack { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.MouseMain); } }

        bool IsMove { get { return PressUp || PressDown || PressLeft || PressRight; } }

        float VelocityRun { get { return 0.5f; } }
        float VelocityWalk { get { return 0.1f; } }
        float VelocityRotate { get { return 20.0f; } }

        float UpdateTimeScale { get { return TimeKeeper.Instance.LastTimeScale; } }

        IModel Model;
        AffineTransform Transform;

        float rotDegreeY;

        Vector3 position = new Vector3();

        float attackCount;

        public DungeonPlayerUpdater()
        {
            UpdateState = UpdateStateInit;
        }

        public void Update(IModel model, AffineTransform transform)
        {
            Model = model;
            Transform = transform;

            Update();

            Model.Update(TimeKeeper.Instance.LastFrameSeconds);
        }

        void UpdateStateInit()
        {
            GoToStand();
        }

        void UpdateStateStand()
        {
            if (PressAttack)
            {
                GoToAttack();
                return;
            }
            else if (PressCrouch)
            {
                GoToCrouch();
                return;
            }
            else if (IsMove)
            {
                if (PressWalk)
                {
                    GoToWalk();
                    return;
                }
                else
                {
                    GoToRun();
                    return;
                }
            }
            else
            {
            }
        }

        void UpdateStateRun()
        {
            if (PressAttack)
            {
                GoToAttack();
                return;
            }
            else if (PressCrouch)
            {
                GoToCrouch();
                return;
            }
            else if (IsMove)
            {
                if (PressWalk)
                {
                    GoToWalk();
                    return;
                }
            }
            else
            {
                GoToStand();
                return;
            }
            Move(VelocityRun);
        }

        void UpdateStateWalk()
        {
            if (PressAttack)
            {
                GoToAttack();
                return;
            }
            else if (PressCrouch)
            {
                GoToCrouch();
                return;
            }
            else if (IsMove)
            {
                if (!PressWalk)
                {
                    GoToRun();
                    return;
                }
            }
            else
            {
                GoToStand();
                return;
            }
            Move(VelocityWalk);
        }

        void Move(float velocity)
        {
            var rotMat = Matrix.CreateRotationY(MathUtil.ToRadians(GetMoveAngle()));
            var move = Vector3.Transform(CameraEyeVector, rotMat);

            MovePosition(move, velocity);
            MoveRotation(move);

            UpdateTransform();
        }

        float GetMoveAngle()
        {
            var moveAngle = 0.0f; // 視線方向を0とした移動方向の角度

            if (PressUp && PressLeft)
                moveAngle = 45.0f;
            else if (PressUp && PressRight)
                moveAngle = -45.0f;
            else if (PressDown && PressLeft)
                moveAngle = 135.0f;
            else if (PressDown && PressRight)
                moveAngle = -135.0f;
            else if (PressUp && !PressDown)
                moveAngle = 0.0f;
            else if (PressDown && !PressUp)
                moveAngle = 180.0f;
            else if (PressLeft && !PressRight)
                moveAngle = 90.0f;
            else if (PressRight && !PressLeft)
                moveAngle = -90.0f;

            return moveAngle;
        }

        void MovePosition(Vector3 move, float velocity)
        {
            position += move * velocity;
        }

        void MoveRotation(Vector3 move)
        {
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

        void UpdateTransform()
        {
            var rot = Transform.Rotation;
            rot.Y = MathUtil.ToRadians(rotDegreeY);
            Transform.Rotation = rot;
            Transform.Translation = position;
        }

        void UpdateStateCrouch()
        {
            if (PressAttack)
            {
                GoToAttack();
                return;
            }
            else if (!PressCrouch)
            {
                GoToStand();
            }
        }

        void UpdateStateAttack()
        {
            if (attackCount > 0.0f)
            {
                attackCount -= UpdateTimeScale;
            }
            else
            {
                GoToStand();
                return;
            }
        }

        void GoToStand()
        {
            UpdateState = UpdateStateStand;
            Model.ChangeMotion("stand", 0.2f);
        }

        void GoToRun()
        {
            UpdateState = UpdateStateRun;
            Model.ChangeMotion("run", 0.2f);
        }

        void GoToWalk()
        {
            UpdateState = UpdateStateWalk;
            Model.ChangeMotion("walk", 0.2f);
        }

        void GoToCrouch()
        {
            UpdateState = UpdateStateCrouch;
            Model.ChangeMotion("crouch", 0.2f);
        }

        void GoToAttack()
        {
            UpdateState = UpdateStateAttack;
            Model.ChangeMotion("attack1", 0.01f);
            attackCount = 20.0f;
        }
    }
}
