using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class SwingCameraControl
{
    [Header("[=====Value�ݒ�=====]")]
    [Header("Swing�A�󒆎��̏�����Ԃ�Y�̊p�x")]
    [SerializeField] private float _firstYvalue = 0;
    [Header("Swing�A�󒆎��̏�����̂�Y�̊p�x(��̓}�C�i�X)")]
    [SerializeField] private float _maxUpValueY = -40;
    [Header("Swing�A�󒆎��̉������̂�Y�̊p�x(���̓v���X)")]
    [SerializeField] private float _maxDownValueY = 10;

    [Header("�l�̕ύX���x_��")]
    [SerializeField] private float _valueChangeSpeedUp = 0.1f;
    [Header("�l�̕ύX���x_��")]
    [SerializeField] private float _valueChangeSpeedDown = 0.1f;

    [Header("�l�̕ύX���x_��_�x��")]
    [SerializeField] private float _valueChangeLowSpeedUp = 0.1f;
    [Header("�l�̕ύX���x_��_�x��")]
    [SerializeField] private float _valueChangeLowSpeedDown = 0.1f;


    [Header("[=====Offset�ݒ�=====]")]
    [Header("Y���̐ݒ�")]
    [Header("������Ԃ�OffSet")]
    [SerializeField] private float _firstOffSet = 1.2f;
    [Header("�ő�̏������OffSet")]
    [SerializeField] private float _maxUpOffSet = 1.2f;
    [Header("Swing�W�����v���܂߂��A�ő�̏������OffSet")]
    [SerializeField] private float _maxUpJumpOffSet = 4f;
    [Header("Swing���̍ő�̉�������OffSet")]
    [SerializeField] private float _maxDownOffSet = 4f;
    [Header("�Ō�̃W�����v���ɒǉ�����l")]
    [SerializeField] private float _addUpEnd = -0.3f;
    [Header("�Ō�̃W�����v�ŏ㏸�����鑬�x")]
    [SerializeField] private float _jumpUpOffsetSpeed = 2;

    [Header("X���̐ݒ�")]
    [Header("������Ԃ�OffSet")]
    [SerializeField] private float _firstOffSetX = 1.2f;

    [Header("OffSet��߂����x")]
    [SerializeField] private float _offsetXResetSpeed = 0.1f;


    [Header("[=====Distance�ݒ�=====]")]
    [Header("�J�����̏�����Distance")]
    [SerializeField] private float _firstSwingCameraDistance = 7f;
    [Header("�J�����̒ʏ펞�̍ő��Distance")]
    [SerializeField] private float _maxSwingCameraDistance = 11f;
    [Header("�J������Swing�W�����v���̍ő��Distance")]
    [SerializeField] private float _maxSwingCameraDistanceSwingJump = 11f;

    [Header("�J������Distance���A�������鑬��")]
    [SerializeField] private float _cameraDistancePlus = 0.001f;
    [Header("�J������Distance���A�߂����鑬��")]
    [SerializeField] private float _cameraDistanceMinas = 0.002f;


    [Header("�W�����v�I�����ɉ�������Distance")]
    [SerializeField] private float _cameraDistancePlusJumpEnd = 0.003f;


    [Header("FOV�ݒ�")]
    [Header("�ʏ�")]
    [SerializeField] private float _defultFOV = 60;
    [Header("�ő�")]
    [SerializeField] private float _maxFOV = 70;
    [Header("FOV��ύX���鑬�x")]
    [SerializeField] private float _fovChecgeSpeed = 10;

    private CinemachineVirtualCamera _camera;
    private CinemachinePOV _swingCinemachinePOV;
    private CinemachineFramingTransposer _swingCameraFraming;


    /// <summary>Swing�I����OffSet</summary>
    private float _setUpEndUpOffSet;

    /// <summary>Swing�̏I��肪�A�㏸���ďI��������ǂ���</summary>
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

    /// <summary>�J�������؂�ւ�������ɌĂԁA�l�������l�ɖ߂�</summary>
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


    /// <summary>FOV��ύX����</summary>
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
        } //�ʒu��߂�

        if (_swingCameraFraming.m_TrackedObjectOffset.x > _firstOffSetX)
        {
            _swingCameraFraming.m_TrackedObjectOffset.x -= Time.deltaTime * _offsetXResetSpeed;

            if (_swingCameraFraming.m_TrackedObjectOffset.x < _firstOffSetX) _swingCameraFraming.m_TrackedObjectOffset.x = _firstOffSetX;
        } //�ʒu��߂�
    }

    /// <summary>Swing���̃X�N���[����ł̃v���C���[��OffSet��ύX����</summary>
    /// <param name="velocityY"></param>
    public void SetSwingCameraOffsetY(float velocityY)
    {
        //�X�N���[����ł̃v���C���[�̈ʒu�̕ύX
        if (velocityY > 6)
        {
            if (_swingCameraFraming.m_TrackedObjectOffset.y > _maxUpOffSet)
            {
                // Debug.Log($"�ő�:{_maxUpOffSet} ����{_swingCameraFraming.m_TrackedObjectOffset.y}");
                _swingCameraFraming.m_TrackedObjectOffset.y -= Time.deltaTime * 1.8f;
            }
        }   //�ʒu�����̕��ɉ�����
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

            } //�ʒu����̕��ɂ���
        }
    }

    public void SetAirCameraOffsetY(float velocityY)
    {
        if (_isUpEnd)//������ɔ�яオ������
        {
            //���j�^�[��ł̃v���C���[�̈ʒu��ς���B��̕���
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

                //���j�^�[��ł̃v���C���[�̈ʒu��ς���B������Ԃ�
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


                //���j�^�[��ł̃v���C���[�̈ʒu��ς���B������Ԃ�
                if (_swingCameraFraming.m_TrackedObjectOffset.y < _firstOffSet)
                    _swingCameraFraming.m_TrackedObjectOffset.y += Time.deltaTime * setSpeed;
            }
        }
    }

    /// <summary>Swing�I�����ɁAOffset�̐ݒ������</summary>
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



    /// <summary>Swing���̃J�����̋����𒲐�����</summary>
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


    /// <summary>Swing���̃J�����̏c�����̌X���𒲐�����</summary>
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

            ////////// //Y���̒���
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

    /// <summary>Y���̊p�x�𒼂�</summary>
    public void SetAirCameraVerticalAxis(float velocityY)
    {
        if (_cameraControl.IsDontCameraMove)
        {
            //�J�����̊p�x�����ɖ߂�
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