using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRunStepCheck
{
    [Header("�ړ����x")]
    [SerializeField] private float _moveSpeed = 7;

    [Header("�i���̌��o����")]
    [SerializeField] private float _checkLong;

    [Header("�i���傫���̂̌��o����")]
    [SerializeField] private float _checkStepHigh = 2;

    [SerializeField] private Transform _center;

    [SerializeField] private LayerMask _wallLayer;

    /// <summary>�o���i���Ɉڍs�������ǂ���</summary>
    private bool _isHitStep = false;

    /// <summary>�ړ��ꏊ</summary>
    private Vector3 _targetDir;

    /// <summary>�i����o�������܂ňړ��ł������ǂ���</summary>
    private bool _isEndTargetPositionMove = false;

    private bool _isCompletedMove = false;

    private Vector3 _isEndPos;

    private float _dis;

    public bool IsHitStep => _isHitStep;

    public bool IsCompletedMove => _isCompletedMove;

    protected PlayerControl _playerControl = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void EndStep()
    {
        _isCompletedMove = false;
    }

    public void Move()
    {
        Debug.DrawRay(_playerControl.PlayerT.position, _targetDir * 10, Color.blue);

        //�o�������܂ōs���Ă��Ȃ�������o��
        if (!_isEndTargetPositionMove)
        {
            _playerControl.Rb.velocity = _targetDir.normalized * _moveSpeed;

            Vector3 startPos = _center.position + -_playerControl.WallRunCheck.WallDir * 0.7f;

            bool no = Physics.Raycast(startPos, _playerControl.WallRun.UseMoveDir.normalized,_dis,_wallLayer);

            Debug.DrawRay(startPos, _playerControl.WallRun.UseMoveDir*10, Color.yellow);

            if (!no)
            {
                _isEndTargetPositionMove = true;
                _isEndPos = _center.position;
                //_playerControl.Rb.velocity = Vector3.zero;
            }
        }
        //�o�������܂ōs���Ă����班��������
        else
        {
            _playerControl.Rb.velocity = _playerControl.WallRun.UseMoveDir * _playerControl.WallRun.MoveSpeed;

            Debug.Log("NNNN");

            if (Vector3.Distance(_center.position, _isEndPos) > 1f)
            {
                _isEndTargetPositionMove = false;
                _isHitStep = false;

                _isCompletedMove = true;

            }
        }
    }


    /// <summary>WallRun�̒i�������邩�ǂ��������o����</summary>
    public void CheckWallStep()
    {
        if (_isHitStep) return;

        Vector3 startPos = _playerControl.WallRunCheck.Hit.point + _playerControl.WallRunCheck.WallDir * 0.1f;
        Vector3 dir = _playerControl.WallRun.UseMoveDir;

        RaycastHit hit;

        bool isHit = Physics.Raycast(startPos, dir, out hit, _checkLong, _wallLayer);

        Debug.DrawRay(startPos, dir * _checkLong, Color.white);

        if (isHit)
        {
            float addLong = 0;

            while (true)
            {
                addLong += 0.1f;

                if (_checkStepHigh < addLong)
                {
                    return;
                }

                Vector3 add = _playerControl.WallRunCheck.WallDir * addLong;

                Vector3 start = _playerControl.WallRunCheck.Hit.point + (-_playerControl.WallRunCheck.WallDir * 0.1f) + add;
                Vector3 dirction = _playerControl.WallRun.UseMoveDir;

                bool isNoHitPos = Physics.Raycast(start, dirction, _checkLong, _wallLayer);

                Debug.DrawRay(start, dirction * _checkLong, Color.yellow);

                if (!isNoHitPos)
                {
                    Vector3 foward = hit.point - _playerControl.PlayerT.position;

                    _targetDir = foward + _playerControl.WallRunCheck.WallDir * (addLong + 0.5f);

                    _dis = Vector3.Distance(_playerControl.PlayerT.position, hit.point);

                    _isHitStep = true;

                    return;
                }
            }
        }
    }


}