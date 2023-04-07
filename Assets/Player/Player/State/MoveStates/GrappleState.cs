using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrappleState : PlayerStateBase
{
    public override void Enter()
    {
        //速度制限を設定
        _stateMachine.PlayerController.Grapple.SetSpeedGrapple();

        //グラップルの初期設定
        _stateMachine.PlayerController.Grapple.GrappleSetting();
    }

    public override void Exit()
    {
        //グラップル終了の設定
        _stateMachine.PlayerController.Grapple.StopSwing();
    }

    public override void FixedUpdate()
    {
        //グラップル中の動き
        _stateMachine.PlayerController.Grapple.GrappleMove();
    }

    public override void LateUpdate()
    {
        //ワイヤーの描写
        _stateMachine.PlayerController.Grapple.DrawLope();
    }

    public override void Update()
    {
        _stateMachine.PlayerController.CoolTimes();


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
            return;
        }

        if (_stateMachine.PlayerController.InputManager.IsJumping)
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


        //ワイヤー着地点と自分の距離が一定距離にまで達したら、終了とする
        _stateMachine.PlayerController.Grapple.CheckDiestance();

    }
}
