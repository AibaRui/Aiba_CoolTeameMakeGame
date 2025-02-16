using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateWallStep : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.AnimControl.WallRunStep(true);
        Debug.Log("Step");
    }

    public override void Exit()
    {
        //止まっているときに、カメラの値をIdle状態の値に変更するまでの時間の計測。の値をリセット
        _stateMachine.PlayerController.CameraControl.WallRunCameraControl.ResetCountReSetWallIdleCameraTime();

        _stateMachine.PlayerController.Move.ReSetTime();

        _stateMachine.PlayerController.WallRunStep.EndStep();

        _stateMachine.PlayerController.AnimControl.WallRunStep(false);
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.WallRunStep.Move();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //各動作のクールタイム
        _stateMachine.PlayerController.CoolTimes();


        if (_stateMachine.PlayerController.WallRunStep.IsCompletedMove)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallRun);
            return;
        }


    }
}
