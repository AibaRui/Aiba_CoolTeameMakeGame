using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossEventState : PlayerStateBase
{
    public override void Enter()
    {
        if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.StartReplace();

            //��ʌ���PostEffect_Off
            _stateMachine.PlayerController.PlayerPostEffectSetting.OffPostEffect();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_HitBoss)
        {
            _stateMachine.PlayerController.PlayerBossHit.EnterBossHit();
        }
    }

    public override void Exit()
    {
        if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_Replace)
        {

        }
        else if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_HitBoss)
        {

        }
    }

    public override void FixedUpdate()
    {


            //�J�������X����
            //_stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);
            //�J�����̉�]���x���v�Z����
            _stateMachine.PlayerController.CameraControl.CountTime();

            //FOV�ݒ�
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(true, _stateMachine.PlayerController.Rb.velocity.y);
        if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.Remove();




        }
        else if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_HitBoss)
        {
            _stateMachine.PlayerController.PlayerBossHit.FixedBossHit();
        }
    }

    public override void LateUpdate()
    {
        if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.ReplaceLateUpddata();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_HitBoss)
        {


        }
    }

    public override void Update()
    {
        if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_Replace)
        {
            if (!_stateMachine.PlayerController.PlayerReplace.IsRemove)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }

        }
        else if (_stateMachine.PlayerController.EventType == PlayerBossEventType.BossStage_HitBoss)
        {
            if(!_stateMachine.PlayerController.PlayerBossHit.IsHitBoss)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }
        }
    }
}