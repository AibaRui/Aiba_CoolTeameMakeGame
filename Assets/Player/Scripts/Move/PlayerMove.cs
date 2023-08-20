using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMove : IPlayerAction
{
    [Header("歩く速度")]
    [SerializeField] private float _walkSpeed = 4;

    [Header("走る速度")]
    [SerializeField] private float _runSpeed = 4;

    [Header("空中での速度")]
    [SerializeField] private float _airMoveSpeed = 4;

    [Header("ジャンプパワー")]
    [SerializeField] private float _jumpPower = 4;

    [Header("重力")]
    [SerializeField] private float _gravity = 0.9f;

    [Header("コントローラーを振動される落下速度")]
    [SerializeField] private float _vibrationSpeed = -10f;

    Vector3 velo;
    Quaternion _targetRotation;


    private bool _isSpeedDash = false;

    float _distance;

    float _startTime = 1;
    float _nowTime = 0;

    public enum MoveType
    {
        Walk,
        Run,
    }

    /// <summary>スウィング後のジャンプや、加速を使った際に呼ぶ</summary>
    public void IsUseSpeedDash()
    {
        _isSpeedDash = true;
        _distance = Vector3.Distance(_playerControl.Rb.velocity, Vector3.zero);
    }

    /// <summary>加速ダッシュによるスピードを減速する処理</summary>
    public void DownSpeedOfSppedDash()
    {
        ////減速処理が終わっているorダッシュしていない場合は処理なし
        if (!_isSpeedDash) return;

        _nowTime += Time.deltaTime;
    }

    public void Do()
    {
        Vector3 ve = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
        ////減速処理が終わっているorダッシュしていない場合は処理なし
        if (!_isSpeedDash && ve.magnitude < 25) return;

        if (_nowTime >= _startTime || ve.magnitude > 25)
        {
            _playerControl.Rb.useGravity = true;
            //Debug.Log("減速中");
            float yVelo = _playerControl.Rb.velocity.y;

            var horizontalRotation = Quaternion.AngleAxis(_playerControl.PlayerT.eulerAngles.y, Vector3.up);
            Vector3 dir = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z).normalized;


            Vector3 vea = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);

            //  _playerControl.Rb.AddForce(-dir * 15);



            Vector3 ve2 = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
            // Debug.Log(ve2.magnitude);
            if (ve2.magnitude < 20)
            {
                // Debug.Log("減速終わり");
                _isSpeedDash = false;
            }

        }
    }


    public void ReSetTime()
    {
        _nowTime = 0;
    }



    public void RotationSetting()
    {

    }

    public void Move(MoveType moveType)
    {

        //移動方向の転換速度
        float turnSpeed = 0;

        //移動速度
        float moveSpeed = 0;

        //走り方によって速度を変更
        if (moveType == MoveType.Walk)
        {
            turnSpeed = 400;
            moveSpeed = _walkSpeed;
        }
        else
        {
            turnSpeed = 300;
            moveSpeed = _runSpeed;
        }

        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        //if (_isSpeedDash)
        //{
        //    if (h == 1)
        //    {
        //        h = 0;
        //    }
        //}

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = turnSpeed * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);

        }

        if (_targetRotation.eulerAngles.y > 180)
        {
            var angle = _targetRotation.eulerAngles.y - 360;
            Quaternion rotation = Quaternion.Euler(angle * Vector3.up);
            _targetRotation = rotation;
            //    Debug.Log("DAA" + _targetRotation.eulerAngles.y);
        }

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);


        //速度を加える
        _playerControl.Rb.velocity = velo * moveSpeed;
        //重力を加える
        _playerControl.Rb.AddForce(Vector3.down * _gravity);
    }

    public void AirMove()
    {
        if(_playerControl.Rb.velocity.y< _vibrationSpeed)
        {
            _playerControl.ControllerVibrationManager.DoVibration();
        }   //コントローラーの振動

        float h = _playerControl.InputManager.HorizontalInput;
        if (_playerControl.VelocityLimit.IsSpeedUp && h > 0)
        {
            h = 0;
        }

        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = 100 * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);
        }

        //if (!_playerControl.CameraControl.IsEndAutpFollow)
        //{
        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);
        // }


        //モデルを傾かせる
        //var rotationSpeed2 = 50 * Time.deltaTime;
        //Quaternion _targetRotation2 = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        //_playerControl.ModelT.rotation = Quaternion.RotateTowards(_playerControl.ModelT.rotation, _targetRotation2, rotationSpeed2);


        float speed = 0;

        if (_playerControl.GroundCheck.IsHitNearGround())
        {
            speed = 3;
        }
        else
        {
            speed = _airMoveSpeed;

        }

        _playerControl.Rb.AddForce((velo * speed) + Vector3.down * _gravity);

        Do();
    }

    public void Jump()
    {
        Vector3 velo = new Vector3(_playerControl.Rb.velocity.x, _jumpPower, _playerControl.Rb.velocity.z);
        _playerControl.Rb.velocity = velo;
    }


}
