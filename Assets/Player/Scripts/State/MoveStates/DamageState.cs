using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageState : PlayerStateBase
{
    public override void Enter()
    {
        //_stateMachine.PlayerController.VantanConnectNexusEvent.KnockWindow();
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.AimAssist.LockOnUIOnOff(true);
    }

    public override void FixedUpdate()
    {
        //ダメージ処理　
        _stateMachine.PlayerController.PlayerDamage.FixedDamge();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.PlayerDamage.Latepdata();
    }

    public override void Update()
    {


        //ダメージを受け終えたらステート移行
        if(!_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);
        }
    }
}
