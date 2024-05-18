using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Swing
{
    [Header("実行時間制限")]
    [SerializeField] private SwingLimit _swingLimit;

    public SwingLimit SwingLimit => _swingLimit;

    [Header("速度制限")]
    [SerializeField] private Vector3 _limitSpeed;
    [Header("Swingのクールタイム")]
    [SerializeField] private float _coolTime = 1f;
    [Header("降下させる時間。Swingの振り子の時間")]
    [SerializeField] private float _downTime = 2f;
    [Header("プレイヤーとワイヤーの高さが同じとする高さの補佐")]
    [SerializeField] private float _hight = 8f;
    [Header("早い速度降下した、とする速度Y")]
    [SerializeField] private float _highSpeedFallspeedY;


    [Header("=====ワイヤーの設定=====")]
    [SerializeField] private SwingJoint _swingJoint;



    [Header("[=====加速度_設定=====]")]
    [Header("入力無しの時に前に加える力")]
    [SerializeField] private float _addFrontPowerNoMove = 3;
    [Header("入力ありの時に前に加える力")]
    [SerializeField] private float _addFrontPowerMove = 5;
    [Header("降下が終わった後の上方向に加える力")]
    [SerializeField] private float _addUpPowerEndIsDown = 30;
    [Header("降下が終わった後の前方に加える力")]
    [SerializeField] private float _addFrontPowerEndIsDown = 20;
    [Header("地面近くでSwingを始めた時に、上に加える力")]
    [SerializeField] private float _addUpPowerNearGround = 5;
    [Header("地面近くでSwingを始めた時に、前に加える力")]
    [SerializeField] private float _addFrontPowerNearGround = 5;
    [Header("降下して地面近くでSwingを始めた時に、上に加える力")]
    [SerializeField] private float _addUpPowerNearGroundHighFall = 5;
    [Header("降下して地面近くでSwingを始めた時に、前に加える力")]
    [SerializeField] private float _addFrontPowerNearGroundHighFall = 60;


    [Header("[======最後のジャンプ設定======]")]
    [Header("離した時のジャンプ力_前")]
    [SerializeField] private float _addJumpPower = 4.5f;
    [Header("離した時のジャンプ力_上")]
    [SerializeField] private float _addJumpPowerUp = 4.5f;

    [Header("ジャンプボタンを押したとき_前方の力")]
    [SerializeField] private float _addJumpFrontPower = 5f;
    [Header("ジャンプボタンを押したとき_上方向の力")]
    [SerializeField] private float _addJumpFrontUpPower = 7f;
    [Header("最大限まで登った時")]
    [SerializeField] private float _addJumpUpPower = 30f;
    [Header("降下して地面近くでSwingを始めた時に、最後のジャンプで上に加える力")]
    [SerializeField] private float _addJumpUpPowerNearGroundHighFall = 20;

    /// <summary>Swingの降下をし終えたかどうか</summary>
    private bool _isDown = false;
    /// <summary>地面に近いところでSwingをしたかどうか </summary>
    private bool _isFirstNearGround = false;
    /// <summary>高速で落下して、地面に近いところでSwingをしたかどうか </summary>
    private bool _isFirstNearGroundOnHighSpeed = false;
    /// <summary>Swingを使用しるまでのクールタイムを計測 </summary>
    private float _countCoolTime = 0;


    private float _countDownTime = 0;

    private float _distanceFromPoint;

    private Vector3 velo;

    private Quaternion _targetRotation;

    private bool _isSamLine = false;

    private bool _isCanSwing = true;

    private Vector3 swingPoint;

    private bool _isSwingNow = false;

    public bool IsDown => _isDown;
    public bool IsSamLime => _isSamLine;
    public bool IsSwingNow => _isSwingNow;
    public bool IsCanSwing => _isCanSwing;
    public bool IsFirstNearGround => _isFirstNearGround;
    public bool IsFirstNearGroundHighFall => _isFirstNearGroundOnHighSpeed;
    public SwingJoint SwingJointSetting => _swingJoint;
    public float HighSpeedFallspeedY => _highSpeedFallspeedY;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _swingJoint.Init(this, playerControl);
        _swingLimit.Init(playerControl);
    }

    /// <summary>Swingの速度制限</summary>
    public void SetSpeedSwing()
    {
        _playerControl.VelocityLimit.SetLimit(_limitSpeed.x, _limitSpeed.y, -50, _limitSpeed.z);
    }



    /// <summary>スウィングの初期設定</summary>
    public void SwingSetting()
    {
        //コントローラーを振動させる
        _playerControl.ControllerVibrationManager.StartVibration(VivrationPower.Swing);

        //スウィング開始時に地面に近かったら、y軸速度を0にする
        if (_playerControl.GroundCheck.IsHitSwingGround())
        {
            if (_playerControl.Rb.velocity.y <= _highSpeedFallspeedY)
            {
                _isFirstNearGroundOnHighSpeed = true;
                _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
                _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, -5, _playerControl.Rb.velocity.z);
            }
            else
            {
                _isFirstNearGround = true;
                _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
                _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, -5, _playerControl.Rb.velocity.z);
            }
        }
        else
        {
            if (_playerControl.Rb.velocity.y > 0)
            {
                _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, -25, _playerControl.Rb.velocity.z);
            }   //連続で使用したさいに、強制的に降下させる用
            else
            {
                int power = Random.Range(5, 10);
                _playerControl.Rb.AddForce(Vector3.down * power, ForceMode.Impulse);
            }
        }

        _isSwingNow = true;

        //重力を無くす
        _playerControl.Rb.useGravity = false;

        _isSamLine = false;


        float y = _playerControl.SearchSwingPoint.RealSwingPoint.y - _playerControl.PlayerT.position.y;

        if (y < _playerControl.SearchSwingPoint.MinWireLong)
        {
            _playerControl.SearchSwingPoint.RealSwingPoint = new Vector3(_playerControl.SearchSwingPoint.RealSwingPoint.x, _playerControl.PlayerT.transform.position.y + _playerControl.SearchSwingPoint.MinWireLong, _playerControl.SearchSwingPoint.RealSwingPoint.y);
        }


        swingPoint = _playerControl.SearchSwingPoint.RealSwingPoint;




        Vector3 loapPoint = _playerControl.SearchSwingPoint.SwingPos;

        //自分とアンカーの位置の間(ジョイント)の長さ。
        _distanceFromPoint = Vector3.Distance(_playerControl.transform.position, swingPoint);

        //Jointの設定をする
        _swingJoint.SetStarSwingtJoint(swingPoint, _distanceFromPoint, loapPoint);
    }


    /// <summary>スウィング中止</summary>
    public void StopSwing(bool isJump)
    {
        //コントローラーの振動を止める
        _playerControl.ControllerVibrationManager.StopVibration();

        //Joint、ワイヤー描写の停止
        _swingJoint.StopJoint();

        //Swing不可
        _isCanSwing = false;

        _isSwingNow = false;

        _isFirstNearGround = false;
        _isFirstNearGroundOnHighSpeed = false;

        _playerControl.Rb.useGravity = true;

        _countDownTime = 0;
        _isDown = false;
    }

    public void SwingRotation()
    {
        //入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //プレイヤーの向き
        if (h != 0 || v != 0)
        {
            velo = horizontalRotation * new Vector3(h, 0, v).normalized;
            var rotationSpeed = 50 * Time.deltaTime;

            if (velo.magnitude > 0.5f)
            {
                _targetRotation = Quaternion.LookRotation(velo, Vector3.up);
            }
            _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);
        }
        else
        {
            var rotationSpeed = 50 * Time.deltaTime;

            _targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
            _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);
        }
    }

    /// <summary>Swing中の移動</summary>
    public void AddSpeed()
    {
        //コントローラーの振動
        _playerControl.ControllerVibrationManager.DoVibration();

        //入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //速度を加える方向
        Vector3 addDir = default;
        //加える速度
        float speed = 0;

        //地面に近いかどうかを確認
        if (_playerControl.GroundCheck.IsHitSwingGroundInSwing())
        {
            if (_isDown == false)
            {
                if (_playerControl.Rb.velocity.y < 0)
                {
                    _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
                }

                _swingJoint.SetJointDistance(_distanceFromPoint, _distanceFromPoint);

                _isDown = true;
            }
        }

        if (v != 0 || h != 0)
        {
            //地面に近いかどうかを確認
            if (_playerControl.GroundCheck.IsHitSwingGroundInSwing())
            {
                v = v * 0.5f;
            }
            addDir = horizontalRotation * new Vector3(h, 0, v).normalized;
            speed = _addFrontPowerMove;

        }   //入力があったら
        else
        {
            addDir = Camera.main.transform.forward;
            addDir.y = 0;

            speed = _addFrontPowerNoMove;
        } //入力なしの場合

        if (_isFirstNearGround)
        {
            _playerControl.Rb.AddForce(Vector3.up * _addUpPowerNearGround);
            speed += _addFrontPowerNearGround;
        }
        else if (_isFirstNearGroundOnHighSpeed)
        {
            _playerControl.Rb.AddForce(Vector3.up * _addUpPowerNearGroundHighFall);
            speed += _addFrontPowerNearGroundHighFall;
        }
        else if (_isDown)
        {
            _playerControl.Rb.AddForce(Vector3.up * _addUpPowerEndIsDown);
            speed += _addFrontPowerEndIsDown;
        }   //Swingの降下が終わっっていたら
        else
        {
            _countDownTime += Time.deltaTime;
            if (_downTime < _countDownTime)
            {
                _isDown = true;
                _swingJoint.SetJointDistance(_distanceFromPoint, _distanceFromPoint);
            }
        }   //Swingの降下が終わっていない

        _playerControl.Rb.AddForce(addDir.normalized * speed);
    }

    public bool CheckLine()
    {
        if (swingPoint.y - _hight <= _playerControl.PlayerT.position.y)
        {
            _isSamLine = true;
            return true;
        }
        return false;
    }

    /// <summary>Swingのクールタイムを計測</summary>
    public void CountCoolTime()
    {
        //Swingが可能だったら回さなくていい
        if (_isCanSwing) return;

        _countCoolTime += Time.deltaTime;

        //一定時間経過で、Swing可能。クールタイムをリセット
        if (_countCoolTime > _coolTime)
        {
            _isCanSwing = true;
            _countCoolTime = 0;
        }
    }

    /// <summary>Swingを不可にする</summary>
    public void SetSwingFalse()
    {
        _isCanSwing = false;
    }

    public void LastJump()
    {
        _playerControl.Rb.AddForce((_playerControl.PlayerT.forward.normalized * _addJumpPower) + (Vector3.up * _addJumpPowerUp), ForceMode.Impulse);
    }

    public void LastJumpFront()
    {
        float power = _addJumpFrontUpPower;

        if (_isFirstNearGroundOnHighSpeed)
        {
            power += _addJumpUpPowerNearGroundHighFall;
        }

        _playerControl.Rb.AddForce((_playerControl.PlayerT.forward.normalized * _addJumpFrontPower) + (Vector3.up * power), ForceMode.Impulse);
    }

    public void LastJumpUp()
    {
        float power = _addJumpUpPower;

        if (_isFirstNearGroundOnHighSpeed)
        {
            power += _addJumpUpPowerNearGroundHighFall;
        }

        _playerControl.Rb.AddForce((Vector3.up * power), ForceMode.Impulse);
    }
}
