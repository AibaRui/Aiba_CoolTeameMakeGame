using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRunUpZipState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.WallRunUpZip.UpZipStart();

        _stateMachine.PlayerController.VelocityLimit.SetLimit(25, 50, 25);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.WallRunUpZip.IsZipToFront)
        {
            _stateMachine.PlayerController.WallRunUpZip.DoUpToFrontZip();
        }
        else
        {
            _stateMachine.PlayerController.WallRunUpZip.DoZip();
        }
    }

    public override void LateUpdate()
    {
        if (_stateMachine.PlayerController.WallRunUpZip.IsZipToFront)
        {
            _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallRunZipUpToFrontCameraSet();
        }
    }

    public override void Update()
    {
        bool isHit = _stateMachine.PlayerController.WallRunCheck.CheckHitWall();

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        float v = _stateMachine.PlayerController.InputManager.VerticalInput;

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        //Zipが終わった
        if (_stateMachine.PlayerController.WallRunUpZip.IsEndZip)
        {
            if (_stateMachine.PlayerController.WallRunUpZip.IsZipToFront)
            {
                _stateMachine.PlayerController.Rb.useGravity = true;

                if (_stateMachine.PlayerController.Rb.velocity.y>0)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                }
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateDownAir);
                }
            }
            else
            {
                if (v > 0)
                {
                    _stateMachine.TransitionTo(_stateMachine.StateWallRun);
                }
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
                }
            }
        }
    }
}
