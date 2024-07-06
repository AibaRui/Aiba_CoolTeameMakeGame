using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttackState : BossStateBase
{
    public override void Enter()
    {
        Debug.Log("Enter_Attack");
        _stateMachine.BossControl.BossAttack.AttackEnter();
        _stateMachine.BossControl.BossAttack.Attack();
    }

    public override void Exit()
    {
        _stateMachine.BossControl.BossAttack.AttackExit();
    }

    public override void FixedUpdate()
    {
        Debug.Log("attack");
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        if (_stateMachine.BossControl.BossHp.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }

        if (!_stateMachine.BossControl.BossAttack.IsAttackNow)
        {
            Debug.Log("Attack=>Idle");
            _stateMachine.TransitionTo(_stateMachine.IdleState);
            return;
        }
    }
}
