using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Rb.useGravity = false;

        //Zipの初期設定
        _stateMachine.PlayerController.ZipMove.ZipFirstSetting();

        //LineRenderer設定
        _stateMachine.PlayerController.ZipLineRenderer.StartZipLine();

        //アニメーションの設定
        _stateMachine.PlayerController.AnimControl.ZipAnim.FrontZip();
        _stateMachine.PlayerController.AnimControl.ZipAnim.SetZip(true);

        _stateMachine.PlayerController.EffectControl.ZipSet(true);


        //画面効果PostEffect_On
        _stateMachine.PlayerController.PlayerPostEffectSetting.OnPostEffect();
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Rb.useGravity = true;

        //アニメーション設定
        _stateMachine.PlayerController.AnimControl.ZipAnim.SetZip(false);

        //FrontZipのタイマーをリセット
        _stateMachine.PlayerController.ZipMove.EndZip();

        //LineRenderer設定
        _stateMachine.PlayerController.ZipLineRenderer.ResetZipLine();

        _stateMachine.PlayerController.EffectControl.ZipSet(false);

        //画面効果PostEffect_Off
        _stateMachine.PlayerController.PlayerPostEffectSetting.OffPostEffect();
    }

    public override void FixedUpdate()
    {
        //プレイヤーの角度を変更
        _stateMachine.PlayerController.ZipMove.ZipMove.ZipSetPlayerRotation();
        _stateMachine.PlayerController.ZipLineRenderer.MedalPosition();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.CameraControl.ZipCameraControl.ZipCamera();

        //LineRenderer設定
        _stateMachine.PlayerController.ZipLineRenderer.SetZipLineWave();
    }

    public override void Update()
    {
        //Zipの有効時間を計測
        _stateMachine.PlayerController.ZipMove.CountFrotZipTime();

        if (_stateMachine.PlayerController.GroundCheck.IsHit())
        {
            //Swingのカメラの値のリセット
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }

        if (_stateMachine.PlayerController.WallRunCheck.CheckWallFront())
        {
            //Swingのカメラの値のリセット
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            return;
        }

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
