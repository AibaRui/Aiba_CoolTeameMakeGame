using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class WallRunCameraControl
{
    [Header("上向きにする時速さ")]
    [SerializeField] private float _upperMaxSpeed = 70;

    [Header("左右に向かせる速さ")]
    [SerializeField] private float _horizontalMaxSpeed = 50;

    [Header("止まっているときのカメラの距離")]
    [SerializeField] private float _idleCameraDistance = 5;

    [Header("動いているときのカメラの距離")]
    [SerializeField] private float _moveCameraDistance = 7;

    [Header("止まってからカメラを戻すまでの時間")]
    [SerializeField] private float _reSetCameraDistanceTime = 3;

    [Header("止まってからカメラの距離を変更する速度")]
    [SerializeField] private float _reSetCameraDistanceSpeed = 5;

    [Header("動いてからカメラの距離を変更する速度")]
    [SerializeField] private float _setCameraDistanceSpeed = 10;


    [Header("-----X軸のOffsetの移動についての設定-----")]
    [Header("移動する際のOffSetの変更速度")]
    [SerializeField] private float _offSetXChangeSpeedOnMove = 2f;
    [Header("止まる際のOffSetの変更速度")]
    [SerializeField] private float _offSetXChangeSpeedOnStop = 2f;

    [Header("初期状態のOffSet")]
    [SerializeField] private float _startOffSetX = 0f;
    [Header("壁が左側の時の最大OffSet")]
    [SerializeField] private float _leftWallMaxOffSetX = 3f;
    [Header("壁が右側の時の最大OffSet")]
    [SerializeField] private float _rightWallMaxOffSetX = -3f;

    [Header("-----Y軸のOffsetの移動についての設定-----")]
    [Header("OffSetの変更速度")]
    [SerializeField] private float _offSetYChangeSpeed = 2f;
    [Header("初期値のOffSetY")]
    [SerializeField] private float _defultOffSetY = 1f;
    [Header("移動時のOffSetY")]
    [SerializeField] private float _setOffSetY = 2f;



    private float _reSetCameraDistanceTimeCount = 0;

    private CameraControl _cameraControl;

    private float _upperSpeed = 0;

    private CinemachinePOV _wallRunPOV;
    private CinemachineFramingTransposer _wallRunFraming;

    public void Init(CameraControl cameraControl)
    {
        _cameraControl = cameraControl;

        _wallRunPOV = _cameraControl.SwingCinemachinePOV;
        _wallRunFraming = _cameraControl.SwingCameraFraming;
    }

    /// <summary>Idle時、カメラを元に戻す</summary>
    public void WallIdleCamera()
    {
        if (_reSetCameraDistanceTimeCount >= _reSetCameraDistanceTime)
        {
            if (_idleCameraDistance < _wallRunFraming.m_CameraDistance)
            {
                _wallRunFraming.m_CameraDistance -= Time.deltaTime * _reSetCameraDistanceSpeed;
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

    public void WallRunEndCamera()
    {
        _wallRunFraming.m_TrackedObjectOffset.x = _startOffSetX;
    }

    public void XOffSetWallIdle()
    {
        if (_wallRunFraming.m_TrackedObjectOffset.x > _startOffSetX)
        {
            _wallRunFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offSetXChangeSpeedOnStop;

            if (_wallRunFraming.m_TrackedObjectOffset.x < _startOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x = _startOffSetX;
            }
        }
        else if (_wallRunFraming.m_TrackedObjectOffset.x < _startOffSetX)
        {
            _wallRunFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offSetXChangeSpeedOnStop;

            if (_wallRunFraming.m_TrackedObjectOffset.x > _startOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x = _startOffSetX;
            }
        }
    }

    public void SetOffsetY(bool isMove)
    {
        if (!isMove)
        {
            if (_reSetCameraDistanceTimeCount >= _reSetCameraDistanceTime)
            {
                if (_wallRunFraming.m_TrackedObjectOffset.y > _defultOffSetY)
                {
                    _wallRunFraming.m_TrackedObjectOffset.y -= Time.deltaTime * _offSetYChangeSpeed;

                    if (_wallRunFraming.m_TrackedObjectOffset.y < _defultOffSetY)
                    {
                        _wallRunFraming.m_TrackedObjectOffset.y = _defultOffSetY;
                    }
                }
                else if (_wallRunFraming.m_TrackedObjectOffset.y < _defultOffSetY)
                {
                    _wallRunFraming.m_TrackedObjectOffset.y += Time.deltaTime * _offSetYChangeSpeed;

                    if (_wallRunFraming.m_TrackedObjectOffset.y > _defultOffSetY)
                    {
                        _wallRunFraming.m_TrackedObjectOffset.y = _defultOffSetY;
                    }
                }
            }
        }
        else
        {
            if (_wallRunFraming.m_TrackedObjectOffset.y > _setOffSetY)
            {
                _wallRunFraming.m_TrackedObjectOffset.y -= Time.deltaTime * _offSetYChangeSpeed;

                if (_wallRunFraming.m_TrackedObjectOffset.y < _setOffSetY)
                {
                    _wallRunFraming.m_TrackedObjectOffset.y = _setOffSetY;
                }
            }
            else if (_wallRunFraming.m_TrackedObjectOffset.y < _setOffSetY)
            {
                _wallRunFraming.m_TrackedObjectOffset.y += Time.deltaTime * _offSetYChangeSpeed;

                if (_wallRunFraming.m_TrackedObjectOffset.y > _setOffSetY)
                {
                    _wallRunFraming.m_TrackedObjectOffset.y = _setOffSetY;
                }
            }
        }

    }

    public void XOffSetControlWallRun()
    {
        if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Right)
        {
            if (_wallRunFraming.m_TrackedObjectOffset.x < _leftWallMaxOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offSetXChangeSpeedOnMove;
            }
        }
        else if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Left)
        {
            if (_wallRunFraming.m_TrackedObjectOffset.x > _rightWallMaxOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offSetXChangeSpeedOnMove;
            }
        }
        else
        {
            if (_wallRunFraming.m_TrackedObjectOffset.x > _startOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offSetXChangeSpeedOnMove;

                if (_wallRunFraming.m_TrackedObjectOffset.x < _startOffSetX)
                {
                    _wallRunFraming.m_TrackedObjectOffset.x = _startOffSetX;
                }
            }
            else if (_wallRunFraming.m_TrackedObjectOffset.x < _startOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offSetXChangeSpeedOnMove;

                if (_wallRunFraming.m_TrackedObjectOffset.x > _startOffSetX)
                {
                    _wallRunFraming.m_TrackedObjectOffset.x = _startOffSetX;
                }
            }
        }
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

            if (lookDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

                float targetAngle = lookRotation.eulerAngles.y;

                //現在の角度
                float currentAngle = _wallRunPOV.m_HorizontalAxis.Value;

                //新しい角度を作成
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _horizontalMaxSpeed * Time.deltaTime);

                //現在の角度と、向かせたい角度の差が一定値に収まるまで更新
                if (Mathf.Abs(lookRotation.eulerAngles.y - _wallRunPOV.m_HorizontalAxis.Value) > 0.5f)
                {
                    _wallRunPOV.m_HorizontalAxis.Value = newAngle;
                }

                //縦方向の回転の設定
                if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Up)
                {
                    if (_wallRunPOV.m_VerticalAxis.Value > -60)
                    {
                        _wallRunPOV.m_VerticalAxis.Value -= _upperMaxSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    if (_wallRunPOV.m_VerticalAxis.Value < 25)
                    {
                        _wallRunPOV.m_VerticalAxis.Value += _upperMaxSpeed * Time.deltaTime;
                    }
                }
            }
        }

        //カメラの距離の設定
        if (_wallRunFraming.m_CameraDistance < _moveCameraDistance)
        {
            _wallRunFraming.m_CameraDistance += Time.deltaTime * _setCameraDistanceSpeed;

            if (_wallRunFraming.m_CameraDistance > _moveCameraDistance) _wallRunFraming.m_CameraDistance = _moveCameraDistance;
        }
    }

    public void WallRunZipUpToFrontCameraSet()
    {
        if (_cameraControl.PlayerControl.InputManager.IsControlCameraValueChange == Vector2.zero)
        {

            if (_wallRunPOV.m_VerticalAxis.Value > 7)
            {
                _wallRunPOV.m_VerticalAxis.Value -= 300 * Time.deltaTime;

                if (_wallRunPOV.m_VerticalAxis.Value - 7 < 0.5f)
                {
                    _wallRunPOV.m_VerticalAxis.Value = 7;
                }
            }
            if (_wallRunPOV.m_VerticalAxis.Value < 7)
            {
                _wallRunPOV.m_VerticalAxis.Value += 300 * Time.deltaTime;

                if (_wallRunPOV.m_VerticalAxis.Value - 7 < 0.5f)
                {
                    _wallRunPOV.m_VerticalAxis.Value = 7;
                }
            }
        }
    }

}
