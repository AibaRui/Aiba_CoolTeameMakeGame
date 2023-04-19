using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwingState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Swing.SetSpeedSwing();
        _stateMachine.PlayerController.Swing.SwingSetting();
        _stateMachine.PlayerController.AnimControl.Swing(true);
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.AnimControl.Swing(false);
        _stateMachine.PlayerController.CameraControl.SwingEndSetCamera();


        if (_stateMachine.PlayerController.PlayerT.eulerAngles.y > 180)
        {
            _stateMachine.PlayerController.CameraControl.SwingEndPlayerRotateY = _stateMachine.PlayerController.PlayerT.eulerAngles.y - 360;
        }
        else
        {
            _stateMachine.PlayerController.CameraControl.SwingEndPlayerRotateY = _stateMachine.PlayerController.PlayerT.eulerAngles.y;
        }

      //  Debug.Log($"最終は:{_stateMachine.PlayerController.PlayerT.eulerAngles.y}");
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.Swing.AddSpeed();





        //カメラを傾ける
        _stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
        //カメラを傾ける。X軸
        _stateMachine.PlayerController.CameraControl.SwingCameraValueX(true);
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.Swing.DrawLope();



    }

    public override void Update()
    {
        _stateMachine.PlayerController.InputManager.SwingIngInputSet();


        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        _stateMachine.PlayerController.CameraControl.CountTime();

        _stateMachine.PlayerController.Swing.SwingStartCount();

        _stateMachine.PlayerController.Swing.CheckLine();

        //壁が当たったら、WallRun状態に
        if (_stateMachine.PlayerController.WallRunCheck.CheckWalAlll())
        {
            if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateWallRun);
            }    //WallRunへ移行
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            }
            _stateMachine.PlayerController.Swing.StopSwing(false);
            return;
        }



        if (_stateMachine.PlayerController.Swing.IsSamLime && _stateMachine.PlayerController.Swing.IsStartSwing)
        {
            //ジャンプして終わる
            _stateMachine.PlayerController.Swing.StopSwing(true);

            //FrontZipのタイマーをリセット
            _stateMachine.PlayerController.ZipMove.ResetFrontZip(true);


            if (_stateMachine.PlayerController.Rb.velocity.y > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }
            _stateMachine.PlayerController.Swing.LastJumpUp();
            _stateMachine.PlayerController.Move.IsUseSpeedDash();

            //スピードの加減の設定
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //カメラの追従を始める
            _stateMachine.PlayerController.CameraControl.EndFollow();
        }
        else if (_stateMachine.PlayerController.InputManager.IsSwing != 1)
        {
            //ジャンプしないで終わる
            _stateMachine.PlayerController.Swing.StopSwing(false);

            if (_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }
            else
            {
                if (_stateMachine.PlayerController.Rb.velocity.y > 0)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                }
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateDownAir);
                }

            }

            //カメラの追従を始める
            _stateMachine.PlayerController.CameraControl.EndFollow();

            //スピードの加減の設定
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();
        }
        else if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            _stateMachine.PlayerController.CameraControl.SwingEndTranspectorUp();

            //ジャンプして終わる
            _stateMachine.PlayerController.Swing.StopSwing(true);

            //FrontZipのタイマーをリセット
            _stateMachine.PlayerController.ZipMove.ResetFrontZip(true);

            _stateMachine.PlayerController.Swing.LastJumpFront();
            _stateMachine.PlayerController.Move.IsUseSpeedDash();
            if (_stateMachine.PlayerController.Rb.velocity.y > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }
            //スピードの加減の設定
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //カメラの追従を始める
            _stateMachine.PlayerController.CameraControl.EndFollow();
        }

    }
}
