using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class ZipCameraControl
{
    [Header("[=====Value設定=====]")]
    [Header("Swing、空中時の初期状態のYの角度")]
    [SerializeField] private float _firstYvalue = 0;

    [Header("[=====Offset設定=====]")]
    [Header("Y軸の設定")]
    [Header("初期状態のOffSet")]
    [SerializeField] private float _firstOffSet = 1.2f;

    [Header("[=====Distance設定=====]")]
    [Header("Zip時のDistanceの距離")]
    [SerializeField] private float _zipDistance = 7;
    [Header("距離を離す速度")]
    [SerializeField] private float _zipDistanceCangeSpeed = 7;

    [Header("[=====FOV設定=====]")]
    [Header("最大FOV")]
    [SerializeField] private float _maxFOV = 70;
    [Header("FOVを変更する速度")]
    [SerializeField] private float _fovChecgeSpeed = 10;

    private CameraControl _cameraControl;
    private CinemachineVirtualCamera _camera;
    private CinemachinePOV _swingCinemachinePOV;
    private CinemachineFramingTransposer _swingCameraFraming;



    public void Init(CameraControl cameraControl)
    {
        _cameraControl = cameraControl;
        _swingCinemachinePOV = _cameraControl.SwingCinemachinePOV;
        _swingCameraFraming = _cameraControl.SwingCameraFraming;
        _camera = _cameraControl.SwingCamera;
    }


    /// <summary>Zipをしたときのカメラ</summary>
    public void ZipMoveCamera()
    {
        if (_swingCameraFraming.m_CameraDistance < _zipDistance)
        {
            _swingCameraFraming.m_CameraDistance += _zipDistanceCangeSpeed;
        }

        if (_swingCameraFraming.m_CameraDistance > _zipDistance)
        {
            _swingCameraFraming.m_CameraDistance = _zipDistance;
        }
    }

    /// <summary>Zip中のカメラの設定</summary>
    public void ZipCamera()
    {
        SetOffset();
        SetValue();
        ZipMoveCamera();
        ChangeFOV();
    }


    /// <summary>FOVを変更する</summary>
    public void ChangeFOV()
    {
        if (_camera.m_Lens.FieldOfView < _maxFOV)
        {
            _camera.m_Lens.FieldOfView += Time.deltaTime * _fovChecgeSpeed;

            if (_camera.m_Lens.FieldOfView > _maxFOV)
            {
                _camera.m_Lens.FieldOfView = _maxFOV;
            }
        }
    }


    /// <summary>カメラの角度についての設定</summary>
    public void SetValue()
    {
        if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
        {
            _swingCinemachinePOV.m_VerticalAxis.Value -= Time.deltaTime * 30;

            if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
            {
                _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
            }
        }
        else if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
        {
            _swingCinemachinePOV.m_VerticalAxis.Value += Time.deltaTime * 30;

            if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
            {
                _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
            }
        }
    }

    /// <summary>カメラのOffset設定</summary>
    public void SetOffset()
    {
        if (_swingCameraFraming.m_TrackedObjectOffset.y < _firstOffSet)
        {
            _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime * 3f;

            if (_swingCameraFraming.m_TrackedObjectOffset.y > _firstOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
            }
        }
        else if (_swingCameraFraming.m_TrackedObjectOffset.y > _firstOffSet)
        {
            _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime * 1f;

            if (_swingCameraFraming.m_TrackedObjectOffset.y < _firstOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
            }
        }
    }
}
