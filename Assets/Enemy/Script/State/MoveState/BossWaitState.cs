using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossWaitState : BossStateBase
{
    public override void Enter()
    {
        _stateMachine.BossControl.Wait.WeightStop();
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
        if(!_stateMachine.BossControl.IsMovie)
        {
            _stateMachine.BossControl.Wait.StartGame();
            Debug.Log("Wait=>Idle");
            _stateMachine.TransitionTo(_stateMachine.IdleState);
        }
    }
}
