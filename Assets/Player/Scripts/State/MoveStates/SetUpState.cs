using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetUpState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.SetUp.SetUpCamera();

       _stateMachine.PlayerController.CameraControl.SetUpCamera();
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.SetUp.SetEnd();
    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //�\����Ԃ�
        _stateMachine.PlayerController.SetUp.SetUping();

        if (_stateMachine.PlayerController.InputManager.IsAttack && _stateMachine.PlayerController.Attack.IsCanAttack)
        {
            _stateMachine.PlayerController.Attack.AttackEnemy();
        }   //�U��

        //�e����̃N�[���^�C��
        _stateMachine.PlayerController.CoolTimes();

        if (_stateMachine.PlayerController.Grapple.SearchGrapplePoint())
        {
            //���g���K�[������邩0�ɂȂ�����
            if (_stateMachine.PlayerController.InputManager.IsSwing > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateGrapple);
            }
        }   //���C���[��������AGrapple�\

        //���g���K�[������邩0�ɂȂ�����
        if (_stateMachine.PlayerController.InputManager.IsSetUp == 0)
        {
            if (_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }   //�n�ʂɂ���ꍇ
            else
            {
                if (_stateMachine.PlayerController.Rb.velocity.y > 0)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                }   //�㏸
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateDownAir);
                }   //�~��

            }   //�󒆂ɂ���ꍇ

        }
    }
}