using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvoidState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Avoid.SetSpeedLimit();

        _stateMachine.PlayerController.AnimControl.Avoid();
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Avoid.EndAvoid();
    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        _stateMachine.PlayerController.Avoid.DoAvoid();

        if(_stateMachine.PlayerController.Avoid.IsEndAvoid)
        {
            //  �n��ł̈ړ�
            if (_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
                {
                    if (_stateMachine.PlayerController.InputManager.IsSwing == 1)
                    {
                        _stateMachine.TransitionTo(_stateMachine.StateRun);
                    }   //����������Ă����瑖��
                    else
                    {
                        _stateMachine.TransitionTo(_stateMachine.StateWalk);
                    }  //�����Ă��Ȃ����������
                }
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }   //�������


            //�󒆂ɂ���Ƃ�
            if (!_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                if (_stateMachine.PlayerController.Rb.velocity.y > 0)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                }   //�㏸
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateDownAir);
                }   //�~��
            }
        }

    }
}