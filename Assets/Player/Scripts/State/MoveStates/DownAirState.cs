using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DownAirState : PlayerStateBase
{
    public override void Enter()
    {
        //�J�������������ɂ���
        _stateMachine.PlayerController.CameraControl.UseSwingCamera();

        //���x�ݒ�
        _stateMachine.PlayerController.VelocityLimit.SetLimit(25, 40, -25, 25);
    }

    public override void Exit()
    {
        //�R���g���[���[�̐U��
        _stateMachine.PlayerController.ControllerVibrationManager.StopVibration();
        _stateMachine.PlayerController.Move.ReSetTime();
        _stateMachine.PlayerController.CameraControl.EndSwingCamera();
    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            _stateMachine.PlayerController.Move.AirMove();

        //���x�̌���
        _stateMachine.PlayerController.VelocityLimit.SlowToSpeedUp();
    }

    public override void LateUpdate()
    {
        //�J�����̎���
        _stateMachine.PlayerController.CameraControl.CountTime();

        //�X�e�[�^�X�ݒ�
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.CheckStatas();

        //�J�����̌X����߂�
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetAirCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetAirCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetAirCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);

        //�J������X����Offset��߂�
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetOffSetX();

        //FOV�ݒ�
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(false, _stateMachine.PlayerController.Rb.velocity.y);

        //�J�������v���C���[�̌��Ɏ����I�Ɍ�����BX��
        _stateMachine.PlayerController.CameraControl.SwingEndCameraAutoFollow();

        //�J�������X����BX��
        _stateMachine.PlayerController.CameraControl.SwingCameraValueX(false);
    }

    public override void Update()
    {
        //�e����̃N�[���^�C��
        _stateMachine.PlayerController.CoolTimes();

        _stateMachine.PlayerController.Move.DownSpeedOfSppedDash();



        if (_stateMachine.PlayerController.InputManager.IsAttack)
        {
            if (_stateMachine.PlayerController.Attack.IsCanAttack)
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
            }
        }   //�U���X�e�[�g

        //Zip
        if (_stateMachine.PlayerController.InputManager.IsJumping && _stateMachine.PlayerController.ZipMove.IsCanZip)
        {
            //���̉��̐ݒ�
            _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(false);

            _stateMachine.TransitionTo(_stateMachine.StateZip);
            return;
        }

        if (_stateMachine.PlayerController.SearchSwingPoint.Search())
        {
            if (_stateMachine.PlayerController.Swing.IsCanSwing &&
                _stateMachine.PlayerController.InputManager.IsSwing == 1)
            {
                _stateMachine.TransitionTo(_stateMachine.StateSwing);
                return;
            }
        }

        if (_stateMachine.PlayerController.GroundCheck.IsHit())
        {
            //Swing�̃J�����̒l�̃��Z�b�g
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            //���n��
            _stateMachine.PlayerController.PlayerAudioManager.GroundAudio.LandAudioPlay();
            //���̉��̐ݒ�
            _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(false);

            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }   //�n��

        //  if (_stateMachine.PlayerController.InputManager.IsSetUp > 0)
        // {
        //      _stateMachine.TransitionTo(_stateMachine.StateGrappleSetUp);
        // }   //�\��

        //if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        //{
        //    _stateMachine.PlayerController.Avoid.SetAvoidDir();
        //    _stateMachine.TransitionTo(_stateMachine.AvoidState);
        //}   //���

        //�ǂ�����������AWallRun��Ԃ�
        //if (_stateMachine.PlayerController.WallRunCheck.CheckWall())
        //{
        //    if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
        //    {
        //        _stateMachine.TransitionTo(_stateMachine.StateWallRun);
        //    }    //WallRun�ֈڍs
        //    else
        //    {
        //        _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
        //    } �@//WallIdle�ֈڍs
        //}


    }
}