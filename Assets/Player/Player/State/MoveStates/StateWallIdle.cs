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

        //WallRunのAnimatorを設定
        _stateMachine.PlayerController.AnimControl.WallRunSet(true);
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();

        //WallRun直後がは、Swingを不可にする
        _stateMachine.PlayerController.Swing.SetSwingFalse();

        //FrontZipを実行可能にする
        _stateMachine.PlayerController.ZipMove.SetCanZip();
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

        if (_stateMachine.PlayerController.InputManager.IsSwing > 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallRun);
        }    //WallRunへ移行

        if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            //重力をオン
            _stateMachine.PlayerController.Rb.useGravity = true;

            //WallRunのAnimatorを設定
            _stateMachine.PlayerController.AnimControl.WallRunSet(false);

            //ジャンプ処理
            _stateMachine.PlayerController.WallRun.LastJump(false);

            //WallRunへ移行
            _stateMachine.TransitionTo(_stateMachine.StateJump);

        }

    }
}
