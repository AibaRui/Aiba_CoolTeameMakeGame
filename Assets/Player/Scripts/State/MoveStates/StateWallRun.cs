using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallRun : PlayerStateBase
{
    public override void Enter()
    {
        //Swingのカメラの値のリセット
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

        //カメラをWallRun用に変更
        _stateMachine.PlayerController.CameraControl.UseWallRunCamera();

        _stateMachine.PlayerController.WallRunCheck.CheckHitWall();

        _stateMachine.PlayerController.Rb.useGravity = false;

        //WallRunのAnimatorを設定
        _stateMachine.PlayerController.AnimControl.WallRunSet(true);

        Debug.Log("WAll");
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();

        //WallRun直後がは、Swingを不可にする
        _stateMachine.PlayerController.Swing.SetSwingFalse();

        _stateMachine.PlayerController.WallRun.SetNoMove(false);
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.WallRun.WallMove();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallRunCameraFollow();

        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.XOffSetControlWallRun();

        //Y軸のoffsetの設定
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.SetOffsetY(true);
    }

    public override void Update()
    {
        _stateMachine.PlayerController.WallRun.CountNoMove();

        bool isHit = _stateMachine.PlayerController.WallRunCheck.CheckHitWall();

        float h = _stateMachine.PlayerController.InputManager.HorizontalInput;
        float v = _stateMachine.PlayerController.InputManager.VerticalInput;

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        //段差確認
        _stateMachine.PlayerController.WallRunStep.CheckWallStep();

        //段差登りに移行
        if(_stateMachine.PlayerController.WallRunStep.IsHitStep)
        {
            _stateMachine.TransitionTo(_stateMachine.WallRunStep);
            return;
        }

        if(_stateMachine.PlayerController.WallRunUpZip.CheckUpZipPosition()&& _stateMachine.PlayerController.InputManager.IsJumping)
        {
            _stateMachine.TransitionTo(_stateMachine.WallRunUpZipState);
            return;
        }

        if (h==0 && v<=0 && !_stateMachine.PlayerController.WallRun.IsEndNoMove)
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

            //Swingの実行待機時間を設定
            _stateMachine.PlayerController.Swing.SwingLimit.SetSwingLimit(1);

            //移行
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);

        }    //WallRunへ移行
    }
}
