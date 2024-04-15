using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZipCamera
{
    [Header("初期カメラの距離")]
    [SerializeField] private float _firstCameraDistance = 5;

    [Header("最終カメラ距離")]
    [SerializeField] private float _endCameraDistance = 2.5f;

    [Header("カメラの距離の変更速度")]
    [SerializeField] private float _cameraDistanceChangeSpeed = 0.04f;

    private CinemachineVirtualCamera _camera;

    private CinemachineFramingTransposer _cameraTransposer;

    private CameraControl _cameraControl;

    public void Init(CameraControl cameraControl)
    {
        _cameraControl = cameraControl;
        _camera = _cameraControl.PointZipVirtualCamera;
        _cameraTransposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void SetCamera()
    {
        Vector3 dir = _cameraControl.PlayerControl.PointZip.PointZipSearch.MoveTargetPositin - _cameraControl.PlayerControl.PlayerT.position;
        dir.y = 0;

        _camera.transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
        _cameraTransposer.m_CameraDistance = _firstCameraDistance;
    }



    /// <summary>カメラの距離を縮める</summary>
    public void PointCameraDistanceShorting()
    {
        if (_cameraTransposer.m_CameraDistance > _endCameraDistance)
        {
            _cameraTransposer.m_CameraDistance -= Time.deltaTime * _cameraDistanceChangeSpeed;

            if (Mathf.Abs(_cameraTransposer.m_CameraDistance - _endCameraDistance) < 0.1f)
            {
                _cameraTransposer.m_CameraDistance = _endCameraDistance;
            }
        }
    }



}
