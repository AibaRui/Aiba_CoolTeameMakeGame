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

        //WallRunのAnimatorを設定
        _stateMachine.PlayerController.AnimControl.WallRunSet(true);
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
            //重力をオン
            _stateMachine.PlayerController.Rb.useGravity = true;

            //WallRunのAnimatorを設定
            _stateMachine.PlayerController.AnimControl.WallRunSet(false);
            
            //ジャンプ処理
            _stateMachine.PlayerController.WallRun.LastJump(true);

            //移行
            _stateMachine.TransitionTo(_stateMachine.StateJump);
        }    //WallRunへ移行
    }
}
