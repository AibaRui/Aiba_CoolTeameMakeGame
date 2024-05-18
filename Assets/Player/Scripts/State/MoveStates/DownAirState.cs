using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DownAirState : PlayerStateBase
{
    public override void Enter()
    {
        //カメラを遠巻きにする
        _stateMachine.PlayerController.CameraControl.UseCanera(CameraType.Swing);

        //速度設定
        _stateMachine.PlayerController.VelocityLimit.SetLimit(25, 40, -25, 25);
    }

    public override void Exit()
    {
        //コントローラーの振動
        _stateMachine.PlayerController.ControllerVibrationManager.StopVibration();
        _stateMachine.PlayerController.Move.ReSetTime();
        _stateMachine.PlayerController.CameraControl.EndSwingCamera();
    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            _stateMachine.PlayerController.Move.AirMove();

        //速度の減衰
        _stateMachine.PlayerController.VelocityLimit.SlowToSpeedUp();

        //左右回転設定
        _stateMachine.PlayerController.PlayerModelRotation.ResetDoModelRotate();
    }

    public override void LateUpdate()
    {
        //カメラの時間
        _stateMachine.PlayerController.CameraControl.CountTime();

        //ステータス設定
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.CheckStatas();

        //カメラの傾きを戻す
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetAirCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetAirCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetAirCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);

        //カメラのX軸のOffsetを戻す
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetOffSetX();

        //FOV設定
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(false, _stateMachine.PlayerController.Rb.velocity.y);

        //カメラをプレイヤーの後ろに自動的に向ける。X軸
        _stateMachine.PlayerController.CameraControl.SwingEndCameraAutoFollow();

        //カメラを傾ける。X軸
        _stateMachine.PlayerController.CameraControl.SwingCameraValueX(false);

        //PointZipのUI
        _stateMachine.PlayerController.PointZip.PointZipUI.UpdatePointZipUIPosition();
    }

    public override void Update()
    {
        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        //Swingの実行待機時間の計測
        _stateMachine.PlayerController.Swing.SwingLimit.CountSwingLimitTime();

        _stateMachine.PlayerController.Move.DownSpeedOfSppedDash();
        
        //Boss戦、接触確認
        _stateMachine.PlayerController.PlayerBossHit.CheckHittingTime();


        if(_stateMachine.PlayerController.Rb.velocity.y<-10 && !_stateMachine.PlayerController.PlayerPostEffectSetting.IsEnable)
        {
            //画面効果PostEffect_On
            _stateMachine.PlayerController.PlayerPostEffectSetting.OnPostEffect();
        }


        //ダメージ
        if (_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }

        //位置調整
        if(_stateMachine.PlayerController.PlayerReplace.IsRemove)
        {
            _stateMachine.TransitionTo(_stateMachine.EventState);
            return;
        }   

        //Boss接触
        if(_stateMachine.PlayerController.PlayerBossHit.IsHitBoss)
        {
            _stateMachine.TransitionTo((_stateMachine.EventState));
            return;
        }


        //PoinZip
        if (!_stateMachine.PlayerController.IsBossButtle)
        {
            if (_stateMachine.PlayerController.PointZip.Search())
            {
                _stateMachine.TransitionTo(_stateMachine.PointZipState);
                return;
            }
        }

        if (_stateMachine.PlayerController.InputManager.IsAttack)
        {
            if (_stateMachine.PlayerController.Attack.IsCanAttack)
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
            }
        }   //攻撃ステート

        //Zip
        if (_stateMachine.PlayerController.InputManager.IsJumping && _stateMachine.PlayerController.ZipMove.IsCanZip)
        {
            //風の音の設定
            _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(false);

            _stateMachine.TransitionTo(_stateMachine.StateZip);
            return;
        }

        if (_stateMachine.PlayerController.SearchSwingPoint.Search())
        {
            if (_stateMachine.PlayerController.Swing.IsCanSwing &&
                _stateMachine.PlayerController.Swing.SwingLimit.IsCanSwing &&
                _stateMachine.PlayerController.InputManager.IsSwing == 1)
            {
                _stateMachine.TransitionTo(_stateMachine.StateSwing);
                return;
            }
        }

        if (_stateMachine.PlayerController.GroundCheck.IsHit())
        {
            //Swingのカメラの値のリセット
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            //着地音
            _stateMachine.PlayerController.PlayerAudioManager.GroundAudio.LandAudioPlay();
            //風の音の設定
            _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(false);

            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }   //地面

        if (_stateMachine.PlayerController.InputManager.IsSetUp > 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateGrappleSetUp);
            return;
        }   //構え

        //if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        //{
        //    _stateMachine.PlayerController.Avoid.SetAvoidDir();
        //    _stateMachine.TransitionTo(_stateMachine.AvoidState);
        //}   //回避

        //壁が当たったら、WallRun状態に
        //if (_stateMachine.PlayerController.WallRunCheck.CheckWall())
        //{
        //    if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
        //    {
        //        _stateMachine.TransitionTo(_stateMachine.StateWallRun);
        //    }    //WallRunへ移行
        //    else
        //    {
        //        _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
        //    } 　//WallIdleへ移行
        //}


    }
}
