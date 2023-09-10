using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwingState : PlayerStateBase
{
    public override void Enter()
    {
        //速度設定
        _stateMachine.PlayerController.Swing.SetSpeedSwing();
        //Swingの初期設定
        _stateMachine.PlayerController.Swing.SwingSetting();
        //アニメーションの設定
        _stateMachine.PlayerController.AnimControl.SwingAnim.Swing(true);

        //ワイヤーを飛ばす音
        _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.WireFireSounds();

        //ロープの描画設定
        _stateMachine.PlayerController.Swing.FirstSettingDrawLope();

        //風の音の設定
        _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(true);
    }

    public override void Exit()
    {
        //アニメーションの設定
        _stateMachine.PlayerController.AnimControl.SwingAnim.Swing(false);
        _stateMachine.PlayerController.AnimControl.SwingAnim.SetHighFallType();

        //Swing終了時のカメラの再設定
        _stateMachine.PlayerController.CameraControl.SwingEndSetCamera();

        //FrontZipを実行可能にする
        _stateMachine.PlayerController.ZipMove.SetCanZip();


        if (_stateMachine.PlayerController.PlayerT.eulerAngles.y > 180)
        {
            _stateMachine.PlayerController.CameraControl.SwingEndPlayerRotateY = _stateMachine.PlayerController.PlayerT.eulerAngles.y - 360;
        }
        else
        {
            _stateMachine.PlayerController.CameraControl.SwingEndPlayerRotateY = _stateMachine.PlayerController.PlayerT.eulerAngles.y;
        }
    }

    public override void FixedUpdate()
    {
        //Swing中の加速
        _stateMachine.PlayerController.Swing.AddSpeed();

        //Swing中の回転
        _stateMachine.PlayerController.Swing.SwingRotation();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.Swing.DrowWire();


        //カメラの回転速度を計算する
        _stateMachine.PlayerController.CameraControl.CountTime();

        //カメラを傾ける
        //_stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);

        //FOV設定
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(true, _stateMachine.PlayerController.Rb.velocity.y);

        //カメラを傾ける。X軸
        _stateMachine.PlayerController.CameraControl.SwingCameraValueX(true);

    }

    public override void Update()
    {
        //各動作のクールタイムを計測
        _stateMachine.PlayerController.CoolTimes();

        //ワイヤーを描画するまでの時間を計測
        _stateMachine.PlayerController.Swing.CountDrowWireTime();

        //壁が当たったら、WallRun状態に
        if (_stateMachine.PlayerController.WallRunCheck.CheckWalAlll())
        {
            if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            {

                // _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            }    //WallRunへ移行
            else
            {
                _stateMachine.PlayerController.WallRun.SetNoMove(true);
                //_stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            }

            //Swingのカメラの値のリセット
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallRunEndCamera();

            _stateMachine.TransitionTo(_stateMachine.StateWallRun);

            _stateMachine.PlayerController.AnimControl.WallRunTransition();

            _stateMachine.PlayerController.Swing.StopSwing(false);

            //風の音の設定
            _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(false);

            return;
        }


        //アンカーの着地点より高く上がったら
        if (_stateMachine.PlayerController.Swing.CheckLine())
        {
            //アニメーション設定(Swing終了ジャンプのタイプ分け)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(1);

            //アニメーションの設定
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingHighEnd();

            //ジャンプ音
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.UpJumpSounds();
            //マントの音
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //ジャンプ処理
            _stateMachine.PlayerController.Swing.LastJumpUp();

            //ジャンプして終わる
            _stateMachine.PlayerController.Swing.StopSwing(true);


            //ジャンプして終わる
            _stateMachine.PlayerController.Move.IsUseSpeedDash();

            //スピードの加減の設定
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //カメラの追従を始める
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //上昇して終了
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(true, false, _stateMachine.PlayerController.Rb.velocity.y);

            //推移。(Y速度によって水位先を変える)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);

            return;
        }

        //Swingのボタンを離したら
        if (_stateMachine.PlayerController.InputManager.IsSwing < 0.6f)
        {
            //アニメーション設定(Swing終了ジャンプのタイプ分け)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(0);

            //ジャンプ音
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.SwingEndSounds();
            //マントの音
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //ジャンプしないで終わる
            _stateMachine.PlayerController.Swing.StopSwing(false);

            //カメラの追従を始める
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //スピードの加減の設定
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(false, false, _stateMachine.PlayerController.Rb.velocity.y);

            //推移
            if (_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }   //地面についててたら=>Idle
            else
            {
                //推移。(Y速度によって水位先を変える)
                if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                else _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }   //空中だったら=>_Air
            return;
        }

        //Swing中にジャンプ押したら
        if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            //アニメーション設定(Swing終了ジャンプのタイプ分け)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(2);
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetJumpEndType();

            //ジャンプ音
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.FrontJumpSounds();
            //マントの音
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //_stateMachine.PlayerController.CameraControl.SwingEndTranspectorUp();

            //ジャンプ処理
            _stateMachine.PlayerController.Swing.LastJumpFront();

            //ジャンプして終わる
            _stateMachine.PlayerController.Swing.StopSwing(true);

            //スピードの加減速
            // _stateMachine.PlayerController.Move.IsUseSpeedDash();

            //空中で前方に加速する、ということを伝える
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //カメラの追従を始める
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //上昇して終了
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(false, true, _stateMachine.PlayerController.Rb.velocity.y);

            //推移。(Y速度によって水位先を変える)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);
        }

    }
}
