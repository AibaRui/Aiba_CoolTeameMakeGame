using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public enum InputChoice
    {
        KeyboardAndMouse, Controller,
    }

    [Header("Swing�̃J�����ݒ�")]
    [SerializeField] private SwingCameraControl _swingCameraControl;
    [Header("WallRun�J�����̐ݒ�")]
    [SerializeField] private WallRunCameraControl _wallRunCamera;
    [Header("Zip�̃J�����ݒ�")]
    [SerializeField] private ZipCameraControl _zipCameraControl;

    [Header("Swing�A�󒆎��̏�����Ԃ�Y�̊p�x")]
    [SerializeField] private float _firstYvalue = 0;

    [Header("Swing�A�󒆎��̉������̂�Y�̊p�x")]
    [SerializeField] private float _downvalueY = 10;

    [Header("Swing�A�󒆎��̏�����Ԃ�OffSet")]
    [SerializeField] private float _firstOffSet = 1.2f;

    [Header("Swing���̍ő�̏������OffSet")]
    [SerializeField] private float _maxUpOffSet = 1.2f;

    [Header("Swing���̍ő�̉�������OffSet")]
    [SerializeField] private float _maxDownOffSet = 4f;

    [Header("Swing���̃J�����̍ŏ���Distance")]
    [SerializeField] private float _firstSwingCameraDistance = 7f;

    [Header("Swing���̃J�����̍ő��Distance")]
    [SerializeField] private float _maxSwingCameraDistance = 11f;

    [Header("�J�������_�̕ύX������܂ł̃N�[���^�C��")]
    [SerializeField] private float _cameraAngleChangeTime = 1f;



    [SerializeField] private PlayerControl _playerControl;

    [SerializeField] Transform follow;

    [SerializeField] Transform lookAt;

    public InputChoice inputChoice;

    public bool allowRuntimeCameraSettingsChanges;

    [SerializeField] private float _firstZ;

    [SerializeField] private CinemachineVirtualCamera keyboardAndMouseCamera;

    [Header("�ʏ펞�̃J����")]
    [SerializeField] private CinemachineVirtualCamera controllerCamera;

    [Header("Swing�A�󒆈ړ����̃J����")]
    [SerializeField] private CinemachineVirtualCamera _swingControllerCamera;

    [Header("�\�����̃J����")]
    [SerializeField] private CinemachineVirtualCamera _setUpControllerCamera;

    [Header("WallRun�p�̃J����")]
    [SerializeField] private CinemachineVirtualCamera _wallRunControllerCamera;

    private CinemachinePOV _swingCinemachinePOV;


    private CinemachineFramingTransposer _swingCameraFraming;

    public CinemachineVirtualCamera WallRunCameraController => _wallRunControllerCamera;

    public CinemachinePOV _groundCameraPOV;

    public CinemachinePOV GroundCameraPOV => _groundCameraPOV;

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

    /// <summary>Swing���̃J�����̋����Ɋւ��鏈�����܂Ƃ߂��N���X</summary>
    public SwingCameraControl SwingCameraControl => _swingCameraControl;
    /// <summary>Zip���̃J�����̋����Ɋւ��鏈�����܂Ƃ߂��N���X</summary>
    public ZipCameraControl ZipCameraControl => _zipCameraControl;
    /// <summary>WallRun���̃J�����̋����Ɋւ��鏈�����܂Ƃ߂��N���X</summary>
    public WallRunCameraControl WallRunCameraControl => _wallRunCamera;


    /// <summary>��莞�ԃJ����������R���g���[���[�ł��Ă��Ȃ����ǂ���</summary>
    public bool IsDontCameraMove => _isDontCameraMove;



    public CinemachineVirtualCamera SwingCamera => _swingControllerCamera;
    public CinemachinePOV SwingCinemachinePOV => _swingCinemachinePOV;
    public CinemachineFramingTransposer SwingCameraFraming { get => _swingCameraFraming; set => _swingCameraFraming = value; }

    public PlayerControl PlayerControl => _playerControl;
    public float SwingEndPlayerRotateY { get => _swingEndPlayerRotateY; set => _swingEndPlayerRotateY = value; }

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


    /// <summary>�R���g���[���[�ŃJ�����𑀍삵�����ǂ����̔���ƁA�N�[���^�C�����v������</summary>
    private void CountDontCameraMoveTime()
    {
        if (_playerControl.InputManager.IsControlCameraValueChange != Vector2.zero)
        {
            _isEndAutoFollow = false;
            _isDontCameraMove = false;
            _countTime = 0;
        }   //�R���g���[���[�ŃJ�����𑀍삵���ꍇ�A���삵�Ă����ԂƂ���

        if (!_isDontCameraMove)
        {
            _countTime += Time.deltaTime;
            if (_countTime > _cameraAngleChangeTime)
            {
                _isDontCameraMove = true;
                _countTime = 0;
            }
        }   //�J�����𑀍삵���ꍇ�A���삵�ĂȂ���ԁA�ɂ܂Ŏ����Ă������߂̎��Ԃ��v������B
    }

    public void CountTime()
    {
        //�ړ����͂��󂯎��
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

    /// <summary>Swing��Ƀv���C���[�̃J�����̉�]��␳����</summary>
    public void SwingEndCameraAutoFollow()
    {

    }


    public void SwingCameraValueX(bool isSwing)
    {
        if (isSwing)
        {
            //�ړ����͂��󂯎��
            float h = _playerControl.InputManager.HorizontalInput;

            ////////////X���̒���
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

            //�ړ����͂��󂯎��
            float h = _playerControl.InputManager.HorizontalInput;

            ////////////X���̒���
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


        ////Swing���̃J������Offset��߂�
        //_swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
    }

    public void EndSwingCamera()
    {
        _groundCameraPOV.m_HorizontalAxis.Value = _swingCinemachinePOV.m_HorizontalAxis.Value;
    }

    public void UseSwingCamera()
    {
        controllerCamera.Priority = 0;
        _setUpControllerCamera.Priority = 0;

        _swingControllerCamera.Priority = 50;
    }

    public void SetUpCamera()
    {
        controllerCamera.Priority = 0;
        _swingControllerCamera.Priority = 0;
        _wallRunControllerCamera.Priority = 0;

        _setUpControllerCamera.Priority = 50;

        //Swing���̃J������Offset��߂�
        _swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;
    }

    public void RsetCamera()
    {
        //controllerCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 

        _swingControllerCamera.Priority = 0;
        _setUpControllerCamera.Priority = 0;
        _wallRunControllerCamera.Priority = 0;

        controllerCamera.Priority = 40;
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
