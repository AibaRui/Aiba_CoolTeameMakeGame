using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IdleState : PlayerStateBase
{
    public override void Enter()
    {
        //カメラを近くする
        _stateMachine.PlayerController.CameraControl.RsetCamera();

        //モデルの左右回転をリセット
        _stateMachine.PlayerController.Swing.SwingRotationSetting.ResetModelRotate();
    }

    public override void Exit()
    {
        //FrontZipを実行可能にする
        _stateMachine.PlayerController.ZipMove.SetCanZip();
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





        //if (_stateMachine.PlayerController.InputManager.IsSetUp > 0)
        //{
        //    _stateMachine.TransitionTo(_stateMachine.StateGrappleSetUp);
        //}   //構え

        //  地上での移動
        if (_stateMachine.PlayerController.GroundCheck.IsHit())
        {

            if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
            {
                _stateMachine.PlayerController.Avoid.SetAvoidDir();
                _stateMachine.TransitionTo(_stateMachine.AvoidState);
            }   //回避

            
            if(_stateMachine.PlayerController.InputManager.IsAttack && _stateMachine.PlayerController.Attack.IsCanAttack)
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
            }   //攻撃


            //ジャンプ
            if (_stateMachine.PlayerController.InputManager.IsJumping && _stateMachine.PlayerController.GroundCheck.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.StateJump);
            }

            if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            {
                if (_stateMachine.PlayerController.InputManager.IsSwing == 1)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateRun);
                }   //走りを押していたら走る
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateWalk);
                }  //押していなかったら歩く

            }
        }


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
        }
    }
}
