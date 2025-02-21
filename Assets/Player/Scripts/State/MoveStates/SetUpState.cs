using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetUpState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.SetUp.EnterSetUp();

        _stateMachine.PlayerController.SetUp.SetUpCamera();

        _stateMachine.PlayerController.CameraControl.UseCanera(CameraType.Setup);
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.SetUp.ExitSetUp();

        _stateMachine.PlayerController.SetUp.SetEnd();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.SetUp.FallSpeedDown();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //構え状態の
        _stateMachine.PlayerController.SetUp.SetUping();

        //左トリガーが離れるか0になった時
        if (_stateMachine.PlayerController.InputManager.IsSetUp == 0)
        {
            if (_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }   //地面にいる場合
            else
            {
                if (_stateMachine.PlayerController.Rb.velocity.y > 0)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                }   //上昇
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateDownAir);
                }   //降下

            }   //空中にいる場合
            return;
        }


        if (_stateMachine.PlayerController.InputManager.IsSwing > 0 && _stateMachine.PlayerController.AimAssist.IsScuccsesTarget)
        {
            _stateMachine.TransitionTo(_stateMachine.AttackState);
            return;
        }   //攻撃

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        //if (_stateMachine.PlayerController.Grapple.SearchGrapplePoint())
        //{
        //    //左トリガーが離れるか0になった時
        //    if (_stateMachine.PlayerController.InputManager.IsSwing > 0)
        //    {
        //        _stateMachine.TransitionTo(_stateMachine.StateGrapple);
        //    }
        //}   //ワイヤーが当たり、Grapple可能


        //ダメージ
        if (_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }

        //Event発生
        if (_stateMachine.PlayerController.IsEvent)
        {
            _stateMachine.TransitionTo(_stateMachine.EventState);
            return;
        }


    }
}
