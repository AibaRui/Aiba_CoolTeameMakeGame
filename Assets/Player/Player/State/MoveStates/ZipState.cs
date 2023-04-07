using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.AnimControl.FrontZip();
        //前方に飛ぶ
        _stateMachine.PlayerController.ZipMove.FrontZip();
    }

    public override void Exit()
    {
        //FrontZipのタイマーをリセット
        _stateMachine.PlayerController.ZipMove.ResetFrontZip(false);
    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //Zipの有効時間を計測
        _stateMachine.PlayerController.ZipMove.CountFrotZipTime();

        if (_stateMachine.PlayerController.ZipMove.IsEndFrontZip)
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
}
