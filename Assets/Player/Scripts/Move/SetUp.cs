using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetUp
{
    [Header("落下速度")]
    [SerializeField] private float _fallSpeed = -2f;

    [Header("落下が遅くなる最大時間")]
    [SerializeField] private float _fallSpeedDownTime = 2f;


    [Header("カメラのPriority")]
    [SerializeField] private int _cameraPriority = 30;

    [SerializeField] private float _count = 0.5f;

    private float _countFallSpeedDownTime = 0;

    /// <summary>落下速度低下を使用できるかどうか</summary>
    private bool _isCanDownFallSpeedDown = true;


    private float _countTime = 0;

    private bool _isEndCameraTransition;

    public bool IsEndCameraTransition => _isEndCameraTransition;


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void EnterSetUp()
    {
        _playerControl.AnimControl.SetUpSetBool(true);

        _playerControl.VelocityLimit.SetLimit(10, 10, -10, 10);

        //コントローラーを振動させる
        //  _playerControl.ControllerVibrationManager.StartVibration(VivrationPower.SetUp);
    }

    public void ExitSetUp()
    {
        _playerControl.AnimControl.SetUpSetBool(false);

        //コントローラーを振動させる
        _playerControl.ControllerVibrationManager.StopVibration();

        //タイムスケールを戻す
        Time.timeScale = 1f;

        _playerControl.CameraBrain.m_IgnoreTimeScale = false;

        _playerControl.LineRenderer.positionCount = 0;

        Quaternion r = _playerControl.PlayerT.rotation;
        r.x = 0;
        r.z = 0;

        _playerControl.PlayerT.rotation = r;

        //準備時間計測用のタイマーをリセット
        _countTime = 0;

        _isEndCameraTransition = false;

        if (_countFallSpeedDownTime > _fallSpeedDownTime)
        {
            _isCanDownFallSpeedDown = false;
            _countFallSpeedDownTime = 0;
        }   //時間いっぱい、落下速度低下だったら次は強制的に速度低下不可
        else
        {
            _isCanDownFallSpeedDown = !_isCanDownFallSpeedDown;
        }   //可能不可能を入れ替える
    }

    public void FallSpeedDown()
    {
        if (_isCanDownFallSpeedDown)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, _fallSpeed, _playerControl.Rb.velocity.z);
        }
    }

    public void SetUpCamera()
    {
        // Time.timeScale = 0.3f;
        _playerControl.CameraBrain.m_IgnoreTimeScale = true;
    }

    public void SetUping()
    {
        //  Time.timeScale = 0.3f;
        _playerControl.PlayerT.transform.forward = _playerControl.CameraGrapple.transform.forward;

        if (_countTime > _count)
        {
            //カメラの推移が完了
            _isEndCameraTransition = true;
        }
        else
        {
            _countTime += Time.unscaledDeltaTime;
        }

        if (_isCanDownFallSpeedDown)
        {
            _countFallSpeedDownTime += Time.deltaTime;

            if (_countFallSpeedDownTime > _fallSpeedDownTime)
            {
                _isCanDownFallSpeedDown = false;
            }

        }
    }

    public void SetEnd()
    {

    }



}
