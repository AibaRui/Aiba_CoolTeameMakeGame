using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetUp
{
    [Header("カメラのPriority")]
    [SerializeField] private int _cameraPriority = 30;

    [SerializeField]
    private float _count = 0.5f;

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

    }

    public void SetEnd()
    {

    }



}
