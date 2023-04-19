using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Swing : IPlayerAction
{

    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 1f;

    [Header("速度制限")]
    [SerializeField] private Vector3 _limitSpeed;

    [Header("何もしないときに前に加える力")]
    [SerializeField] private float _addFrontPowerNoMove = 3;

    [Header("前に加える力")]
    [SerializeField] private float _addFrontPowerMove = 5;


    [Header("クールタイム")]
    [SerializeField] private float _downTime = 2f;

    private float _countDownTime = 0;

    private bool _isDown = false;

    [Header("地面に近い際の上に加える力")]
    [SerializeField] private float _addUpPowerNearGround = 5;

    [Header("空中で上に加える力")]
    [SerializeField] private float _addUpPower = 5;

    [Header("空中で上に加える時間")]
    private float _addUpTime = 2;

    [Header("最後にジャンプさせる方向")]
    [SerializeField] private Vector3 _addJumpDir;

    [Header("最後にジャンプさせる力")]
    [SerializeField] private float _addJumpPower = 4.5f;

    [Header("前方にジャンプさせる方向")]
    [SerializeField] private Vector3 _addJumpFrontDir;

    [Header("前方にジャンプさせる力")]
    [SerializeField] private float _addJumpFrontPower = 30f;

    [Header("上にジャンプさせる方向")]
    [SerializeField] private Vector3 _addJumpUpDir;

    [Header("上にジャンプさせる力")]
    [SerializeField] private float _addJumpUpPower = 30f;

    [Header("Swing中に地面につきそうになった時に加える方向")]
    [SerializeField] private Vector3 _addForceDirNearGroundOnSwing;

    [Header("Swing前に地面につきそうになった時に加える力")]
    [SerializeField] private Vector3 _addForceNearGroundNoSwing;

    private bool _isStartSwing = false;

    private float _addUpTimeCount = 0;

    [Header("Test用、固定")]
    [SerializeField] private Vector3 _swingHitPos = default;

    [Header("バネの強さ")]
    [SerializeField] private float _springPower = 4.5f;

    [Header("ダンパ-の強さ")]
    [SerializeField] private float _damperPower = 7;

    [Header("ダンパ-の強さ")]
    [SerializeField] private float _massScale = 4.5f;





    private bool _isAddEnd = false;

    private float _startSwingCount = 0;

    private float _countCoolTime = 0;

    private bool _isSamLine = false;

    private bool _isCanSwing = true;

    private Vector3 swingPoint;

    private bool _isSwingNow = false;

    private float _wireLong;
    public bool IsStartSwing => _isStartSwing;
    public bool IsSamLime => _isSamLine;
    public bool IsSwingNow => _isSwingNow;
    public bool IsCanSwing => _isCanSwing;

    float distanceFromPoint;
    Vector3 velo;
    Quaternion _targetRotation;

    private bool _isFirstNearGround = false;


    private bool _isSetEndMaxDis = false;


    /// <summary>Swingの速度制限</summary>
    public void SetSpeedSwing()
    {
        _playerControl.VelocityLimit.SetLimit(_limitSpeed.x, _limitSpeed.y, _limitSpeed.z);
    }



    /// <summary>スウィングの初期設定</summary>
    public void SwingSetting()
    {
        _isSwingNow = true;

        //重力を無くす
        _playerControl.Rb.useGravity = false;
        _isAddEnd = false;

        //スウィング開始時に地面に近かったら、y軸速度を0にする
        if (_playerControl.GroundCheck.IsHitSwingGround())
        {
            _isFirstNearGround = true;
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
        }

        _addUpTimeCount += Time.deltaTime;
        _isSamLine = false;
        //swing/grappleの設定クラスに自身のjointを渡す
        _playerControl.Joint = _playerControl.gameObject.AddComponent<SpringJoint>();
        // _playerController.PlayerSwingAndGrappleSetting.Joint = _joint;

        //アンカーの着地点。
        // var r = Camera.main.transform.TransformDirection(_swingHitPos);
        // swingPoint = r + _playerControl.PlayerT.position;
        swingPoint = _playerControl.SearchSwingPoint.SwingPos;

        //Anchor(jointをつけているオブジェクトのローカル座標  ////例)自分についてる命綱の位置)
        //connectedAnchor(アンカーのついてる点のワールド座標　////例)アンカーの先。バンジージャンプの橋の、支えているところ)

        //autoConfigureConnectedAnchorはジョイントをつけた時はOnになっており、AnchorとconnectedAnchor(アンカーの接地点)が同じ
        //つまり自分の居る位置にアンカーを刺してそこでぶら下がっている状態。なのでオフにする

        _playerControl.Joint.autoConfigureConnectedAnchor = false;
        _playerControl.Joint.connectedAnchor = swingPoint;

        //自分とアンカーの位置の間(ジョイント)の長さ。
        distanceFromPoint = Vector3.Distance(_playerControl.transform.position, swingPoint);

        //ジョイントの長さを変更(*1だとアンカーを指しても長さが縮まらないため、すぐに浮かない)
        //強制的に短くする事で引っ張られる事になる


        _playerControl.Joint.maxDistance = distanceFromPoint * 1.2f;
        // _playerControl.Joint.maxDistance = distanceFromPoint * 0.8f;


        _playerControl.Joint.minDistance = distanceFromPoint * 0.3f;


        // _playerControl.Joint.spring = 0;

        _wireLong = distanceFromPoint;

        //ばねの強さ(のび縮みのしやすさ)
        _playerControl.Joint.spring = _springPower;

        //(springの減る量)バネがビヨーンと伸びるのを繰り返してから動かなくなるまでの時間。値が多いほど短くなる
        _playerControl.Joint.damper = _damperPower;

        //質量
        _playerControl.Joint.massScale = _massScale;

        //_tipPos = _playerController.PlayerSwingAndGrappleSetting.PredictionHit.point;
        //current(意味:現在)
        //currentGrapplePosition = gunTip.position;
    }


    public void DrawLope()
    {
        if (_playerControl.Joint == null)
        {
            return;
        }
        //線を引く位置を決める
        //0は線を引く開始点
        //１は線を引く終了点
        _playerControl.LineRenderer.positionCount = 2;


        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, swingPoint);
    }

    /// <summary>スウィング中止</summary>
    public void StopSwing(bool isJump)
    {
        //Jointを消す
        _playerControl.DestroyJoint();
        //Swing不可
        _isCanSwing = false;

        _isSwingNow = false;

        _startSwingCount = 0;
        _isStartSwing = false;

        _isFirstNearGround = false;

        if (!isJump)
        {
            _playerControl.Rb.useGravity = true;
        }

        _countDownTime = 0;
        _isDown = false;
        _isSetEndMaxDis = false;
    }



    public void SwingStartCount()
    {
        if (!_isStartSwing)
        {
            _startSwingCount += Time.deltaTime;


            if ((_startSwingCount > 0.2f) && !_playerControl.GroundCheck.IsHitSwingGround())
            {
                _isStartSwing = true;
            }

        }
    }


    public void SwingCamera()
    {
        //入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //手動でカメラの角度を動かしていたら動かさない
        if (_playerControl.InputManager.IsControlCameraValueChange == Vector2.zero)
        {

            Vector3 velo2 = horizontalRotation * new Vector3(h, 0, v).normalized;
            var rSpeed = 100 * Time.deltaTime;
            var rSpeed2 = 300 * Time.deltaTime;

            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);

            Vector3 playerFoward = _playerControl.transform.forward.normalized;
            playerFoward.y = 0;

            Quaternion _targetRotationC = Quaternion.LookRotation(playerFoward, Vector3.up);
            //Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, _targetRotationC, rSpeed2);
            Debug.Log("A");

        }
    }


    /// <summary>Swing中の移動</summary>
    public void AddSpeed()
    {
        //入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //プレイヤーの向き
        if (h!=0 || v!=0)
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


        //速度を加える方向
        Vector3 addDir = default;

        //加える速度
        float speed = 0;


        //地面に近いかどうかを確認
        if (_playerControl.GroundCheck.IsHitSwingGround())
        {
            if (_isFirstNearGround == false)
            {
                if (_playerControl.Rb.velocity.y < 0)
                {
                    _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
                }
                _isFirstNearGround = true;
            }
        }

        //地面に近いときは、上方向に力を加える
        if (_isFirstNearGround)
        {
            _playerControl.Rb.AddForce(new Vector3(0, 1, 0) * _addUpPowerNearGround);
        }

        if (v != 0 || h != 0)
        {
            addDir = horizontalRotation * new Vector3(h, 0, v).normalized;
            speed = _addFrontPowerMove;

        }   //入力があったら
        else
        {
            addDir = Camera.main.transform.forward;
            speed = _addFrontPowerNoMove;
        } //入力なしの場合

        _playerControl.Rb.AddForce(addDir.normalized * speed);


        if (!_isSetEndMaxDis)
        {
            var groundDir = Mathf.Abs(swingPoint.y - _playerControl.GroundCheck.IsSwingPlayerToGroundOfLong());
            var swingPointDir = Vector3.Distance(_playerControl.transform.position, swingPoint);

            float _maxDistance = 0;

            if (groundDir - 3 <= swingPointDir)
            {
                _isSetEndMaxDis = true;
                _maxDistance = swingPointDir;
                _playerControl.Joint.maxDistance = _maxDistance;
                _playerControl.Joint.minDistance = _maxDistance;
            }
        }

        if (_isDown)
        {
            if (!_isSetEndMaxDis)
            {
                _playerControl.Joint.maxDistance = distanceFromPoint;
                _playerControl.Joint.minDistance = distanceFromPoint;
            }
            _playerControl.Rb.AddForce(Vector3.up * 20);
        }
        else
        {
            _countDownTime += Time.deltaTime;
            if (_downTime < _countDownTime)
            {
                _isDown = true;
            }
        }
    }

    public void CheckLine()
    {
        if (swingPoint.y - 8 <= _playerControl.PlayerT.position.y)
        {
            _isSamLine = true;
        }
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
        //  Debug.Log("ラスト:普通にジャンプ");
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        velo = horizontalRotation * new Vector3(h, 30, v).normalized;

        _playerControl.Rb.AddForce(velo * _addJumpPower, ForceMode.Impulse);

        _playerControl.Rb.useGravity = true;
    }

    public void LastJumpFront()
    {

        //   Debug.Log("ラスト:前にジャンプ");
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        velo = horizontalRotation * new Vector3(_addJumpFrontDir.x, _addJumpFrontDir.y, _addJumpFrontDir.z).normalized;
        _playerControl.Rb.AddForce(velo * _addJumpFrontPower, ForceMode.Impulse);


        _playerControl.Rb.useGravity = true;
    }

    public void LastJumpUp()
    {
        //_playerControl.Rb.velocity = Vector3.zero;
        // Debug.Log("ラスト:上にジャンプ");
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //velo = horizontalRotation * new Vector3(_addJumpUpDir.x, _addJumpUpDir.y, _addJumpUpDir.z).normalized;

        velo = _playerControl.Rb.velocity.normalized;

        _playerControl.Rb.AddForce(velo * _addJumpUpPower, ForceMode.Impulse);

        _playerControl.Rb.useGravity = true;
    }
}
