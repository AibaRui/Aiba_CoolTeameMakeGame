using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipState : PlayerStateBase
{
    public override void Enter()
    {
        //前方に飛ぶ
        _stateMachine.PlayerController.ZipMove.FrontZip();

        //演出
        _stateMachine.PlayerController.ZipMove.SetCameraDistance();

        //アニメーションの設定
        _stateMachine.PlayerController.AnimControl.FrontZip();

        _stateMachine.PlayerController.EffectControl.ZipSet(true);
    }

    public override void Exit()
    {
        //FrontZipのタイマーをリセット
        _stateMachine.PlayerController.ZipMove.EndZip();

        _stateMachine.PlayerController.EffectControl.ZipSet(false);
    }

    public override void FixedUpdate()
    {
        //プレイヤーの角度を変更
        _stateMachine.PlayerController.ZipMove.ZipSetPlayerRotation();
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
            //推移。(Y速度によって水位先を変える)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);

            //空中で前方に加速する、ということを伝える
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();
        }
    }
}
