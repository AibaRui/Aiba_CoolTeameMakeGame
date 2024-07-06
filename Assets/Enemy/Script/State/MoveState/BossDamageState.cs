using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDamageState : BossStateBase
{
    public override void Enter()
    {
        Debug.Log("damage");
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        if (!_stateMachine.BossControl.BossHp.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.IdleState);
        }
    }
}
