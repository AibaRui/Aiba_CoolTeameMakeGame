using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Zip
{
    [Header("=====Zip�̈ړ��ݒ�=====")]
    [SerializeField] private ZipMove _zipMove;

    [Header("FrontZip�̉񐔐���")]
    [SerializeField] private float _frontZipDoMaxCount = 2;

    [Header("FrontZip�̎��s����܂ł̎���")]
    [SerializeField] private float _frontZipWaitTime = 0.4f;

    [Header("FrontZip�̎��s����")]
    [SerializeField] private float _frontZipTime = 1;

    [Header("���x����")]
    [SerializeField] private Vector3 _limitSpeed = new Vector3(20, 20, 20);


    private float _frontZipTimeCount = 0;

    private float _frontZipWaitTimeCount = 0;

    private bool _isEndFrontZip = false;

    private int _frontZipDoCount = 0;

    private bool _isZip = false;

    private bool _isCanZip = false;

    public bool IsEndFrontZip => _isEndFrontZip;

    public bool IsCanZip => _isCanZip;
    public ZipMove ZipMove => _zipMove;

    Quaternion targetRotation;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _zipMove.Init(this, playerControl);
    }


    /// <summary>�O���ɉ�������</summary>
    public void ZipFirstSetting()
    {
        //���x�ݒ�
        _playerControl.VelocityLimit.SetLimit(_limitSpeed.x, _limitSpeed.y, -10, _limitSpeed.z);

        _playerControl.Rb.velocity = Vector3.zero;

        //Zip�̃R�C����ł�
        _playerControl.PlayerAudioManager.ZipAudio.ZipCoinFire();

        //��]�����̐ݒ�
        _zipMove.SetRotation();
    }


    /// <summary>Zip�̎��s���Ԃ��v������</summary>
    public void CountFrotZipTime()
    {
        if (!_isZip)
        {
            _frontZipWaitTimeCount += Time.deltaTime;

            if (_frontZipWaitTimeCount >= _frontZipWaitTime)
            {
                _isZip = true;
                _playerControl.AnimControl.ZipAnim.SetDoZip(true);
                _playerControl.PlayerAudioManager.ZipAudio.ZipAudioPlay();
                _playerControl.PlayerAudioManager.MantAudio.PlayMant();
                _zipMove.ZipAddVelocity(_frontZipDoCount);
            }
        }
        else
        {
            _frontZipTimeCount += Time.deltaTime;

            if (_frontZipTimeCount >= _frontZipTime)
            {
                _isEndFrontZip = true;
            }
        }
    }



    /// <summary>Zip���I��������̏���</summary>
    public void EndZip()
    {
        //Zip�I����Bool������Zip�ׂ̈Ƀ��Z�b�g
        _isEndFrontZip = false;

        _isZip = false;

        _frontZipWaitTimeCount = 0;

        //Zip���s���Ԍv���p�̃^�C�}�[�����Z�b�g
        _frontZipTimeCount = 0;

        //�񐔂𑝉�
        _frontZipDoCount++;

        if (_frontZipDoCount <= _frontZipDoMaxCount)
        {
            _isCanZip = false;
        }

        _playerControl.AnimControl.ZipAnim.SetDoZip(false);
    }


    /// <summary>Zip�̎��s���\�ɂ���</summary>
    public void SetCanZip()
    {
        //Zip�̎��s�񐔂����Z�b�g
        _frontZipDoCount = 0;
        //Zip�����s�\�ɂ���
        _isCanZip = true;
    }
}