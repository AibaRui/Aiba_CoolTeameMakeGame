using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallIdle : PlayerStateBase
{
    public override void Enter()
    {
        //�J������WallRun�p�ɕύX
        _stateMachine.PlayerController.CameraControl.UseWallRunCamera();

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.Rb.velocity = Vector3.zero;
        _stateMachine.PlayerController.Rb.useGravity = false;
 
       // _stateMachine.PlayerController.WallRun.SetMoveDir(WallRun.MoveDirection.Up);

        //WallRun��Animator��ݒ�
        _stateMachine.PlayerController.AnimControl.WallRunSet(true);
    }

    public override void Exit()
    {
        //�~�܂��Ă���Ƃ��ɁA�J�����̒l��Idle��Ԃ̒l�ɕύX����܂ł̎��Ԃ̌v���B�̒l�����Z�b�g
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.ResetCountReSetWallIdleCameraTime();

        _stateMachine.PlayerController.Move.ReSetTime();

        //WallRun���オ�́ASwing��s�ɂ���
        _stateMachine.PlayerController.Swing.SetSwingFalse();

        //FrontZip�����s�\�ɂ���
        _stateMachine.PlayerController.ZipMove.SetCanZip();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.WallRun.AddWall();
    }

    public override void LateUpdate()
    {
        //�~�܂��Ă���Ƃ��ɁA�J�����̒l��Idle��Ԃ̒l�ɕύX����
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallIdleCamera();

        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.XOffSetWallIdle();

        //Y����offset�̐ݒ�
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.SetOffsetY(false);
    }

    public override void Update()
    {
        //�~�܂��Ă���Ƃ��ɁA�J�����̒l��Idle��Ԃ̒l�ɕύX����B�܂ł̎��Ԃ��v��
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.CountReSetWallIdleCameraTime();


        float h = _stateMachine.PlayerController.InputManager.HorizontalInput;
        float v = _stateMachine.PlayerController.InputManager.VerticalInput;

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.WallRun.MidleDir();

        //�e����̃N�[���^�C��
        _stateMachine.PlayerController.CoolTimes();

        if (h!=0 || v>0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallRun);
        }    //WallRun�ֈڍs

        if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            //�d�͂��I��
            _stateMachine.PlayerController.Rb.useGravity = true;

            //WallRun��Animator��ݒ�
            _stateMachine.PlayerController.AnimControl.WallRunSet(false);

            //�W�����v����
            _stateMachine.PlayerController.WallRun.LastJump(false);

            //WallRun�ֈڍs
            _stateMachine.TransitionTo(_stateMachine.StateJump);

        }

    }
}