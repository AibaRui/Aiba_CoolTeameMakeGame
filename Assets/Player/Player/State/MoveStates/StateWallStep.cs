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
        //�~�܂��Ă���Ƃ��ɁA�J�����̒l��Idle��Ԃ̒l�ɕύX����܂ł̎��Ԃ̌v���B�̒l�����Z�b�g
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
        //�e����̃N�[���^�C��
        _stateMachine.PlayerController.CoolTimes();


        if (_stateMachine.PlayerController.WallRunStep.IsCompletedMove)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWallRun);
            return;
        }

    }
}