using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //�_���[�W�����@
        _stateMachine.PlayerController.PlayerDamage.FixedDamge();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.PlayerDamage.Latepdata();
    }

    public override void Update()
    {


        //�_���[�W���󂯏I������X�e�[�g�ڍs
        if(!_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);
        }
    }
}