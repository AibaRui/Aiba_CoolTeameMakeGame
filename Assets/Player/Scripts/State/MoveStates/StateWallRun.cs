using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallRun : PlayerStateBase
{
    public override void Enter()
    {
        //Swing�̃J�����̒l�̃��Z�b�g
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

        //�J������WallRun�p�ɕύX
        _stateMachine.PlayerController.CameraControl.UseWallRunCamera();

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();

        _stateMachine.PlayerController.Rb.useGravity = false;

        //WallRun��Animator��ݒ�
        _stateMachine.PlayerController.AnimControl.WallRunSet(true);

        Debug.Log("WAll");
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();

        //WallRun���オ�́ASwing��s�ɂ���
        _stateMachine.PlayerController.Swing.SetSwingFalse();

        _stateMachine.PlayerController.WallRun.SetNoMove(false);
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.WallRun.WallMove();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallRunCameraFollow();

        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.XOffSetControlWallRun();

        //Y����offset�̐ݒ�
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.SetOffsetY(true);
    }

    public override void Update()
    {
        _stateMachine.PlayerController.WallRun.CountNoMove();

        bool isHit = _stateMachine.PlayerController.WallRunCheck.CheckHitWall();

        float h = _stateMachine.PlayerController.InputManager.HorizontalInput;
        float v = _stateMachine.PlayerController.InputManager.VerticalInput;

        //�e����̃N�[���^�C��
        _stateMachine.PlayerController.CoolTimes();

        //�i���m�F
        _stateMachine.PlayerController.WallRunStep.CheckWallStep();

        //�i���o��Ɉڍs
        if(_stateMachine.PlayerController.WallRunStep.IsHitStep)
        {
            _stateMachine.TransitionTo(_stateMachine.WallRunStep);
            return;
        }

        if(_stateMachine.PlayerController.WallRunUpZip.CheckUpZipPosition()&& _stateMachine.PlayerController.InputManager.IsJumping)
        {
            _stateMachine.TransitionTo(_stateMachine.WallRunUpZipState);
            return;
        }

        if (h==0 && v<=0 && !_stateMachine.PlayerController.WallRun.IsEndNoMove)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
        }    //WallRun�ֈڍs

        if (_stateMachine.PlayerController.InputManager.IsJumping || !isHit)
        {
            //�d�͂��I��
            _stateMachine.PlayerController.Rb.useGravity = true;

            //WallRun��Animator��ݒ�
            _stateMachine.PlayerController.AnimControl.WallRunSet(false);
            
            //�W�����v����
            _stateMachine.PlayerController.WallRun.LastJump(true);

            //�ڍs
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);

        }    //WallRun�ֈڍs
    }
}