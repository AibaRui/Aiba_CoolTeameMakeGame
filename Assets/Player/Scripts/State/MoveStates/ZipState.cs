using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Rb.useGravity = false;

        //Zip�̏����ݒ�
        _stateMachine.PlayerController.ZipMove.ZipFirstSetting();

        //LineRenderer�ݒ�
        _stateMachine.PlayerController.ZipLineRenderer.StartZipLine();

        //�A�j���[�V�����̐ݒ�
        _stateMachine.PlayerController.AnimControl.ZipAnim.FrontZip();
        _stateMachine.PlayerController.AnimControl.ZipAnim.SetZip(true);

        _stateMachine.PlayerController.EffectControl.ZipSet(true);
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Rb.useGravity = true;

        //�A�j���[�V�����ݒ�
        _stateMachine.PlayerController.AnimControl.ZipAnim.SetZip(false);

        //FrontZip�̃^�C�}�[�����Z�b�g
        _stateMachine.PlayerController.ZipMove.EndZip();

        //LineRenderer�ݒ�
        _stateMachine.PlayerController.ZipLineRenderer.ResetZipLine();

        _stateMachine.PlayerController.EffectControl.ZipSet(false);
    }

    public override void FixedUpdate()
    {
        //�v���C���[�̊p�x��ύX
        _stateMachine.PlayerController.ZipMove.ZipMove.ZipSetPlayerRotation();
        _stateMachine.PlayerController.ZipLineRenderer.MedalPosition();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.CameraControl.ZipCameraControl.ZipCamera();

        //LineRenderer�ݒ�
        _stateMachine.PlayerController.ZipLineRenderer.SetZipLineWave();
    }

    public override void Update()
    {
        //Zip�̗L�����Ԃ��v��
        _stateMachine.PlayerController.ZipMove.CountFrotZipTime();

        if (_stateMachine.PlayerController.GroundCheck.IsHit())
        {
            //Swing�̃J�����̒l�̃��Z�b�g
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }

        if (_stateMachine.PlayerController.WallRunCheck.CheckWallFront())
        {
            //Swing�̃J�����̒l�̃��Z�b�g
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            return;
        }

        if (_stateMachine.PlayerController.ZipMove.IsEndFrontZip)
        {
            //���ځB(Y���x�ɂ���Đ��ʐ��ς���)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);

            //�󒆂őO���ɉ�������A�Ƃ������Ƃ�`����
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();
        }
    }
}