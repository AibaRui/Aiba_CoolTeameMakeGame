using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WallRunCameraControl : MonoBehaviour
{
    [SerializeField] private CameraControl _cameraControl;

    private CinemachinePOV _swingCinemachinePOV;

    private CinemachineFramingTransposer _swingCameraFraming;

    void Start()
    {
        _swingCinemachinePOV = _cameraControl.WallRunCameraController.GetCinemachineComponent<CinemachinePOV>();
        _swingCameraFraming = _cameraControl.WallRunCameraController.GetCinemachineComponent<CinemachineFramingTransposer>();
    }


    public void WallRunCameraFollow()
    {
        if (_cameraControl.IsDontCameraMove)
        {
            Quaternion targetRotation = default;

            Vector3 wallCrossRight = _cameraControl.PlayerControl.WallRunCheck.WallCrossRight;
            Vector3 wallDir = -_cameraControl.PlayerControl.WallRunCheck.WallDir;

            if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Right)
            {
                targetRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
            }
            else if (_cameraControl.PlayerControl.WallRun.MoveDir == WallRun.MoveDirection.Left)
            {
                targetRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(wallDir, Vector3.up);
            }


            // Calculate the target angle based on the player's rotation
            float _targetAngle = _cameraControl.PlayerControl.PlayerT.eulerAngles.y;

            // Gradually move the POV's horizontal angle towards the target angle
            float currentAngle = _swingCinemachinePOV.m_HorizontalAxis.Value;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, _targetAngle, 100 * Time.deltaTime);
            _swingCinemachinePOV.m_HorizontalAxis.Value = newAngle;
        }
    }
}
