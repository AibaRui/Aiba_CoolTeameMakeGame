using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

[System.Serializable]
public class PlayerDamage
{
    [Header("ボスの攻撃後、飛ぶ場所")]
    [SerializeField] private List<Transform> _damageMovePoss = new List<Transform>();

    [Header("検出の球の半径")]
    [SerializeField] private float _sphyerHalfSize = 2;

    [Header("障害物となるレイヤ-")]
    [SerializeField] private LayerMask _layer;

    [Header("大ダメージの時の移動速度")]
    [SerializeField] private float _bigDamageMoveSpeed = 10;

    [Header("ボスの中心座標")]
    [SerializeField] private Transform _bossCenterPos;

    [Header("ダメージのムービー")]
    [SerializeField] private PlayableDirector _movie1;

   // [SerializeField] private List<GameObject> _camera;


    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _medal;
    [SerializeField] private Transform _armPos;

    private bool _isDamage = false;

    /// <summary>ボスの大攻撃の際、移動する向き</summary>
    private Transform _bigDamageMovePos;

    private bool _isBossBigDamgeDeceleration = false;
    private bool _isBossBigDamgeEnd = false;
    private bool _isJump = false;

    private bool _isLineRenderer = false;
    public bool IsLineRenderer => _isLineRenderer;

    private DamageType _damageType;



    public bool IsDamage => _isDamage;
    private PlayerControl _playerControl;


    public bool IsBossBigDamageEnd => _isBossBigDamgeEnd;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    /// <summary>各ダメージに対する最初の行動設定</summary>
    /// <param name="type"></param>
    public void Damage(DamageType type)
    {
        _damageType = type;

        _isDamage = true;

        if (type == DamageType.BossBigDamage)
        {
            _isBossBigDamgeEnd = false;
            CheckMoveDirection();
            _movie1?.Play();

            var impulseSource = _playerControl.gameObject.GetComponent<CinemachineImpulseSource>();
            impulseSource.GenerateImpulse();

            //int i = Random.Range(0, 2);

            _playerControl.AnimControl.BigDamageAnim(0);
            _playerControl.SpecialHitStop.SetHitStopInfo(0,true);
        }
    }

    /// <summary>各ダメージに対するUpdata処理</summary>
    public void UpdateDamage()
    {

    }

    public void FixedDamge()
    {
        if (_damageType == DamageType.BossBigDamage)
        {
            BigBossDamage();
        }
    }

    public void Latepdata()
    {
        if (_isLineRenderer)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _medal.position);
            _lineRenderer.SetPosition(1, _armPos.position);
        }
    }

    public void OnLine(bool isOn)
    {
        if(isOn)
        {
            _isLineRenderer= true;
        }
        else
        {
            _isLineRenderer= false;
            _lineRenderer.positionCount= 0;
        }
    }

    /// <summary>ボスの大攻撃をくらった際、どこに飛ぶかを決める</summary>
    void CheckMoveDirection()
    {
        float dis = 0;

        List<Transform> list = _damageMovePoss;

        foreach (var r in _damageMovePoss)
        {
            //各移動場所へのベクトル
            Vector3 dir = r.position - _playerControl.transform.position;

            var rayHit = Physics.SphereCast(_playerControl.transform.position, _sphyerHalfSize, dir, out RaycastHit hit, Vector3.Distance(_playerControl.transform.position, r.position), _layer);

            //あたった場合は移動不可
            if (rayHit) continue;

            //距離の近い方に飛ぶ
            float d = Vector3.Distance(_playerControl.transform.position, r.position);

            if (dis == 0 || dis > d)
            {
                dis = d;
                _bigDamageMovePos = r;
            }
        }

        Vector3 moveDir = _bigDamageMovePos.position - _playerControl.transform.position;
        _playerControl.Rb.velocity = moveDir.normalized * _bigDamageMoveSpeed;
    }

    /// <summary>ボスの攻撃によって大きく吹き飛ばされる</summary>
    void BigBossDamage()
    {

        Vector3 dir = _bossCenterPos.transform.position - _playerControl.transform.position;
        dir.y = 0;
        Quaternion _targetRotation = Quaternion.LookRotation(dir, Vector3.up);

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * 40);

        if (_isBossBigDamgeEnd || _bigDamageMovePos == null || _isJump)
        {
            return;
        }
        Vector3 moveDir = _bigDamageMovePos.position - _playerControl.transform.position;


        float dis = Vector3.Distance(_playerControl.transform.position, _bigDamageMovePos.position);

        if (dis < 3)
        {
            _isBossBigDamgeEnd = true;

            _playerControl.Rb.velocity = moveDir.normalized * 1;
        }
        else if (dis < 10)
        {
            _playerControl.Rb.velocity = moveDir.normalized * 3;
        }
        else
        {
            Vector3 r = moveDir.normalized * _bigDamageMoveSpeed;
            _playerControl.Rb.velocity = new Vector3(r.x, _playerControl.Rb.velocity.y, r.z);
        }
    }

    public void BigDmameMove()
    {

    }


    public void BigDamageMoveEnd()
    {
        Vector3 dir = _bossCenterPos.position - _playerControl.transform.position;
        dir.y = 50;
        _playerControl.Rb.velocity = dir.normalized * 80;
        _isJump = true;
    }

    public void EndBigDamage()
    {
        _isDamage = false;
        _bigDamageMovePos = null;
        _isJump = false;
    }

}