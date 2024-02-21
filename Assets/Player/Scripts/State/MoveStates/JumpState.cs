using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.AnimControl.Jump();
        _stateMachine.PlayerController.Move.Jump();

        //ジャンプ音
        _stateMachine.PlayerController.PlayerAudioManager.GroundAudio.JumpAudioPlay();
    }

    public override void Exit()
    {
        //Swingの実行待機時間を設定
        _stateMachine.PlayerController.Swing.SwingLimit.SetSwingLimit(1);
    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        _stateMachine.PlayerController.CoolTimes();

        if (_stateMachine.PlayerController.Rb.velocity.y>0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);
        }
        else
        {
            _stateMachine.TransitionTo(_stateMachine.StateDownAir);
        }
    }
}
