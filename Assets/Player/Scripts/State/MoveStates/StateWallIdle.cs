using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallIdle : PlayerStateBase
{
    public override void Enter()
    {
        //カメラをWallRun用に変更
        _stateMachine.PlayerController.CameraControl.UseWallRunCamera();

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.Rb.velocity = Vector3.zero;
        _stateMachine.PlayerController.Rb.useGravity = false;
 
       // _stateMachine.PlayerController.WallRun.SetMoveDir(WallRun.MoveDirection.Up);

        //WallRunのAnimatorを設定
        _stateMachine.PlayerController.AnimControl.WallRunSet(true);
    }

    public override void Exit()
    {
        //止まっているときに、カメラの値をIdle状態の値に変更するまでの時間の計測。の値をリセット
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.ResetCountReSetWallIdleCameraTime();

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
        //止まっているときに、カメラの値をIdle状態の値に変更する
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallIdleCamera();

        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.XOffSetWallIdle();

        //Y軸のoffsetの設定
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.SetOffsetY(false);
    }

    public override void Update()
    {
        //止まっているときに、カメラの値をIdle状態の値に変更する。までの時間を計測
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.CountReSetWallIdleCameraTime();


        float h = _stateMachine.PlayerController.InputManager.HorizontalInput;
        float v = _stateMachine.PlayerController.InputManager.VerticalInput;

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();
        _stateMachine.PlayerController.WallRun.MidleDir();

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        if (h!=0 || v>0)
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
