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

    public bool IsEndCameraTransition  =>_isEndCameraTransition;


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }



    public void SetUpCamera()
    {
        Time.timeScale = 0.3f;
        _playerControl.CameraGrapple.Priority = _cameraPriority;
        _playerControl.CameraBrain.m_IgnoreTimeScale = true;
    }

    public void SetUping()
    {
        Time.timeScale = 0.3f;
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
            //タイムスケールを戻す
            Time.timeScale = 1f;

            _playerControl.CameraBrain.m_IgnoreTimeScale = false;

            //GrappleのカメラのPriortyを下げる
            _playerControl.CameraGrapple.Priority = -10;

            _playerControl.LineRenderer.positionCount = 0;

            Quaternion r = _playerControl.PlayerT.rotation;
            r.x = 0;
            r.z = 0;

            _playerControl.PlayerT.rotation = r;

            //準備時間計測用のタイマーをリセット
            _countTime = 0;

        _isEndCameraTransition = false;
    }


    
}
