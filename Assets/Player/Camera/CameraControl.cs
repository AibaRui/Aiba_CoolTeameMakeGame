using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public enum InputChoice
    {
        KeyboardAndMouse, Controller,
    }

    [Header("Swingのカメラ設定---")]
    [SerializeField] private SwingCameraControl _swingCameraControl;
    [Header("WallRunカメラの設定---")]
    [SerializeField] private WallRunCameraControl _wallRunCamera;
    [Header("Zipのカメラ設定---")]
    [SerializeField] private ZipCameraControl _zipCameraControl;
    [Header("PointZipのカメラ設定---")]
    [SerializeField] private PointZipCamera _pointZipCamera;

    public PointZipCamera PointZipCameraControl => _pointZipCamera;

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

    public bool allowRuntimeCameraSettingsChanges;

    [SerializeField] private float _firstZ;

    [SerializeField] private CinemachineVirtualCamera keyboardAndMouseCamera;

    [Header("通常時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera controllerCamera;
    [Header("Swing、空中移動時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _swingControllerCamera;
    [Header("構え時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _setUpControllerCamera;
    [Header("WallRun用のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _wallRunControllerCamera;
    [Header("PointZip用のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _pointZipControllerCamera;



    private CinemachinePOV _swingCinemachinePOV;
    private CinemachineFramingTransposer _swingCameraFraming;

    private bool _isTutorial = false;
    public bool IsTutorial { get => _isTutorial; set => _isTutorial = value; }

    private bool _isUpEnd = false;

    private float _countTime = 0;
    private bool _isDontCameraMove = false;
    private float _countCameraMoveSwingingX = 0;
    private float _countCameraMoveAirX = 0;
    private float _countCameraMoveY = 0;
    private bool _isEndAutoFollow = false;
    private float _autoFloowCount = 0;
    private bool _isSwingEndCameraDistanceToLong = false;
    private float _swingEndPlayerRotateY;

    /// <summary>Swing時のカメラの挙動に関する処理をまとめたクラス</summary>
    public SwingCameraControl SwingCameraControl => _swingCameraControl;
    /// <summary>Zip時のカメラの挙動に関する処理をまとめたクラス</summary>
    public ZipCameraControl ZipCameraControl => _zipCameraControl;
    /// <summary>WallRun時のカメラの挙動に関する処理をまとめたクラス</summary>
    public WallRunCameraControl WallRunCameraControl => _wallRunCamera;
    /// <summary>一定時間カメラ操作をコントローラーでしていないかどうか</summary>
    public bool IsDontCameraMove => _isDontCameraMove;
    public CinemachineVirtualCamera SwingCamera => _swingControllerCamera;
    public CinemachinePOV SwingCinemachinePOV => _swingCinemachinePOV;
    public CinemachineFramingTransposer SwingCameraFraming { get => _swingCameraFraming; set => _swingCameraFraming = value; }

    public PlayerControl PlayerControl => _playerControl;
    public float SwingEndPlayerRotateY { get => _swingEndPlayerRotateY; set => _swingEndPlayerRotateY = value; }
    public CinemachineVirtualCamera WallRunCameraController => _wallRunControllerCamera;
    public CinemachinePOV _groundCameraPOV;
    public CinemachinePOV GroundCameraPOV => _groundCameraPOV;
    public CinemachineVirtualCamera PointZipVirtualCamera => _pointZipControllerCamera;
    public bool IsEndAutpFollow { get => _isEndAutoFollow; set => _isEndAutoFollow = value; }


    void Awake()
    {
        UpdateCameraSettings();
        _swingCinemachinePOV = _swingControllerCamera.GetCinemachineComponent<CinemachinePOV>();
        _swingCameraFraming = _swingControllerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _groundCameraPOV = controllerCamera.GetCinemachineComponent<CinemachinePOV>();
        _swingCameraControl.Init(this);
        _wallRunCamera.Init(this);
        _zipCameraControl.Init(this);
        _pointZipCamera.Init(this);
    }

    void Update()
    {
        if (_isTutorial) return;

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


    /// <summary>コントローラーでカメラを操作したかどうかの判定と、クールタイムを計測する</summary>
    private void CountDontCameraMoveTime()
    {
        if (_playerControl.InputManager.IsControlCameraValueChange != Vector2.zero)
        {
            _isEndAutoFollow = false;
            _isDontCameraMove = false;
            _countTime = 0;
        }   //コントローラーでカメラを操作した場合、操作している状態とする

        if (!_isDontCameraMove)
        {
            _countTime += Time.deltaTime;
            if (_countTime > _cameraAngleChangeTime)
            {
                _isDontCameraMove = true;
                _countTime = 0;
            }
        }   //カメラを操作した場合、操作してない状態、にまで持っていくための時間を計測する。
    }

    public void CountTime()
    {
        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;

        if (h > -0.5f && h < 0.5f)
        {
            //if (_countCameraMoveSwingingX > 0)
            //{
            //    _countCameraMoveSwingingX -= 0.01f;
            //}

            //if (_countCameraMoveAirX > 0)
            //{
            //    _countCameraMoveAirX -= 0.01f;
            //}

            _countCameraMoveSwingingX = 0;
            _countCameraMoveAirX = 0;
        }
        else
        {
            if (_playerControl.Swing.IsSwingNow)
            {
                if (_countCameraMoveSwingingX < 0.5)
                {
                    _countCameraMoveSwingingX += 0.002f;
                }
            }
            else
            {
                if (_countCameraMoveAirX < 0.4f)
                {
                    _countCameraMoveAirX += 0.002f;
                }
            }

            // if (!_playerControl.VelocityLimit.IsSpeedUp)
            //  {

            // }

            if (_isDontCameraMove)
            {
                //if (_countCameraMoveY != 0.2f)
                //{
                //    _countCameraMoveY += 0.0005f;
                //    if (_countCameraMoveY > 0.2f)
                //    {
                //        _countCameraMoveY = 0.2f;
                //    }

                //}
            }
        }

    }

    /// <summary>Swing後にプレイヤーのカメラの回転を補正する</summary>
    public void SwingEndCameraAutoFollow()
    {

    }


    public void SwingCameraValueX(bool isSwing)
    {
        if (isSwing)
        {
            //移動入力を受け取る
            float h = _playerControl.InputManager.HorizontalInput;

            ////////////X軸の調整
            if (h > 0.8f)
            {
                _swingCinemachinePOV.m_HorizontalAxis.Value += _countCameraMoveSwingingX;
            }
            else if (h < -0.8f)
            {
                _swingCinemachinePOV.m_HorizontalAxis.Value -= _countCameraMoveSwingingX;
            }
        }
        else
        {
            //if (_playerControl.VelocityLimit.IsSpeedUp ||_isEndAutoFollow)
            //{
            //    _countCameraMoveAirX = 0;
            //    return;
            //}
            //

            //移動入力を受け取る
            float h = _playerControl.InputManager.HorizontalInput;

            ////////////X軸の調整
            if (h > 0.8f)
            {
                _swingCinemachinePOV.m_HorizontalAxis.Value += _countCameraMoveAirX;
            }
            else if (h < -0.8f)
            {
                _swingCinemachinePOV.m_HorizontalAxis.Value -= _countCameraMoveAirX;
            }
        }
    }

    public void UseWallRunCamera()
    {
        //controllerCamera.Priority = 0;
        //_setUpControllerCamera.Priority = 0;

        //_swingControllerCamera.Priority = 0;

        //_wallRunControllerCamera.Priority = 50;


        ////Swing時のカメラのOffsetを戻す
        //_swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
    }

    public void EndSwingCamera()
    {
        _groundCameraPOV.m_HorizontalAxis.Value = _swingCinemachinePOV.m_HorizontalAxis.Value;
    }

    public void UseCanera(CameraType cameraType)
    {
        controllerCamera.Priority = 0;
        _setUpControllerCamera.Priority = 0;
        _swingControllerCamera.Priority = 0;
        _pointZipControllerCamera.Priority = 0;

        if (cameraType == CameraType.Idle)
        {
            controllerCamera.Priority = 40;
        }
        else if (cameraType == CameraType.Setup)
        {
            _setUpControllerCamera.Priority = 50;

            //Swing時のカメラのOffsetを戻す
            _swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
        }
        else if (cameraType == CameraType.Swing)
        {
            _swingControllerCamera.Priority = 50;
        }
        else if (cameraType == CameraType.PointCamera)
        {
            _pointZipControllerCamera.Priority = 50;
            _pointZipCamera.SetCamera();
        }
    }

    public void SwingEndSetCamera()
    {
        _countCameraMoveY = 0;
        // _countCameraMoveAirX = _countCameraMoveSwingingX;
        _countCameraMoveSwingingX = 0;
        _autoFloowCount = 0;

        //_swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
    }

}

public enum CameraType
{
    Idle,
    Setup,
    Swing,
    PointCamera,
}