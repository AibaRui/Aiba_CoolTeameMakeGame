using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WallRunCameraControl : MonoBehaviour
{
    [Header("������ɂ��鎞����")]
    [SerializeField] private float _upperMaxSpeed = 1;

    [Header("���E�Ɍ������鑬��")]
    [SerializeField] private float _horizontalMaxSpeed = 40;

    [Header("�~�܂��Ă���Ƃ��̃J�����̋���")]
    [SerializeField] private float _idleCameraDistance = 5;

    [Header("�����Ă���Ƃ��̃J�����̋���")]
    [SerializeField] private float _moveCameraDistance = 7;

    [Header("�~�܂��Ă���J������߂��܂ł̎���")]
    [SerializeField] private float _reSetCameraDistanceTime = 3;

    [Header("�~�܂��Ă���J�����̋�����ύX���鑬�x")]
    [SerializeField] private float _reSetCameraDistanceSpeed = 10;

    [Header("�����Ă���J�����̋�����ύX���鑬�x")]
    [SerializeField] private float _setCameraDistanceSpeed = 10;


    [Header("-----X����Offset�̈ړ��ɂ��Ă̐ݒ�-----")]

    [Header("OffSet�̕ύX���x")]
    [SerializeField] private float _offSetXChangeSpeed = 50f;

    [Header("������Ԃ�OffSet")]
    [SerializeField] private float _startOffSetX = 0f;

    [Header("�ǂ������̎��̍ő�OffSet")]
    [SerializeField] private float _leftWallMaxOffSetX = -3f;

    [Header("�ǂ��E���̎��̍ő�OffSet")]
    [SerializeField] private float _rightWallMaxOffSetX = 3f;

    private float _reSetCameraDistanceTimeCount = 0;

    [SerializeField] private CameraControl _cameraControl;

    private float _upperSpeed = 0;

    private CinemachinePOV _wallRunPOV;
    private CinemachineFramingTransposer _wallRunFraming;

    void Start()
    {
        _wallRunPOV = _cameraControl.WallRunCameraController.GetCinemachineComponent<CinemachinePOV>();
        _wallRunFraming = _cameraControl.WallRunCameraController.GetCinemachineComponent<CinemachineFramingTransposer>();
    }



    /// <summary>Idle���A�J���������ɖ߂�</summary>
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

    /// <summary>Idle���A�J���������ɖ߂��܂ł̎��Ԃ��v��</summary>
    public void CountReSetWallIdleCameraTime()
    {
        if (_reSetCameraDistanceTimeCount < _reSetCameraDistanceTime)
        {
            _reSetCameraDistanceTimeCount += Time.deltaTime;
        }
    }

    /// <summary>Idle���A�J���������ɖ߂��܂ł̎��Ԍv���̒l�����Z�b�g</summary>
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
            if (Mathf.Abs(_wallRunFraming.m_TrackedObjectOffset.x - _startOffSetX) < 0.01f) return;

            _wallRunFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offSetXChangeSpeed;

        }
        else if (_wallRunFraming.m_TrackedObjectOffset.x < _startOffSetX)
        {
            if (Mathf.Abs(_wallRunFraming.m_TrackedObjectOffset.x - _startOffSetX) < 0.01f) return;

            _wallRunFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offSetXChangeSpeed;
        }
    }

    public void XOffSetControlWallRun()
    {
        if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Right)
        {
            if (_wallRunFraming.m_TrackedObjectOffset.x < _leftWallMaxOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offSetXChangeSpeed;
            }
        }
        else if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Left)
        {
            if (_wallRunFraming.m_TrackedObjectOffset.x > _rightWallMaxOffSetX)
            {
                _wallRunFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offSetXChangeSpeed;
            }
        }
        else
        {
            if (_wallRunFraming.m_TrackedObjectOffset.x > _startOffSetX)
            {
                if (Mathf.Abs(_wallRunFraming.m_TrackedObjectOffset.x - _startOffSetX) < 0.01f) return;

                _wallRunFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offSetXChangeSpeed ;

            }
            else if (_wallRunFraming.m_TrackedObjectOffset.x < _startOffSetX)
            {
                if (Mathf.Abs(_wallRunFraming.m_TrackedObjectOffset.x - _startOffSetX) < 0.01f) return;

                _wallRunFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offSetXChangeSpeed ;
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

            //�J���������������������̉�]
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            float targetAngle = lookRotation.eulerAngles.y;

            //���݂̊p�x
            float currentAngle = _wallRunPOV.m_HorizontalAxis.Value;

            //�V�����p�x���쐬
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _horizontalMaxSpeed * Time.deltaTime);

            //���݂̊p�x�ƁA�����������p�x�̍������l�Ɏ��܂�܂ōX�V
            if (Mathf.Abs(lookRotation.eulerAngles.y - _wallRunPOV.m_HorizontalAxis.Value) > 0.5f)
            {
                _wallRunPOV.m_HorizontalAxis.Value = newAngle;
            }

            //�c�����̉�]�̐ݒ�
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

        //�J�����̋����̐ݒ�
        if (_wallRunFraming.m_CameraDistance < _moveCameraDistance)
        {
            _wallRunFraming.m_CameraDistance += Time.deltaTime * _setCameraDistanceSpeed;
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