using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpAirState : PlayerStateBase
{
    public override void Enter()
    {
        //カメラを遠巻きにする
        _stateMachine.PlayerController.CameraControl.UseSwingCamera();


        _stateMachine.PlayerController.VelocityLimit.SetLimit(25, 20, 25);
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();

    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            _stateMachine.PlayerController.Move.AirMove();


        //速度の減衰
        _stateMachine.PlayerController.VelocityLimit.SlowToSpeedUp();



    }

    public override void LateUpdate()
    {
        //カメラの時間
        _stateMachine.PlayerController.CameraControl.CountTime();
        //カメラの傾きを戻す
        _stateMachine.PlayerController.CameraControl.AirCameraYValue(_stateMachine.PlayerController.Rb.velocity.y);

        //カメラをプレイヤーの後ろに自動的に向ける。X軸
        _stateMachine.PlayerController.CameraControl.SwingEndCameraAutoFollow();

        //カメラを傾ける。X軸
        _stateMachine.PlayerController.CameraControl.SwingCameraValueX(false);
    }

    public override void Update()
    {





        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        _stateMachine.PlayerController.Move.DownSpeedOfSppedDash();



        //if (_stateMachine.PlayerController.InputManager.IsSetUp > 0)
        //{

        //    _stateMachine.TransitionTo(_stateMachine.StateGrappleSetUp);
        //}   //構え

        if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        {
            _stateMachine.PlayerController.Avoid.SetAvoidDir();
            _stateMachine.TransitionTo(_stateMachine.AvoidState);
        }   //回避


        if (_stateMachine.PlayerController.InputManager.IsAttack)
        {
            if (_stateMachine.PlayerController.Attack.IsCanAttack)
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
            }
        }   //攻撃ステート

        ////壁が当たったら、WallRun状態に
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
        ////}

        //Zip
        if (_stateMachine.PlayerController.InputManager.IsJumping && _stateMachine.PlayerController.ZipMove.IsCanZip)
        {
            _stateMachine.TransitionTo(_stateMachine.StateZip);
            return;
        }


        if (_stateMachine.PlayerController.SearchSwingPoint.Search())
        {
            if (_stateMachine.PlayerController.Swing.IsCanSwing &&
                _stateMachine.PlayerController.InputManager.IsSwing == 1)
            {
                _stateMachine.TransitionTo(_stateMachine.StateSwing);
                return;
            }
        }

        if (_stateMachine.PlayerController.Rb.velocity.y <= 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            return;
        }


    }
}
