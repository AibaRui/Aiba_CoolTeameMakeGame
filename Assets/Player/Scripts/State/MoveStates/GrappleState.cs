using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrappleState : PlayerStateBase
{
    public override void Enter()
    {
        //�J�������������ɂ���
        _stateMachine.PlayerController.CameraControl.UseSwingCamera();

        //���x������ݒ�
        _stateMachine.PlayerController.Grapple.SetSpeedGrapple();

        //�O���b�v���̏����ݒ�
        _stateMachine.PlayerController.Grapple.GrappleSetting();
    }

    public override void Exit()
    {
        //�O���b�v���I���̐ݒ�
        _stateMachine.PlayerController.Grapple.StopSwing();
    }

    public override void FixedUpdate()
    {
        //�O���b�v�����̓���
        _stateMachine.PlayerController.Grapple.GrappleMove();
    }

    public override void LateUpdate()
    {
        //���C���[�̕`��
        _stateMachine.PlayerController.Grapple.DrawLope();
    }

    public override void Update()
    {
        _stateMachine.PlayerController.CoolTimes();


        //�ǂ�����������AWallRun��Ԃ�
        if (_stateMachine.PlayerController.WallRunCheck.CheckWalAlll())
        {
            if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateWallRun);
            }    //WallRun�ֈڍs
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            }
            return;
        }

        if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            if (_stateMachine.PlayerController.Rb.velocity.y > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }
        }


        //���C���[���n�_�Ǝ����̋�������苗���ɂ܂ŒB������A�I���Ƃ���
        _stateMachine.PlayerController.Grapple.CheckDiestance();

    }
}