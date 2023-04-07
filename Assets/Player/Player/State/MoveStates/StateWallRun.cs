using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallRun : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.Rb.useGravity = false;
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();

        //WallRun直後がは、Swingを不可にする
        _stateMachine.PlayerController.Swing.SetSwingFalse();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.WallRun.WallMove();
        Debug.Log("WallRun");
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        bool isHit = _stateMachine.PlayerController.WallRunCheck.CheckHitWall();

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        if (_stateMachine.PlayerController.InputManager.IsSwing <= 0 )
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
        }    //WallRunへ移行

        if (_stateMachine.PlayerController.InputManager.IsJumping || !isHit)
        {
            _stateMachine.TransitionTo(_stateMachine.StateJump);
            _stateMachine.PlayerController.WallRun.LastJump();
            _stateMachine.PlayerController.Rb.useGravity = true;
        }    //WallRunへ移行
    }
}
