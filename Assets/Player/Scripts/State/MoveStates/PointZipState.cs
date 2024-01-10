using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PointZipState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.VelocityLimit.SetLimit(30, 40, -30, 30);

        _stateMachine.PlayerController.PointZip.StartPointZip();
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.PointZip.PointZipMove.Move();
        _stateMachine.PlayerController.PointZip.PointZipMove.SetRotation();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.CameraControl.ZipCameraControl.ZipCamera();
    }

    public override void Update()
    {
        _stateMachine.PlayerController.PointZip.CountWaitTime();
        _stateMachine.PlayerController.PointZip.JumpCount();

        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();

        if (_stateMachine.PlayerController.PointZip.IsEndPointZip)
        {
            //Swingのカメラの値のリセット
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            return;
        }

    }
}
