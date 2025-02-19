using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventState : PlayerStateBase
{
    public override void Enter()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossMovie)
        {
            _stateMachine.PlayerController.BossMovie.MovieStart();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.StartReplace();

            //âÊñ å¯â PostEffect_Off
            _stateMachine.PlayerController.PlayerPostEffectSetting.OffPostEffect();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_HitBoss)
        {
            _stateMachine.PlayerController.PlayerBossHit.EnterBossHit();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.Movie)
        {

        }
    }

    public override void Exit()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossMovie)
        {
            _stateMachine.PlayerController.BossMovie.ExitState();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {

        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_HitBoss)
        {

        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.Movie)
        {
            _stateMachine.PlayerController.Anim.Play("AvoidGroundFront");
        }
    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossMovie)
        {

        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.Remove();
            //ÉJÉÅÉâÇåXÇØÇÈ
            //_stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);
            //ÉJÉÅÉâÇÃâÒì]ë¨ìxÇåvéZÇ∑ÇÈ
            _stateMachine.PlayerController.CameraControl.CountTime();

            //FOVê›íË
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(true, _stateMachine.PlayerController.Rb.velocity.y);
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_HitBoss)
        {
            _stateMachine.PlayerController.PlayerBossHit.FixedBossHit();
            //ÉJÉÅÉâÇåXÇØÇÈ
            //_stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);
            //ÉJÉÅÉâÇÃâÒì]ë¨ìxÇåvéZÇ∑ÇÈ
            _stateMachine.PlayerController.CameraControl.CountTime();

            //FOVê›íË
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(true, _stateMachine.PlayerController.Rb.velocity.y);
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.Movie)
        {

        }
    }

    public override void LateUpdate()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossMovie)
        {

        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.ReplaceLateUpddata();
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_HitBoss)
        {


        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.Movie)
        {

        }
    }

    public override void Update()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossMovie)
        {
            if (_stateMachine.PlayerController.BossMovie.IsEndMovie && _stateMachine.PlayerController.Rb.velocity.y < -3)
            {
                _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            if (!_stateMachine.PlayerController.PlayerReplace.IsRemove)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }

        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_HitBoss)
        {
            if (!_stateMachine.PlayerController.PlayerBossHit.IsHitBoss)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }
        }
        else if (_stateMachine.PlayerController.EventType == PlayerEventType.Movie)
        {
            if(!_stateMachine.PlayerController.IsEvent)
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }
        }
    }
}
