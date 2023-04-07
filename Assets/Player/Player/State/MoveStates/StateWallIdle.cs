using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallIdle : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.Rb.velocity = Vector3.zero;
        _stateMachine.PlayerController.Rb.useGravity = false;
        Debug.Log("Idle");
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();

        //WallRun直後がは、Swingを不可にする
        _stateMachine.PlayerController.Swing.SetSwingFalse();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.WallRun.AddWall();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.WallRun.MidleDir();

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        if (_stateMachine.PlayerController.InputManager.IsSwing>0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallRun);
        }    //WallRunへ移行

        if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            _stateMachine.TransitionTo(_stateMachine.StateJump);
            _stateMachine.PlayerController.WallRun.LastJump();
            _stateMachine.PlayerController.Rb.useGravity = true;
        }    //WallRunへ移行

    }
}
