using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class PointZip
{
    [Header("�ړ��ݒ�---")]
    [SerializeField] private PointZipMove _zipMove;
    [Header("�T�m---")]
    [SerializeField] private PointZipSearch _pointZipSearch;
    [Header("UI---")]
    [SerializeField] private PointZipUI _pointZipUI;

    [Header("�ړ��J�n�܂ł̑ҋ@����")]
    [SerializeField] private float _waitTime = 1f;

    [Header("�Ō�̃W�����v�܂ł̑ҋ@����")]
    [SerializeField] private float _waitLastJumpTime = 1f;

    [Header("���C���[�̕`�摬�x")]
    [SerializeField] private float _drowWireSpeed = 1f;

    private float _countWaitTime = 0;

    private float _countLastJumpWaitTime = 0;

    private bool _isEndPointZip = false;

    private bool _isEndWaitTime = false;

    private bool _isHitSearch = false;

    public bool IsHitSearch => _isHitSearch;

    //LineRender�Ɋւ���ݒ�
    private float _setWireLongPercent = 0;
    private bool _isDrowWire = false;


    private PlayerControl _playerControl = null;

    public PointZipUI PointZipUI => _pointZipUI;
    public bool IsEndPointZip => _isEndPointZip;
    public PointZipMove PointZipMove => _zipMove;
    public bool IsEndWaitTime => _isEndWaitTime;

    public PointZipSearch PointZipSearch => _pointZipSearch;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _pointZipSearch.Init(playerControl);
        _zipMove.Init(playerControl);
        _pointZipUI.Init(playerControl);
    }


    public bool Search()
    {
        Time.timeScale = 1f;
        //LeftTrigger�������Ă�����
        if (_playerControl.InputManager.LeftTrigger)
        {
            Time.timeScale = 0.5f;

            _isHitSearch = _pointZipSearch.SearchPointPointZip();

            if (_isHitSearch)
            {
                if (_playerControl.InputManager.RightTrigger)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>PointZip�J�n���Ɏ��s</summary>
    public void StartPointZip()
    {
        Time.timeScale = 1f;

        //PointZip��UI
        _pointZipUI.SetPointZipUI(false);

        //PointZip�p�̃J�������g��
        _playerControl.CameraControl.UseCanera(CameraType.PointCamera);

        //Animator�ݒ�
        if (_pointZipSearch.MoveTargetPositin.y - _playerControl.PlayerT.position.y >= 0)
        {
            _playerControl.AnimControl.IsSetPointZipUp(true);
        }
        else
        {
            _playerControl.AnimControl.IsSetPointZipUp(false);
        }

        //�A�j���[�V�������Đ�
        _playerControl.AnimControl.IsPointZip();

        //�󒆂Ɉꎞ�Ƃǂ߂�
        _playerControl.Rb.velocity = Vector3.zero;
        _playerControl.Rb.useGravity = false;

        _countWaitTime = 0;
        _isEndWaitTime = false;

        _countLastJumpWaitTime = 0;
        _isEndPointZip = false;

        //LineRender�Ɋւ���ݒ�
        _playerControl.LineRenderer.positionCount = 2;
        _setWireLongPercent = 0;
        _isDrowWire = false;

        //�ړ����̏����ݒ�
        _zipMove.StartSet();
    }

    public void StopPointZip()
    {
        _playerControl.Rb.useGravity = true;
    }


    /// <summary>�ҋ@���Ԃ��v�� </summary>
    public void CountWaitTime()
    {
        if (_isEndWaitTime) return;

        if (_countWaitTime >= _waitTime)
        {
            _isEndWaitTime = true;

            _isDrowWire = true;
        }
        else
        {
            _countWaitTime += Time.deltaTime;
        }
    }


    public void JumpCount()
    {
        if (!_zipMove.IsMoveEnd) return;

        if (_countLastJumpWaitTime >= _waitLastJumpTime)
        {
            _playerControl.LineRenderer.positionCount = 0;

            _isEndPointZip = true;
            _zipMove.Lastjump();
            _playerControl.AnimControl.IsPointZipJump();
        }
        else
        {
            _countLastJumpWaitTime += Time.deltaTime;
        }
    }


    /// <summary>���C���[��`�悷��</summary>
    public void DrowWire()
    {
        if (!_isDrowWire)
        {
            return;
        }

        _setWireLongPercent += _drowWireSpeed * Time.deltaTime;
        if (_setWireLongPercent > 1)
        {
            _setWireLongPercent = 1;
        }

        float step = Vector3.Distance(_playerControl.Hads.position, _pointZipSearch.RayHitPoint) * _setWireLongPercent;
        Vector3 setPoint = Vector3.MoveTowards(_playerControl.Hads.position, _pointZipSearch.RayHitPoint, step);

        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, setPoint);
    }
}