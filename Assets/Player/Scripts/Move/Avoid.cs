using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Avoid
{
    [SerializeField] private Vector3 _speedLimit = new Vector3(20,20,20);


    [SerializeField] private float _avoidSpeed = 5;

    [SerializeField] private float _coolTime = 2;

    [SerializeField] private float _avoidTime = 2;

    private float _nowAvoidTime = 0;

    private float _countCoolTime = 0;

    private float _countAvoidTime = 0;

    private bool _isCanAvoid = true;

    private bool _isEndAvoid = false;

    private Vector3 _avoidDir = default;

    public bool IsEndAvoid => _isEndAvoid;
    public bool IsCanAvoid => _isCanAvoid;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetSpeedLimit()
    {
        _playerControl.VelocityLimit.SetLimit(_speedLimit.x, _speedLimit.y, _speedLimit.z);
    }

    /// <summary>回避のクールタイムを数える</summary>
    public void CountCoolTimeAvoid()
    {
        if (_isCanAvoid) return;

        _countCoolTime += Time.deltaTime;

        if (_countCoolTime >= _coolTime)
        {
            _isCanAvoid = true;
            _countCoolTime = 0;
        }
    }

    public void SetAvoidDir()
    {
        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        if (h != 0 || v != 0)
        {
            _avoidDir = horizontalRotation * new Vector3(h, 0, v).normalized;
        }
        else
        {
            _avoidDir = horizontalRotation * new Vector3(0, 0, 1).normalized;
        }

        Quaternion _targetRotation = Quaternion.LookRotation(_avoidDir, Vector3.up);
        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, 360);

        _isEndAvoid = false;

        _nowAvoidTime = _avoidTime;
    }


    /// <summary>回避の速度設定</summary>
    public void DoAvoid()
    {
        if (!_isEndAvoid)
        {
            _playerControl.Rb.velocity = _avoidDir * _avoidSpeed;

            _countAvoidTime += Time.deltaTime;

            if (_countAvoidTime >= _avoidTime || _isEndAvoid)
            {
                _isEndAvoid = true;
                return;
            }
        }
    }

    public void EndAvoid()
    {
        _isCanAvoid = false;

        _countAvoidTime = 0;

        _playerControl.Rb.velocity = _avoidDir * _avoidSpeed/5;
    }


    void Update()
    {

    }
}
