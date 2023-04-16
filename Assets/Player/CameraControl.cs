using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public enum InputChoice
    {
        KeyboardAndMouse, Controller,
    }

    [Header("Swing、空中時の初期状態のYの角度")]
    [SerializeField] private float _firstYvalue = 0;



    [Header("Swing、空中時の下向きののYの角度")]
    [SerializeField] private float _downvalueY = 10;



    [Header("Swing、空中時の初期状態のOffSet")]
    [SerializeField] private float _firstOffSet = 1.2f;

    [Header("Swing時の最大の上方向のOffSet")]
    [SerializeField] private float _maxUpOffSet = 1.2f;

    [Header("Swing時の最大の下方向のOffSet")]
    [SerializeField] private float _maxDownOffSet = 4f;

    [Header("Swing時のカメラの最初のDistance")]
    [SerializeField] private float _firstSwingCameraDistance = 7f;

    [Header("Swing時のカメラの最大のDistance")]
    [SerializeField] private float _maxSwingCameraDistance = 11f;

    [Header("カメラ視点の変更をするまでのクールタイム")]
    [SerializeField] private float _cameraAngleChangeTime = 1f;

    [SerializeField] private PlayerControl _playerControl;

    [SerializeField] Transform follow;

    [SerializeField] Transform lookAt;

    public InputChoice inputChoice;
    // public InvertSettings keyboardAndMouseInvertSettings;
    // public InvertSettings controllerInvertSettings;
    public bool allowRuntimeCameraSettingsChanges;

    [SerializeField] private float _firstZ;

    [SerializeField] private CinemachineVirtualCamera keyboardAndMouseCamera;

    [Header("通常時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera controllerCamera;

    [Header("Swing、空中移動時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _swingControllerCamera;

    [Header("構え時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _setUpControllerCamera;

    private CinemachinePOV _swingCinemachinePOV;
    private CinemachineFramingTransposer _swingCameraFraming;

    private bool _isUpEnd = false;

    private float _countTime = 0;
    private bool _isDontCameraMove = false;

    private float _countCameraMoveX = 0;

    private float _countCameraMoveY = 0;

    void Awake()
    {
        UpdateCameraSettings();
        _swingCinemachinePOV = _swingControllerCamera.GetCinemachineComponent<CinemachinePOV>();
        _swingCameraFraming = _swingControllerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        if (allowRuntimeCameraSettingsChanges)
        {
            UpdateCameraSettings();
        }
        CountDontCameraMoveTime();
    }

    void UpdateCameraSettings()
    {
        keyboardAndMouseCamera.Follow = follow;
        keyboardAndMouseCamera.LookAt = lookAt;
        // keyboardAndMouseCamera.m_XAxis.m_InvertInput = keyboardAndMouseInvertSettings.invertX;
        // keyboardAndMouseCamera.m_YAxis.m_InvertInput = keyboardAndMouseInvertSettings.invertY;

        //  controllerCamera.m_XAxis.m_InvertInput = controllerInvertSettings.invertX;
        //  controllerCamera.m_YAxis.m_InvertInput = controllerInvertSettings.invertY;
        controllerCamera.Follow = follow;
        controllerCamera.LookAt = lookAt;

        keyboardAndMouseCamera.Priority = inputChoice == InputChoice.KeyboardAndMouse ? 1 : 0;
        controllerCamera.Priority = inputChoice == InputChoice.Controller ? 1 : 0;
    }


    public void SwingEndTranspectorUp()
    {
        _isUpEnd = true;
    }

    private void CountDontCameraMoveTime()
    {
        if (_playerControl.InputManager.IsControlCameraValueChange != Vector2.zero)
        {
            _isDontCameraMove = false;
            _countTime = 0;
        }

        if (!_isDontCameraMove)
        {
            _countTime += Time.deltaTime;
            if (_countTime > _cameraAngleChangeTime)
            {
                _isDontCameraMove = true;
                _countTime = 0;
            }
        }
    }

    public void CountTime()
    {
        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;

        if (h == 0)
        {
            _countCameraMoveX = 0;
        }
        else
        {
            if (_countCameraMoveX != 0.8)
            {
                _countCameraMoveX += Time.deltaTime * 0.5f;

                if (_countCameraMoveX > 0.8f)
                {
                    _countCameraMoveX = 0.8f;
                }
            }
        }

        if (_isDontCameraMove)
        {
            _countCameraMoveY += Time.deltaTime * 0.15f;
        }


    }

    /// <summary>Swing中にスクリーン上でのプレイヤーの位置を変更する</summary>
    /// <param name="velocityY"></param>
    /// <param name="down"></param>
    /// <param name="up"></param>
    /// <param name="changeSpeed"></param>
    public void SwingCameraYValues(float velocityY, float down, float up, float changeSpeed)
    {
        //スクリーン上でのプレイヤーの位置の変更
        if (velocityY > 0)
        {
            if (_swingCameraFraming.m_TrackedObjectOffset.y > _maxUpOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime;
            }
        }   //位置を下の方に下げる
        else if (velocityY < 0)
        {
            if (_swingCameraFraming.m_TrackedObjectOffset.y < _maxDownOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime;
            }
        }   //位置を上の方にする

        if (_isDontCameraMove)
        {

            //移動入力を受け取る
            float h = _playerControl.InputManager.HorizontalInput;

            ////////////X軸の調整
            if (true)
            {
                if (h > 0.8)
                {
                    _swingCinemachinePOV.m_HorizontalAxis.Value += _countCameraMoveX;
                }
                else if (h < -0.8)
                {
                    _swingCinemachinePOV.m_HorizontalAxis.Value -= _countCameraMoveX;
                }
            }

            ////////// //Y軸の調整
            if (velocityY > 0)
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue - velocityY * 0.7f)
                {
                    _swingCinemachinePOV.m_VerticalAxis.Value -= _countCameraMoveY;
                }
            }
            else
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value < _downvalueY)
                {
                    _swingCinemachinePOV.m_VerticalAxis.Value += _countCameraMoveY;
                }
            }
        }
    }

    public void SwingX(Vector3 dir)
    {
        //手動でカメラの角度を動かしていたら動かさない
        if (_playerControl.InputManager.IsControlCameraValueChange == Vector2.zero)
        {
            return;
        }

    }

    /// <summary>Y軸の角度を直す</summary>
    public void AirCameraYValue(float velocityY)
    {

        if (_isUpEnd)
        {

            //モニター上でのプレイヤーの位置を変える。上の方に
            if (_swingCameraFraming.m_TrackedObjectOffset.y > -2)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime * 10;

                if (_swingCameraFraming.m_CameraDistance <= _maxSwingCameraDistance)
                    _swingCameraFraming.m_CameraDistance += Time.deltaTime * 10;
            }
            else
            {
                _isUpEnd = false;
            }




        }       //上方向に飛び上がった時
        else
        {
            //モニター上でのプレイヤーの位置を変える。初期状態に
            if (velocityY < 0)
            {
                if (_swingCameraFraming.m_TrackedObjectOffset.y < _firstOffSet)
                {
                    _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime * 2;
                }

                if (_swingCameraFraming.m_CameraDistance > _firstSwingCameraDistance)
                {
                    _swingCameraFraming.m_CameraDistance -= Time.deltaTime;

                    if (_swingCameraFraming.m_CameraDistance < _firstSwingCameraDistance)
                    {
                        _swingCameraFraming.m_CameraDistance = _firstSwingCameraDistance;
                    }
                }
            }
            if (_isDontCameraMove)
            {
                //カメラの角度を元に戻す
                if (_playerControl.InputManager.IsControlCameraValueChange == Vector2.zero)
                {
                    if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
                    {
                        _swingCinemachinePOV.m_VerticalAxis.Value -= Time.deltaTime * 20;

                        if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
                        {
                            _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
                        }
                    }
                    else if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
                    {
                        _swingCinemachinePOV.m_VerticalAxis.Value += Time.deltaTime * 20;

                        if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
                        {
                            _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
                        }
                    }
                }
            }
        }

    }
    public void SwingCamera()
    {
        controllerCamera.Priority = 0;
        _setUpControllerCamera.Priority = 0;

        _swingControllerCamera.Priority = 50;
    }

    public void SetUpCamera()
    {
        controllerCamera.Priority = 0;
        _swingControllerCamera.Priority = 0;

        _setUpControllerCamera.Priority = 50;
    }

    public void RsetCamera()
    {
        //controllerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 

        _swingControllerCamera.Priority = 0;
        _setUpControllerCamera.Priority = 0;

        controllerCamera.Priority = 40;
    }


    public void SwingEndSetCamera()
    {
        _countCameraMoveY = 0;
        _countCameraMoveX = 0;
    }

}

