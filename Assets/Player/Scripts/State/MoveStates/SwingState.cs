using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwingState : PlayerStateBase
{
    public override void Enter()
    {
        //���x�ݒ�
        _stateMachine.PlayerController.Swing.SetSpeedSwing();
        //Swing�̏����ݒ�
        _stateMachine.PlayerController.Swing.SwingSetting();
        //�A�j���[�V�����̐ݒ�
        _stateMachine.PlayerController.AnimControl.SwingAnim.Swing(true);

        //���C���[���΂���
        _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.WireFireSounds();

        //���[�v�̕`��ݒ�
        _stateMachine.PlayerController.Swing.SwingJointSetting.FirstSettingDrawLope();

        //���̉��̐ݒ�
        _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(true);
    }

    public override void Exit()
    {
        //�A�j���[�V�����̐ݒ�
        _stateMachine.PlayerController.AnimControl.SwingAnim.Swing(false);
        _stateMachine.PlayerController.AnimControl.SwingAnim.SetHighFallType();

        //Swing�I�����̃J�����̍Đݒ�
        _stateMachine.PlayerController.CameraControl.SwingEndSetCamera();

        if (_stateMachine.PlayerController.PlayerT.eulerAngles.y > 180)
        {
            _stateMachine.PlayerController.CameraControl.SwingEndPlayerRotateY = _stateMachine.PlayerController.PlayerT.eulerAngles.y - 360;
        }
        else
        {
            _stateMachine.PlayerController.CameraControl.SwingEndPlayerRotateY = _stateMachine.PlayerController.PlayerT.eulerAngles.y;
        }
    }

    public override void FixedUpdate()
    {
        //Swing���̉���
        _stateMachine.PlayerController.Swing.AddSpeed();

        //Swing���̉�]
        _stateMachine.PlayerController.Swing.SwingRotation();
    }

    public override void LateUpdate()
    {
        _stateMachine.PlayerController.Swing.SwingJointSetting.DrowWire();


        //�J�����̉�]���x���v�Z����
        _stateMachine.PlayerController.CameraControl.CountTime();

        //�J�������X����
        //_stateMachine.PlayerController.CameraControl.SwingCameraYValues(_stateMachine.PlayerController.Rb.velocity.y, 20, -20, 20f);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraDistance(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraOffsetY(_stateMachine.PlayerController.Rb.velocity.y);
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetSwingCameraVerticalAxis(_stateMachine.PlayerController.Rb.velocity.y);

        //FOV�ݒ�
        _stateMachine.PlayerController.CameraControl.SwingCameraControl.ChangeFOV(true, _stateMachine.PlayerController.Rb.velocity.y);

        //�J�������X����BX��
        _stateMachine.PlayerController.CameraControl.SwingCameraValueX(true);

    }

    public override void Update()
    {
        //�e����̃N�[���^�C�����v��
        _stateMachine.PlayerController.CoolTimes();

        //���C���[��`�悷��܂ł̎��Ԃ��v��
        _stateMachine.PlayerController.Swing.SwingJointSetting.CountDrowWireTime();

        //�ǂ�����������AWallRun��Ԃ�
        if (_stateMachine.PlayerController.WallRunCheck.CheckWalAlll())
        {
            //FrontZip�����s�\�ɂ���
            _stateMachine.PlayerController.ZipMove.SetCanZip();

            if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0 || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
            {

                // _stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            }    //WallRun�ֈڍs
            else
            {
                _stateMachine.PlayerController.WallRun.SetNoMove(true);
                //_stateMachine.TransitionTo(_stateMachine.StateWallIdle);
            }

            //Swing�̃J�����̒l�̃��Z�b�g
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.ResetValues();

            _stateMachine.PlayerController.CameraControl.WallRunCameraControl.WallRunEndCamera();

            _stateMachine.TransitionTo(_stateMachine.StateWallRun);

            _stateMachine.PlayerController.AnimControl.WallRunTransition();

            _stateMachine.PlayerController.Swing.StopSwing(false);

            //���̉��̐ݒ�
            _stateMachine.PlayerController.PlayerAudioManager.LoopAudio.PlayWindAudio(false);

            return;
        }


        //�A���J�[�̒��n�_��荂���オ������
        if (_stateMachine.PlayerController.Swing.CheckLine())
        {
            //FrontZip�����s�\�ɂ���
            _stateMachine.PlayerController.ZipMove.SetCanZip();

            //�A�j���[�V�����ݒ�(Swing�I���W�����v�̃^�C�v����)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(1);

            //�A�j���[�V�����̐ݒ�
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingHighEnd();

            //�W�����v��
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.UpJumpSounds();
            //�}���g�̉�
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //�W�����v����
            _stateMachine.PlayerController.Swing.LastJumpUp();

            //�W�����v���ďI���
            _stateMachine.PlayerController.Swing.StopSwing(true);

            //�W�����v���ďI���
            _stateMachine.PlayerController.Move.IsUseSpeedDash();

            //�X�s�[�h�̉����̐ݒ�
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //�J�����̒Ǐ]���n�߂�
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //�㏸���ďI��
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(true, false, _stateMachine.PlayerController.Rb.velocity.y);

            //���ځB(Y���x�ɂ���Đ��ʐ��ς���)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);

            return;
        }



        //Swing�̃{�^���𗣂�����
        if (_stateMachine.PlayerController.InputManager.IsSwing < 0.6f)
        {
            //�A�j���[�V�����ݒ�(Swing�I���W�����v�̃^�C�v����)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(0);

            //�W�����v��
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.SwingEndSounds();
            //�}���g�̉�
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //�W�����v���Ȃ��ŏI���
            _stateMachine.PlayerController.Swing.StopSwing(false);

            //�J�����̒Ǐ]���n�߂�
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //�X�s�[�h�̉����̐ݒ�
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(false, false, _stateMachine.PlayerController.Rb.velocity.y);

            //����
            if (_stateMachine.PlayerController.GroundCheck.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
            }   //�n�ʂɂ��ĂĂ���=>Idle
            else
            {
                //���ځB(Y���x�ɂ���Đ��ʐ��ς���)
                if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
                else _stateMachine.TransitionTo(_stateMachine.StateDownAir);
            }   //�󒆂�������=>_Air
            return;
        }

        //Swing���ɃW�����v��������
        if (_stateMachine.PlayerController.InputManager.IsJumping && _stateMachine.PlayerController.Swing.IsDown)
        {
            //FrontZip�����s�\�ɂ���
            _stateMachine.PlayerController.ZipMove.SetCanZip();

            //�A�j���[�V�����ݒ�(Swing�I���W�����v�̃^�C�v����)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(2);
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetJumpEndType();

            //�W�����v��
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.FrontJumpSounds();
            //�}���g�̉�
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //_stateMachine.PlayerController.CameraControl.SwingEndTranspectorUp();

            //�W�����v����
            _stateMachine.PlayerController.Swing.LastJumpFront();

            //�W�����v���ďI���
            _stateMachine.PlayerController.Swing.StopSwing(true);

            //�󒆂őO���ɉ�������A�Ƃ������Ƃ�`����
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //�J�����̒Ǐ]���n�߂�
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //�㏸���ďI��
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(false, true, _stateMachine.PlayerController.Rb.velocity.y);

            //���ځB(Y���x�ɂ���Đ��ʐ��ς���)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);
        }

        //Swing���ɃW�����v��������
        if (_stateMachine.PlayerController.InputManager.IsJumping && !_stateMachine.PlayerController.Swing.IsDown)
        {
            //FrontZip�����s�\�ɂ���
            _stateMachine.PlayerController.ZipMove.SetCanZip();

            //�A�j���[�V�����ݒ�(Swing�I���W�����v�̃^�C�v����)
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetSwingEndType(2);
            _stateMachine.PlayerController.AnimControl.SwingAnim.SetJumpEndType();

            //�W�����v��
            _stateMachine.PlayerController.PlayerAudioManager.SwingAudio.FrontJumpSounds();
            //�}���g�̉�
            _stateMachine.PlayerController.PlayerAudioManager.MantAudio.PlayMant();

            //_stateMachine.PlayerController.CameraControl.SwingEndTranspectorUp();

            //�W�����v����
            _stateMachine.PlayerController.Swing.LastJump();

            //�W�����v���ďI���
            _stateMachine.PlayerController.Swing.StopSwing(true);

            //�󒆂őO���ɉ�������A�Ƃ������Ƃ�`����
            _stateMachine.PlayerController.VelocityLimit.DoSpeedUp();

            //�J�����̒Ǐ]���n�߂�
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.EndFollow();

            //�㏸���ďI��
            _stateMachine.PlayerController.CameraControl.SwingCameraControl.SetUpEndOffSet(false, true, _stateMachine.PlayerController.Rb.velocity.y);

            //���ځB(Y���x�ɂ���Đ��ʐ��ς���)
            if (_stateMachine.PlayerController.Rb.velocity.y > 0) _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            else _stateMachine.TransitionTo(_stateMachine.StateDownAir);
        }
    }
}