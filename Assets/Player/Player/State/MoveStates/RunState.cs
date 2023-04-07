using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.Move.Move(PlayerMove.MoveType.Run);
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        _stateMachine.PlayerController.CoolTimes();

        if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        {
            _stateMachine.PlayerController.Avoid.SetAvoidDir();
            _stateMachine.TransitionTo(_stateMachine.AvoidState);
        }   //回避

        //歩き
        if ((_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            && _stateMachine.PlayerController.InputManager.IsSwing != 1)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWalk);
        }


        //止まる
        if (_stateMachine.PlayerController.InputManager.HorizontalInput == 0 && _stateMachine.PlayerController.InputManager.VerticalInput == 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
        }

        //ジャンプ
        if (_stateMachine.PlayerController.InputManager.IsJumping && _stateMachine.PlayerController.GroundCheck.IsHit())
        {
            _stateMachine.TransitionTo(_stateMachine.StateJump);
        }
    }
}
