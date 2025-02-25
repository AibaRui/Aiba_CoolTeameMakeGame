using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialState : PlayerStateBase
{
    public override void Enter()
    {
        if(_stateMachine.PlayerController.Tutorial.IsEndTutorial)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
        }
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
        if (_stateMachine.PlayerController.Tutorial.IsEndTutorial)
        {
            _stateMachine.TransitionTo(_stateMachine.StateDownAir);
        }
    }
}
