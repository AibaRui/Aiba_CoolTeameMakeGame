using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Attack.AttackEnter();
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Attack.AttackExit();
        _stateMachine.PlayerController.AimAssist.ResetLockOn();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.Attack.AttacFixedUpdata();
    }

    public override void LateUpdate()
    {
 
    }

    public override void Update()
    {
        _stateMachine.PlayerController.Attack.AttackUpdata();

        if (!_stateMachine.PlayerController.Attack.IsEndAttack) return;

        if (_stateMachine.PlayerController.Rb.velocity.y > 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);
        }
        else
        {
            _stateMachine.TransitionTo(_stateMachine.StateDownAir);
        }
    }
}
