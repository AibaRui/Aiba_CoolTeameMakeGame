using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossIdleState : BossStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
       // Debug.Log("Boss_idle");
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        _stateMachine.BossControl.BossRotation.Rotate();
        _stateMachine.BossControl.BossAttack.AttackCoolTimeCount();

        if(_stateMachine.BossControl.IsMovie)
        {
            Debug.Log("Idle=>Wait");
            _stateMachine.TransitionTo(_stateMachine.EventState);
            return;
        }

        if (_stateMachine.BossControl.BossAttack.IsAttack)
        {
            Debug.Log("Idle=>Attack");
            _stateMachine.TransitionTo(_stateMachine.AttackState);
            return;
        }

        //�_���[�W
        if (_stateMachine.BossControl.BossHp.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }
    }
}
