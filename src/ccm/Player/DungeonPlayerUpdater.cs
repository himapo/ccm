using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.System;
using HimaLib.Camera;
using HimaLib.Collision;
using HimaLib.Debug;
using ccm.Input;
using ccm.Debug;
using ccm.Collision;
using ccm.Sound;
using ccm.Battle;
using ccm.Deco;
using ccm.Map;

namespace ccm.Player
{
    public class DungeonPlayerUpdater : StateMachine, IPlayerUpdater
    {
        public CameraBase Camera { get; set; }

        public HimaLib.Collision.CollisionManager CollisionManager { get; set; }

        public Dungeon Dungeon { get; set; }

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
        bool PressWalk { get { return false; } }
        bool PressCrouch { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Crouch); } }
        bool PressJump { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.Jump); } }
        bool PushJump { get { return InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Jump); } }
        bool DoublePushJump { get { return PushJump && JumpCount > 0.0f; } }
        bool PressAttack { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.MouseMain); } }
        bool PushAttack { get { return InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.MouseMain); } }
        bool PressGuard { get { return InputAccessor.IsPress(ControllerLabel.Main, BooleanDeviceLabel.MouseSub); } }
        bool PushStep { get { return InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Step); } }

        bool IsMove { get { return PressUp || PressDown || PressLeft || PressRight; } }

        float VelocityRun { get { return 0.5f; } }
        float VelocityWalk { get { return 0.1f; } }
        float VelocityDash { get { return 0.9f; } }
        float VelocityRotate { get { return 20.0f; } }
        float VelocityStep { get { return 1.2f; } }

        float AccelFall { get { return -0.01f; } }

        float UpdateTimeScale { get { return TimeKeeper.Instance.LastTimeScale; } }

        IModel Model;

        AffineTransform Transform = new AffineTransform();

        AffineTransform PrevTransform = new AffineTransform();

        Vector3 Direction
        {
            get
            {
                var rotMat = Matrix.CreateRotationY(Transform.Rotation.Y);
                return Vector3.TransformAffine(Vector3.UnitZ, rotMat);
            }
        }

        public int HitPoint { get; set; }

        float AttackCount;

        float StepCount;

        float JumpCount;

        Vector3 StepDirection;

        float FallVelocity;

        PlayerBodyCollisionInfo BodyCollision;

        PlayerBodyCollisionInfo GroundCollision;

        PlayerDamageCollisionInfo DamageCollision;

        PlayerDamageCollisionInfo GuardCollision;

        PlayerAttackCollisionInfo AttackCollision;

        SoundManager SoundManager { get { return SoundManager.Instance; } }

        TimeKeeper TimeKeeper { get { return TimeKeeper.Instance; } }

        ComboCounter ComboCounter { get; set; }

        bool IsGround;

        // デコ
        public DecoManager DecoManager { get; set; }

        ccm.Deco.Deco ShockDeco;

        public DungeonPlayerUpdater()
        {
            BodyCollision = new PlayerBodyCollisionInfo()
            {
                Base = () => Transform.Translation,
                Reaction = (id, count) =>
                {
                    Transform.Translation = PrevTransform.Translation;
                },
            };

            GroundCollision = new PlayerBodyCollisionInfo()
            {
                Base = () => Transform.Translation,
                Group = () => (int)CollisionGroup.PlayerGround,
                Reaction = (id, count) =>
                {
                    IsGround = true;
                    //Transform.Translation.Y = 0;
                    //FallVelocity = 0.0f;
                    //GoToStand();
                },
                Color = Color.Yellow,
            };

            DamageCollision = new PlayerDamageCollisionInfo()
            {
                Center = () =>
                {
                    if (UpdateState == UpdateStateGuard)
                    {
                        return Transform.Translation + Vector3.UnitY * 1.5f - Direction * 1.5f;
                    }
                    else
                    {
                        return Transform.Translation + Vector3.UnitY * 1.5f;
                    }
                },
                Radius = () => (UpdateState == UpdateStateGuard) ? 2.0f : 3.0f,
                AttackReaction = Damage,
            };

            GuardCollision = new PlayerDamageCollisionInfo()
            {
                Active = () => (UpdateState == UpdateStateGuard),
                Center = () => Transform.Translation + Direction * 3.0f + Vector3.UnitY * 1.5f,
                Radius = () => 4.0f,
                AttackReaction = Guard,
            };

            AttackCollision = new PlayerAttackCollisionInfo()
            {
                Active = () => AttackCount > 0.0f,
                Center = () =>
                {
                    // 判定をネギの先端(Z方向)に4ずらす
                    var offset = new Vector3(0.0f, 0.0f, 4.0f);
                    return Vector3.TransformAffine(offset, Model.GetAttachmentMatrix("negi"));
                },
            };

            UpdateState = UpdateStateInit;
        }

        void Damage(int collisionId, int collisionCount, AttackCollisionActor actor)
        {
            if (HitPoint > 0 && collisionCount == 1)
            {
                HitPoint -= actor.Power;
                DebugPrint.PrintLine("Player damage {0}, HP {1}", actor.Power, HitPoint);
                ComboCounter.Damage(actor.Shock);
                if (ComboCounter.Shocked)
                {
                    GoToShocked();
                }
            }
        }

        void Guard(int collisionId, int collisionCount, AttackCollisionActor actor)
        {
            if (HitPoint > 0 && collisionCount == 1)
            {
                SoundManager.PlaySoundEffect("metal03");
                //DebugPrint.PrintLine("Player guard");
                ComboCounter.Guard(actor.Shock);
                if (ComboCounter.Shocked)
                {
                    GoToShocked();
                }
            }
        }

        void Update(IModel model, AffineTransform transform)
        {
            Model = model;
            PrevTransform = new AffineTransform(Transform);
            Transform = transform;

            Update();

            Model.Update(TimeKeeper.LastFrameSeconds);

            ComboCounter.Update(TimeKeeper.LastTimeScale);

            IsGround = false;
        }

        public void Update(Player player)
        {
            ComboCounter = player.ComboCounter;
            Update(player.Model, player.Transform);
        }

        void UpdateStateInit()
        {
            InitCollision();
            HitPoint = 3000;
            //GoToStand();
            Respawn();
        }

        void InitCollision()
        {
            CollisionManager.Add(BodyCollision);
            CollisionManager.Add(GroundCollision);
            CollisionManager.Add(DamageCollision);
            CollisionManager.Add(GuardCollision);
            CollisionManager.Add(AttackCollision);
        }

        void Respawn()
        {
            Transform.Translation = Dungeon.GetRandomRespawnPoint();
            FallVelocity = 0.0f;
            GoToFall();
        }

        void UpdateStateStand()
        {
            if (JumpCount > 0.0f)
            {
                JumpCount -= UpdateTimeScale;
                if (JumpCount < 0.0f)
                {
                    JumpCount = 0.0f;
                }
            }

            if (!IsGround)
            {
                GoToFall();
            }
            else if (DoublePushJump)
            {
                GoToDash();
                return;
            }
            else if (PushJump)
            {
                JumpCount = 20.0f;
            }
            
            if (PushAttack)
            {
                GoToAttack();
                return;
            }
            else if (PressGuard)
            {
                GoToGuard();
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
            if (JumpCount > 0.0f)
            {
                JumpCount -= UpdateTimeScale;
                if (JumpCount < 0.0f)
                {
                    JumpCount = 0.0f;
                }
            }

            if (!IsGround)
            {
                GoToFall();
            }
            else if (DoublePushJump)
            {
                GoToDash();
                return;
            }
            else if (PushJump)
            {
                JumpCount = 20.0f;
            }

            if (PushAttack)
            {
                GoToAttack();
                return;
            }
            else if (PressGuard)
            {
                GoToGuard();
                return;
            }
            else if (PushStep)
            {
                GoToStep();
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

        void UpdateStateDash()
        {
            if (!IsGround)
            {
                GoToFall();
            }
            else if (PushAttack)
            {
                GoToAttack();
                return;
            }
            else if (PressGuard)
            {
                GoToGuard();
                return;
            }
            else if (PushStep)
            {
                GoToStep();
                return;
            }
            else if (PressCrouch)
            {
                GoToCrouch();
                return;
            }
            else if (IsMove)
            {
                
            }
            else
            {
                GoToStand();
                return;
            }
            Move(VelocityDash);
        }

        void UpdateStateWalk()
        {
            if (!IsGround)
            {
                GoToFall();
            }
            else if (PushAttack)
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
            var move = GetMoveVector();

            MovePosition(move, velocity);
            MoveRotation(move);
        }

        Vector3 GetMoveVector()
        {
            var rotMat = Matrix.CreateRotationY(MathUtil.ToRadians(GetMoveAngle()));
            return Vector3.TransformAffine(CameraEyeVector, rotMat);
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
            Transform.Translation += move * velocity;
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

            var rotY = Transform.Rotation.Y;
            var rotDegreeY = MathUtil.ToDegrees(rotY);

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

            rotY += MathUtil.ToRadians(rotAngle);
            Transform.Rotation = Vector3.UnitY * rotY;
        }

        void UpdateStateCrouch()
        {
            if (PressAttack)
            {
                GoToAttack();
            }
            else if (!PressCrouch)
            {
                GoToStand();
            }
        }

        void UpdateStateAttack()
        {
            if (AttackCount > 0.0f)
            {
                if (AttackCount > 5.0f)
                {
                    if (PushStep)
                    {
                        AttackCount = 0.0f;
                        GoToStep();
                        return;
                    }
                }
                AttackCount -= UpdateTimeScale;
            }
            else
            {
                CollisionManager.ResetCollisionCount(AttackCollision.ID);

                if (PressAttack)
                {
                    GoToAttack2();
                }
                else
                {
                    GoToStand();
                }
            }
        }

        void UpdateStateAttack2()
        {
            if (AttackCount > 0.0f)
            {
                if (AttackCount > 5.0f)
                {
                    if (PushStep)
                    {
                        AttackCount = 0.0f;
                        GoToStep();
                        return;
                    }
                }
                AttackCount -= UpdateTimeScale;
            }
            else
            {
                CollisionManager.ResetCollisionCount(AttackCollision.ID);
                GoToStand();
            }
        }

        void UpdateStateGuard()
        {
            if(!PressGuard)
            {
                GoToStand();
            }
        }

        void UpdateStateStep()
        {
            if (StepCount > 0.0f)
            {
                MovePosition(StepDirection, VelocityStep);
                StepCount -= UpdateTimeScale;
            }
            else
            {
                GoToStand();
            }
        }

        void UpdateStateShocked()
        {
            if (!ComboCounter.Shocked)
            {
                DecoManager.Remove(ShockDeco);
                ShockDeco = null;
                GoToStand();
            }
        }

        void UpdateStateJump()
        {

        }

        void UpdateStateFall()
        {
            if (IsGround)
            {
                Transform.Translation.Y = -0.1f;
                FallVelocity = 0.0f;
                GoToStand();
            }

            Transform.Translation.Y += FallVelocity;

            if (FallVelocity > -2.0f)
            {
                FallVelocity += AccelFall;
            }

            if (Transform.Translation.Y < -200.0f)
            {
                Respawn();
            }
        }

        void GoToStand()
        {
            UpdateState = UpdateStateStand;
            Model.ChangeMotion("stand", 0.2f);
            JumpCount = 0.0f;
        }

        void GoToRun()
        {
            UpdateState = UpdateStateRun;
            Model.ChangeMotion("run", 0.2f);
            JumpCount = 0.0f;
        }

        void GoToWalk()
        {
            UpdateState = UpdateStateWalk;
            Model.ChangeMotion("walk", 0.2f);
            JumpCount = 0.0f;
        }

        void GoToCrouch()
        {
            UpdateState = UpdateStateCrouch;
            Model.ChangeMotion("crouch", 0.2f);
            JumpCount = 0.0f;
        }

        void GoToAttack()
        {
            UpdateState = UpdateStateAttack;
            Model.ChangeMotion("attack1", 0.01f);
            AttackCount = 38.0f;
            JumpCount = 0.0f;
            SoundManager.PlaySoundEffect("hit_s03_a");
        }

        void GoToAttack2()
        {
            UpdateState = UpdateStateAttack2;
            Model.ChangeMotion("attack2", 0.01f);
            AttackCount = 38.0f;
            JumpCount = 0.0f;
            SoundManager.PlaySoundEffect("hit_s02");
        }

        void GoToGuard()
        {
            UpdateState = UpdateStateGuard;
            Model.ChangeMotion("guard", 0.04f);
            JumpCount = 0.0f;
        }

        void GoToStep()
        {
            UpdateState = UpdateStateStep;
            Model.ChangeMotion("step", 0.01f);
            StepCount = 20.0f;
            StepDirection = GetMoveVector();
            JumpCount = 0.0f;
            //SoundManager.PlaySoundEffect("jump00");
        }

        void GoToShocked()
        {
            UpdateState = UpdateStateShocked;
            Model.ChangeMotion("stand", 0.2f);
            SoundManager.PlaySoundEffect("puu17");
            JumpCount = 0.0f;

            CreateShockDeco();
        }

        void GoToDash()
        {
            UpdateState = UpdateStateDash;
            Model.ChangeMotion("dash", 0.2f);
            JumpCount = 0.0f;
        }

        void GoToJump()
        {

        }

        void GoToFall()
        {
            UpdateState = UpdateStateFall;
            Model.ChangeMotion("stand", 0.2f);
            JumpCount = 0.0f;
        }

        void CreateShockDeco()
        {
            if (ShockDeco != null)
            {
                return;
            }
            var decoTransform = new AffineTransform(Transform);
            decoTransform.Translation.Y += 12.0f;
            ShockDeco = new Deco_Shock(decoTransform, Camera);
            DecoManager.Add(ShockDeco);
        }
    }
}
