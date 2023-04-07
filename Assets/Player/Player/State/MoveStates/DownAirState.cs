using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DownAirState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Move.ReSetTime();
    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            _stateMachine.PlayerController.Move.AirMove();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        _stateMachine.PlayerController.Move.DownSpeedOfSppedDash();

        if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        {
            _stateMachine.PlayerController.Avoid.SetAvoidDir();
            _stateMachine.TransitionTo(_stateMachine.AvoidState);
        }   //回避


        if (_stateMachine.PlayerController.InputManager.IsAttack)
        {
            if (_stateMachine.PlayerController.Attack.IsCanAttack && _stateMachine.PlayerController.Attack.SearchEnemy())
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
            }
        }   //攻撃ステート

        //Grapple
        if (_stateMachine.PlayerController.InputManager.IsSwing < 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateGrappleSetUp);
        }

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

        //Zip
        if (_stateMachine.PlayerController.InputManager.IsJumping)
        {
            _stateMachine.TransitionTo(_stateMachine.StateZip);
            return;
        }

        if (_stateMachine.PlayerController.SearchSwingPoint.IsCanHit)
        {
            if (_stateMachine.PlayerController.Swing.IsCanSwing &&
                _stateMachine.PlayerController.InputManager.IsSwing == 1)
            {
                _stateMachine.TransitionTo(_stateMachine.StateSwing);
                return;
            }
        }

        if (_stateMachine.PlayerController.GroundCheck.IsHit())
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }
    }
}
