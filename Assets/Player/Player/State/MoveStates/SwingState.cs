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
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.Swing.AddSpeed();


    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.Swing.DrawLope();

        //カメラを傾ける
        _stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);

    }

    public override void Update()
    {
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

        }

    }
}
