using Cinemachine;
using UnityEngine;

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

    [SerializeField] private WallRunCameraControl _wallRunCamera;

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

    private CinemachinePOV _swingCinemachinePOV;

    private CinemachineFramingTransposer _swingCameraFraming;

    public CinemachineVirtualCamera WallRunCameraController => _wallRunControllerCamera;


    private bool _isUpEnd = false;

    private float _countTime = 0;
    private bool _isDontCameraMove = false;

    public bool IsDontCameraMove => _isDontCameraMove;

    private float _countCameraMoveSwingingX = 0;

    private float _countCameraMoveAirX = 0;

    private float _countCameraMoveY = 0;

    private bool _isEndAutoFollow = false;

    private float _autoFloowCount = 0;

    private bool _isSwingEndCameraDistanceToLong = false;

    private float _swingEndPlayerRotateY;


    public WallRunCameraControl WallRunCameraControl => _wallRunCamera;
    public PlayerControl PlayerControl => _playerControl;
    public float SwingEndPlayerRotateY { get => _swingEndPlayerRotateY; set => _swingEndPlayerRotateY = value; }




    public bool IsEndAutpFollow => _isEndAutoFollow;
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
            _isEndAutoFollow = false;
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
                if (_countCameraMoveSwingingX < 0.4)
                {
                    _countCameraMoveSwingingX += 0.001f;
                }
            }

            if (!_playerControl.VelocityLimit.IsSpeedUp)
            {
                if (_countCameraMoveAirX < 0.3f)
                {
                    _countCameraMoveAirX += 0.001f;
                }
            }

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
        //var h = _playerControl.InputManager.HorizontalInput;
        //var v = _playerControl.InputManager.VerticalInput;
        //if (h != 0 || v != 0)
        //{
        //    //追従を終了
        //    _isEndAutoFollow = false;
        //}

        //if (_isEndAutoFollow && _isDontCameraMove)
        //{
        //    if (_autoFloowCount < 0.8f)
        //    {
        //        _autoFloowCount += 0.01f;
        //    }

        //    float y = 0;
        //    if (_playerControl.PlayerT.eulerAngles.y > 180)
        //    {
        //        y = _playerControl.PlayerT.eulerAngles.y - 360;
        //    }
        //    else
        //    {
        //        y = _playerControl.PlayerT.eulerAngles.y;
        //    }

        //    float angleDiff = Mathf.DeltaAngle(y, _swingCinemachinePOV.m_HorizontalAxis.Value); // 角度差を-180度から180度の範囲に収める

        //    if (Mathf.Abs(angleDiff) > 90f)
        //    {
        //        angleDiff -= Mathf.Sign(angleDiff) * 180f;
        //    }// 角度差が90度より大きい場合は、逆方向に回転する

        //    if (angleDiff > 0f)
        //    {
        //        _swingCinemachinePOV.m_HorizontalAxis.Value -= Mathf.Min(angleDiff, _autoFloowCount);
        //    }// プレイヤーの回転角度に近づくようにValueの値を減らす
        //    else if (angleDiff < 0f)
        //    {
        //        _swingCinemachinePOV.m_HorizontalAxis.Value += Mathf.Min(-angleDiff, _autoFloowCount);
        //    }// プレイヤーの回転角度に近づくようにValueの値を増やす

        //    float dis = Mathf.Abs(_swingEndPlayerRotateY - _swingCinemachinePOV.m_HorizontalAxis.Value);

        //    if (dis < 1f)
        //    {
        //        //追従を終了
        //        _isEndAutoFollow = false;

        //        //現在のカメラの回転速度を受け継ぐ
        //        _countCameraMoveAirX = _autoFloowCount;

        //        return;
        //    }
        //}
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

    /// <summary>Swing中にスクリーン上でのプレイヤーの位置を変更する</summary>
    /// <param name="velocityY"></param>
    public void SwingCameraYValues(float velocityY, float down, float up, float changeSpeed)
    {
        //スクリーン上でのプレイヤーの位置の変更
        if (velocityY > 0)
        {
            Vector3 v = new Vector3(0, velocityY, 0);
            if (_swingCameraFraming.m_CameraDistance > _firstSwingCameraDistance + 0.5f)
                _swingCameraFraming.m_CameraDistance -= 0.0007f * v.magnitude;

            if (Mathf.Abs(_swingCameraFraming.m_CameraDistance - (_firstSwingCameraDistance + 0.5f)) < 0.1f)
            {
                _swingCameraFraming.m_CameraDistance = _firstSwingCameraDistance + 0.5f;
            }


            if (_swingCameraFraming.m_TrackedObjectOffset.y > _maxUpOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime;
            }
        }   //位置を下の方に下げる
        else if (velocityY < 0)
        {
            //カメラの距離を離す
            Vector3 v = new Vector3(0, velocityY, 0);
            if (_swingCameraFraming.m_CameraDistance < _maxSwingCameraDistance)
                _swingCameraFraming.m_CameraDistance += 0.01f * v.magnitude;

            if (Mathf.Abs(_swingCameraFraming.m_CameraDistance - _maxSwingCameraDistance) < 0.1f)
            {
                _swingCameraFraming.m_CameraDistance = _maxSwingCameraDistance;
            }


            if (_swingCameraFraming.m_TrackedObjectOffset.y < _maxDownOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime;
            } //位置を上の方にする
        }

        if (_isDontCameraMove)
        {
            ////////// //Y軸の調整
            if (velocityY > 0)
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value > -40)
                {
                    Vector3 v = new Vector3(0, velocityY, 0);

                    _swingCinemachinePOV.m_VerticalAxis.Value -= 0.009f * v.magnitude;
                    Debug.Log("d");
                }
            }
            else if (velocityY < 0)
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value <= 30)
                {
                    Vector3 v = new Vector3(0, velocityY, 0);
                    _swingCinemachinePOV.m_VerticalAxis.Value += 0.005f * v.magnitude;
                }
            }
        }
    }

    /// <summary>Y軸の角度を直す</summary>
    public void AirCameraYValue(float velocityY)
    {
        //Swing終わりに、カメラを離すかどうか
        if (_isSwingEndCameraDistanceToLong)
        {
            if (velocityY < 0) _isSwingEndCameraDistanceToLong = false;

            //カメラの距離を離す
            Vector3 v = new Vector3(0, velocityY, 0);
            if (_swingCameraFraming.m_CameraDistance < _maxSwingCameraDistance + 1)
                _swingCameraFraming.m_CameraDistance += 0.01f * v.magnitude;

            if (Mathf.Abs(_swingCameraFraming.m_CameraDistance - _maxSwingCameraDistance + 1) < 0.1f)
            {
                _swingCameraFraming.m_CameraDistance = _maxSwingCameraDistance + 1;
            }
        }


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
                    _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime * 0.5f;

                if (_swingCameraFraming.m_TrackedObjectOffset.y > _firstOffSet)
                    _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime * 0.5f;

                if (Mathf.Abs(_swingCameraFraming.m_TrackedObjectOffset.y - _firstOffSet) < 0.02f)
                    _swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;


                if (_swingCameraFraming.m_CameraDistance > _firstSwingCameraDistance)
                {
                    Vector3 v = new Vector3(0, _playerControl.Rb.velocity.y, 0);
                    _swingCameraFraming.m_CameraDistance -= 0.004f * v.magnitude;

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
                        _swingCinemachinePOV.m_VerticalAxis.Value -= Time.deltaTime * 10;

                        if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
                        {
                            _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
                        }
                    }
                    else if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
                    {
                        _swingCinemachinePOV.m_VerticalAxis.Value += Time.deltaTime * 10;

                        if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
                        {
                            _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
                        }
                    }
                }
            }
        }
    }


    /// <summary>Zipをしたときのカメラ</summary>
    public void ZipMoveCamera(float setDistance)
    {
        if (setDistance < _swingCameraFraming.m_CameraDistance)
        {
            return;
        }

        _swingCameraFraming.m_CameraDistance = setDistance;
    }


    public void UseWallRunCamera()
    {
        controllerCamera.Priority = 0;
        _setUpControllerCamera.Priority = 0;

        _swingControllerCamera.Priority = 0;

        _wallRunControllerCamera.Priority = 50;
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
    }

    public void EndFollow()
    {
        _isEndAutoFollow = true;
        _isSwingEndCameraDistanceToLong = true;
    }


}

