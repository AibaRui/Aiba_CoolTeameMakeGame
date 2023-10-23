using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class SwingCameraControl
{
    [Header("[=====Value設定=====]")]
    [Header("Swing、空中時の初期状態のYの角度")]
    [SerializeField] private float _firstYvalue = 0;
    [Header("Swing、空中時の上向きののYの角度(上はマイナス)")]
    [SerializeField] private float _maxUpValueY = -40;
    [Header("Swing、空中時の下向きののYの角度(下はプラス)")]
    [SerializeField] private float _maxDownValueY = 10;

    [Header("値の変更速度_上")]
    [SerializeField] private float _valueChangeSpeedUp = 0.1f;
    [Header("値の変更速度_下")]
    [SerializeField] private float _valueChangeSpeedDown = 0.1f;

    [Header("値の変更速度_上_遅い")]
    [SerializeField] private float _valueChangeLowSpeedUp = 0.1f;
    [Header("値の変更速度_下_遅い")]
    [SerializeField] private float _valueChangeLowSpeedDown = 0.1f;


    [Header("[=====Offset設定=====]")]
    [Header("Y軸の設定")]
    [Header("初期状態のOffSet")]
    [SerializeField] private float _firstOffSet = 1.2f;
    [Header("最大の上方向のOffSet")]
    [SerializeField] private float _maxUpOffSet = 1.2f;
    [Header("Swingジャンプも含めた、最大の上方向のOffSet")]
    [SerializeField] private float _maxUpJumpOffSet = 4f;
    [Header("Swing中の最大の下方向のOffSet")]
    [SerializeField] private float _maxDownOffSet = 4f;
    [Header("最後のジャンプ時に追加する値")]
    [SerializeField] private float _addUpEnd = -0.3f;
    [Header("最後のジャンプで上昇させる速度")]
    [SerializeField] private float _jumpUpOffsetSpeed = 2;

    [Header("X軸の設定")]
    [Header("初期状態のOffSet")]
    [SerializeField] private float _firstOffSetX = 1.2f;

    [Header("OffSetを戻す速度")]
    [SerializeField] private float _offsetXResetSpeed = 0.1f;


    [Header("[=====Distance設定=====]")]
    [Header("カメラの初期のDistance")]
    [SerializeField] private float _firstSwingCameraDistance = 7f;
    [Header("カメラの通常時の最大のDistance")]
    [SerializeField] private float _maxSwingCameraDistance = 11f;
    [Header("カメラのSwingジャンプ時の最大のDistance")]
    [SerializeField] private float _maxSwingCameraDistanceSwingJump = 11f;

    [Header("カメラのDistanceを、遠くする速さ")]
    [SerializeField] private float _cameraDistancePlus = 0.001f;
    [Header("カメラのDistanceを、近くする速さ")]
    [SerializeField] private float _cameraDistanceMinas = 0.002f;


    [Header("ジャンプ終了時に遠くするDistance")]
    [SerializeField] private float _cameraDistancePlusJumpEnd = 0.003f;


    [Header("FOV設定")]
    [Header("通常")]
    [SerializeField] private float _defultFOV = 60;
    [Header("最大")]
    [SerializeField] private float _maxFOV = 70;
    [Header("FOVを変更する速度")]
    [SerializeField] private float _fovChecgeSpeed = 10;

    private CinemachineVirtualCamera _camera;
    private CinemachinePOV _swingCinemachinePOV;
    private CinemachineFramingTransposer _swingCameraFraming;


    /// <summary>Swing終わりのOffSet</summary>
    private float _setUpEndUpOffSet;

    /// <summary>Swingの終わりが、上昇して終わったかどうか</summary>
    private bool _isUpEnd = false;

    private bool _isJumpEnd = false;

    private bool _isSwingEndCameraDistanceToLong = false;

    private CameraControl _cameraControl;

    public void Init(CameraControl cameraControl)
    {
        _cameraControl = cameraControl;

        _swingCinemachinePOV = _cameraControl.SwingCinemachinePOV;
        _swingCameraFraming = _cameraControl.SwingCameraFraming;
        _camera = _cameraControl.SwingCamera;
    }

    /// <summary>カメラが切り替わった時に呼ぶ、値を初期値に戻す</summary>
    public void ResetValues()
    {
        //_isJumpEnd = false;
        //_isUpEnd = false;

        ////FOV
        //_camera.m_Lens.FieldOfView = _defultFOV;

        ////Offset
        //_swingCameraFraming.m_TrackedObjectOffset.y = _firstOffSet;

        ////Distance
        //_swingCameraFraming.m_CameraDistance = _firstSwingCameraDistance;

        ////Value
        //_swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
    }


    public void CheckStatas()
    {
        if (_cameraControl.PlayerControl.Rb.velocity.y < 0)
        {
            _isUpEnd = false;
            _isJumpEnd = false;
        }
    }


    /// <summary>FOVを変更する</summary>
    public void ChangeFOV(bool isSwing, float velocityY)
    {
        if (isSwing)
        {
            if (velocityY < 0 && _camera.m_Lens.FieldOfView < _maxFOV)
            {
                _camera.m_Lens.FieldOfView += Time.deltaTime * _fovChecgeSpeed;
            }
            else
            {
                if (_camera.m_Lens.FieldOfView > _defultFOV)
                {
                    _camera.m_Lens.FieldOfView -= Time.deltaTime * _fovChecgeSpeed;
                }
            }
        }
        else
        {
            if (_camera.m_Lens.FieldOfView > _defultFOV)
            {
                _camera.m_Lens.FieldOfView -= Time.deltaTime * _fovChecgeSpeed;
            }
        }
    }

    public void ResetOffSetX()
    {
        if (_swingCameraFraming.m_TrackedObjectOffset.x < _firstOffSetX)
        {
            _swingCameraFraming.m_TrackedObjectOffset.x += Time.deltaTime * _offsetXResetSpeed;

            if (_swingCameraFraming.m_TrackedObjectOffset.x > _firstOffSetX) _swingCameraFraming.m_TrackedObjectOffset.x = _firstOffSetX;
        } //位置を戻す

        if (_swingCameraFraming.m_TrackedObjectOffset.x > _firstOffSetX)
        {
            _swingCameraFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offsetXResetSpeed;

            if (_swingCameraFraming.m_TrackedObjectOffset.x < _firstOffSetX) _swingCameraFraming.m_TrackedObjectOffset.x = _firstOffSetX;
        } //位置を戻す
    }

    /// <summary>Swing中のスクリーン上でのプレイヤーのOffSetを変更する</summary>
    /// <param name="velocityY"></param>
    public void SetSwingCameraOffsetY(float velocityY)
    {
        //スクリーン上でのプレイヤーの位置の変更
        if (velocityY > 6)
        {
            if (_swingCameraFraming.m_TrackedObjectOffset.y > _maxUpOffSet)
            {
                // Debug.Log($"最大:{_maxUpOffSet} 現在{_swingCameraFraming.m_TrackedObjectOffset.y}");
                _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime * 1.8f;
            }
        }   //位置を下の方に下げる
        else if (velocityY < 0)
        {
            if (_swingCameraFraming.m_TrackedObjectOffset.y < _maxDownOffSet)
            {
                if (_swingCameraFraming.m_TrackedObjectOffset.y < _firstOffSet)
                {
                    _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime * 2;
                }
                else
                {
                    _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime;
                }

            } //位置を上の方にする
        }
    }

    public void SetAirCameraOffsetY(float velocityY)
    {
        if (_isUpEnd)//上方向に飛び上がった時
        {
            //モニター上でのプレイヤーの位置を変える。上の方に
            if (_swingCameraFraming.m_TrackedObjectOffset.y > _setUpEndUpOffSet)
            {
                _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime * _jumpUpOffsetSpeed;
            }
        }
        else
        {
            if (velocityY < 0)
            {
                float speed = 0;

                if (velocityY < -5)
                {
                    speed = Time.deltaTime * 1.5f;
                }
                else if (velocityY < -3)
                {
                    speed = Time.deltaTime * 1.2f;
                }
                else if (velocityY < -1)
                {
                    speed = Time.deltaTime * 0.6f;
                }
                else
                {
                    speed = Time.deltaTime * 0.2f;
                }

                //モニター上でのプレイヤーの位置を変える。初期状態に
                if (_swingCameraFraming.m_TrackedObjectOffset.y < _maxDownOffSet)
                    _swingCameraFraming.m_TrackedObjectOffset.y += speed;
            }
            else
            {
                float setSpeed = 0;

                if (velocityY < 0)
                {
                    setSpeed = 1f;
                }
                else if (velocityY < 5)
                {
                    setSpeed = 0.5f;
                }


                //モニター上でのプレイヤーの位置を変える。初期状態に
                if (_swingCameraFraming.m_TrackedObjectOffset.y < _firstOffSet)
                    _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime * setSpeed;
            }
        }
    }

    /// <summary>Swing終了時に、Offsetの設定をする</summary>
    /// <param name="isUpEnd"></param>
    /// <param name="velocityY"></param>
    public void SetUpEndOffSet(bool isUpEnd, bool isJumpEnd, float velocityY)
    {
        _setUpEndUpOffSet = 0;

        if (isUpEnd)
        {
            _isUpEnd = true;
            _setUpEndUpOffSet = _maxUpJumpOffSet;
        }
        else if (isJumpEnd)
        {
            _isUpEnd = true;
            _isJumpEnd = true;

            _setUpEndUpOffSet = _firstOffSet - _addUpEnd;

            if (_setUpEndUpOffSet < _maxUpJumpOffSet)
            {
                _setUpEndUpOffSet = _maxUpOffSet;
            }
        }
        else
        {
            _isUpEnd = false;
            _setUpEndUpOffSet = _swingCameraFraming.m_TrackedObjectOffset.y + 0.1f;

            if (_setUpEndUpOffSet < _maxUpJumpOffSet)
            {
                _setUpEndUpOffSet = _maxUpOffSet;
            }
        }

    }



    /// <summary>Swing中のカメラの距離を調整する</summary>
    /// <param name="velocityY"></param>
    public void SetSwingCameraDistance(float velocityY)
    {
        if (_swingCameraFraming.m_CameraDistance < _maxSwingCameraDistance)
        {
            Vector3 v = new Vector3(0, velocityY, 0);
            _swingCameraFraming.m_CameraDistance += _cameraDistancePlus * Time.deltaTime;
        }
    }


    public void SetAirCameraDistance(float velocityY)
    {
        if (_isJumpEnd)
        {
            if (_swingCameraFraming.m_CameraDistance < _maxSwingCameraDistanceSwingJump)
            {
                Vector3 v = new Vector3(0, _cameraControl.PlayerControl.Rb.velocity.y, 0);
                _swingCameraFraming.m_CameraDistance += _cameraDistancePlusJumpEnd;

                if (_swingCameraFraming.m_CameraDistance > _maxSwingCameraDistanceSwingJump)
                {
                    _swingCameraFraming.m_CameraDistance = _maxSwingCameraDistanceSwingJump;
                }
            }
        }
        else
        {
            if (_swingCameraFraming.m_CameraDistance > _firstSwingCameraDistance)
            {
                Vector3 v = new Vector3(0, _cameraControl.PlayerControl.Rb.velocity.y, 0);
                _swingCameraFraming.m_CameraDistance -= _cameraDistanceMinas;

                if (_swingCameraFraming.m_CameraDistance < _firstSwingCameraDistance)
                {
                    _swingCameraFraming.m_CameraDistance = _firstSwingCameraDistance;
                }
            }
        }
    }


    /// <summary>Swing中のカメラの縦方向の傾きを調整する</summary>
    /// <param name="velocityY"></param>
    public void SetSwingCameraVerticalAxis(float velocityY)
    {
        if (_cameraControl.IsDontCameraMove)
        {
            float changeSpeedUp = _valueChangeSpeedUp;
            float changeSpeedDown = _valueChangeSpeedDown;

            if(_cameraControl.PlayerControl.Swing.IsFirstNearGround || _cameraControl.PlayerControl.Swing.IsFirstNearGroundHighFall)
            {
                changeSpeedUp = _valueChangeLowSpeedUp;
                changeSpeedDown = _valueChangeLowSpeedDown;
            }

            ////////// //Y軸の調整
            if (velocityY > 0)
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value > _maxUpValueY)
                {
                    _swingCinemachinePOV.m_VerticalAxis.Value -= changeSpeedUp * Time.deltaTime;
                }
            }
            else if (velocityY < 0)
            {
                if (_swingCinemachinePOV.m_VerticalAxis.Value <= _maxDownValueY)
                {
                    _swingCinemachinePOV.m_VerticalAxis.Value += changeSpeedDown * Time.deltaTime;
                }
            }
        }
    }

    /// <summary>Y軸の角度を直す</summary>
    public void SetAirCameraVerticalAxis(float velocityY)
    {
        if (_cameraControl.IsDontCameraMove)
        {
            //カメラの角度を元に戻す
            if (_cameraControl.PlayerControl.InputManager.IsControlCameraValueChange == Vector2.zero)
            {
                if (_cameraControl.PlayerControl.Rb.velocity.y < -15)
                {
                    //if (_swingCinemachinePOV.m_VerticalAxis.Value <= 50)
                    //{
                    //    float add = new Vector3(0, velocityY, 0).magnitude;

                    //    float limit = new Vector3(0, 20, 0).magnitude;

                    //    if (add > limit)
                    //    {
                    //        add = limit;
                    //    }
                    //    _swingCinemachinePOV.m_VerticalAxis.Value += 0.005f * add;
                    //}
                }
                else
                {
                    if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
                    {
                        _swingCinemachinePOV.m_VerticalAxis.Value -= Time.deltaTime * 15;

                        if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
                        {
                            _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
                        }
                    }
                    else if (_swingCinemachinePOV.m_VerticalAxis.Value < _firstYvalue)
                    {
                        _swingCinemachinePOV.m_VerticalAxis.Value += Time.deltaTime * 15;

                        if (_swingCinemachinePOV.m_VerticalAxis.Value > _firstYvalue)
                        {
                            _swingCinemachinePOV.m_VerticalAxis.Value = _firstYvalue;
                        }
                    }
                }
            }
        }
    }

    public void UpAirEnd()
    {
        _isUpEnd = true;
    }

    public void EndFollow()
    {
        _cameraControl.IsEndAutpFollow = true;
        _isSwingEndCameraDistanceToLong = true;
    }
}
