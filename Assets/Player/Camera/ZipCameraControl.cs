using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class ZipCameraControl
{
    [Header("[=====Value�ݒ�=====]")]
    [Header("Swing�A�󒆎��̏�����Ԃ�Y�̊p�x")]
    [SerializeField] private float _firstYvalue = 0;

    [Header("[=====Offset�ݒ�=====]")]
    [Header("Y���̐ݒ�")]
    [Header("������Ԃ�OffSet")]
    [SerializeField] private float _firstOffSet = 1.2f;

    [Header("[=====Distance�ݒ�=====]")]
    [Header("Zip����Distance�̋���")]
    [SerializeField] private float _zipDistance = 7;
    [Header("�����𗣂����x")]
    [SerializeField] private float _zipDistanceCangeSpeed = 7;

    [Header("[=====FOV�ݒ�=====]")]
    [Header("�ő�FOV")]
    [SerializeField] private float _maxFOV = 70;
    [Header("FOV��ύX���鑬�x")]
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


    /// <summary>Zip�������Ƃ��̃J����</summary>
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

    /// <summary>Zip���̃J�����̐ݒ�</summary>
    public void ZipCamera()
    {
        SetOffset();
        SetValue();
        ZipMoveCamera();
        ChangeFOV();
    }


    /// <summary>FOV��ύX����</summary>
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


    /// <summary>�J�����̊p�x�ɂ��Ă̐ݒ�</summary>
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

    /// <summary>�J������Offset�ݒ�</summary>
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