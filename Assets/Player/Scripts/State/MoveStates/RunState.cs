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
            return;
        }   //回避

        //上昇、降下
        if (!_stateMachine.PlayerController.GroundCheck.IsHit())
        {
            if (_stateMachine.PlayerController.Rb.velocity.y > 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }      //上昇
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }   //降下


            //Swingの実行待機時間を設定
            _stateMachine.PlayerController.Swing.SwingLimit.SetSwingLimit(1);

            return;
        }



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
