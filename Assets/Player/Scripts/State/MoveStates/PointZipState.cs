using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PointZipState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.VelocityLimit.SetLimit(50, 50, -30, 50);

        _stateMachine.PlayerController.PointZip.StartPointZip();
    }

    public override void Exit()
    {
        //上昇して終了
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(true, false, _stateMachine.PlayerController.Rb.velocity.y);

        _stateMachine.PlayerController.PointZip.StopPointZip();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.PointZip.PointZipMove.Move();
        _stateMachine.PlayerController.PointZip.PointZipMove.SetRotation();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.CameraControl.ZipCameraControl.ZipCamera();
        _stateMachine.PlayerController.CameraControl.PointZipCameraControl.PointCameraDistanceShorting();

        _stateMachine.PlayerController.PointZip.DrowWire();
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
