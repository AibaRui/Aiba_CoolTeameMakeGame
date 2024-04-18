using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventState : PlayerStateBase
{
    public override void Enter()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.StartReplace();       
            
            //âÊñ å¯â PostEffect_Off
            _stateMachine.PlayerController.PlayerPostEffectSetting.OffPostEffect();

        }
    }

    public override void Exit()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {

        }
    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.Remove();

            //ÉJÉÅÉâÇÃâÒì]ë¨ìxÇåvéZÇ∑ÇÈ
            _stateMachine.PlayerController.CameraControl.CountTime();

            //ÉJÉÅÉâÇåXÇØÇÈ
            //_stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);

            //FOVê›íË
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(true, _stateMachine.PlayerController.Rb.velocity.y);
        }
    }

    public override void LateUpdate()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            _stateMachine.PlayerController.PlayerReplace.ReplaceLateUpddata();
        }
    }

    public override void Update()
    {
        if (_stateMachine.PlayerController.EventType == PlayerEventType.BossStage_Replace)
        {
            if (!_stateMachine.PlayerController.PlayerReplace.IsRemove)
            {
                _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            }

        }
    }
}
