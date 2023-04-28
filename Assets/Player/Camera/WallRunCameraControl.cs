using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WallRunCameraControl : MonoBehaviour
{
    [Header("上向きにする時速さ")]
    [SerializeField] private float _upperMaxSpeed = 1;

    [Header("左右に向かせる速さ")]
    [SerializeField] private float _horizontalMaxSpeed = 40;

    [Header("止まっているときのカメラの距離")]
    [SerializeField] private float _idleCameraDistance = 5;

    [Header("動いているときのカメラの距離")]
    [SerializeField] private float _moveCameraDistance = 7;

    [Header("止まってからカメラを戻すまでの時間")]
    [SerializeField] private float _reSetCameraDistanceTime = 3;

    [Header("止まってからカメラの距離を変更する速度")]
    [SerializeField] private float _reSetCameraDistanceSpeed = 10;

    [Header("動いてからカメラの距離を変更する速度")]
    [SerializeField] private float _setCameraDistanceSpeed = 10;

    private float _reSetCameraDistanceTimeCount = 0;

    [SerializeField] private CameraControl _cameraControl;

    private float _upperSpeed = 0;

    private CinemachinePOV _swingCinemachinePOV;
    private CinemachineFramingTransposer _swingCameraFraming;

    void Start()
    {
        _swingCinemachinePOV = _cameraControl.WallRunCameraController.GetCinemachineComponent<CinemachinePOV>();
        _swingCameraFraming = _cameraControl.WallRunCameraController.GetCinemachineComponent<CinemachineFramingTransposer>();
    }



    /// <summary>Idle時、カメラを元に戻す</summary>
    public void WallIdleCamera()
    {
        if (_reSetCameraDistanceTimeCount >= _reSetCameraDistanceTime)
        {
            if(_idleCameraDistance<_swingCameraFraming.m_CameraDistance)
            {
                _swingCameraFraming.m_CameraDistance -= Time.deltaTime*_reSetCameraDistanceSpeed;
            }
        }
    }

    /// <summary>Idle時、カメラを元に戻すまでの時間を計測</summary>
    public void CountReSetWallIdleCameraTime()
    {
        if (_reSetCameraDistanceTimeCount < _reSetCameraDistanceTime)
        {
            _reSetCameraDistanceTimeCount += Time.deltaTime;
        }
    }

    /// <summary>Idle時、カメラを元に戻すまでの時間計測の値をリセット</summary>
    public void ResetCountReSetWallIdleCameraTime()
    {
        _reSetCameraDistanceTimeCount = 0;
    }



    public void WallRunCameraFollow()
    {
        Vector3 wallCrossRight = _cameraControl.PlayerControl.WallRunCheck.WallCrossRight;
        Vector3 wallDir = -_cameraControl.PlayerControl.WallRunCheck.WallDir;

        Vector3 lookDirection = default;

        if (_cameraControl.IsDontCameraMove)
        {
            if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Right)
            {
                lookDirection = wallCrossRight;
            }
            else if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Left)
            {
                lookDirection = -wallCrossRight;
            }
            else
            {
                lookDirection = wallDir;
            }

            //カメラを向かせたい方向の回転
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            float targetAngle = lookRotation.eulerAngles.y;

            //現在の角度
            float currentAngle = _swingCinemachinePOV.m_HorizontalAxis.Value;

            //新しい角度を作成
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _horizontalMaxSpeed * Time.deltaTime);

            //現在の角度と、向かせたい角度の差が一定値に収まるまで更新
            if (Mathf.Abs(lookRotation.eulerAngles.y - _swingCinemachinePOV.m_HorizontalAxis.Value) > 0.5f)
            {
                _swingCinemachinePOV.m_HorizontalAxis.Value = newAngle;
            }

            //縦方向の回転の設定
            if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Up)
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value > -60)
                {
                    _swingCinemachinePOV.m_VerticalAxis.Value -= _upperMaxSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value < 25)
                {
                    _swingCinemachinePOV.m_VerticalAxis.Value += _upperMaxSpeed * Time.deltaTime;
                }
            }
        }

        //カメラの距離の設定
        if (_swingCameraFraming.m_CameraDistance < _moveCameraDistance)
        {
            _swingCameraFraming.m_CameraDistance += Time.deltaTime * _setCameraDistanceSpeed;
        }
    }
}
