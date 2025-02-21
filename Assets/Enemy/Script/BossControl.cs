using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BossControl : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] private Transform _player;

    [Header("待機設定")]
    [SerializeField] private BossWait _wait;

    [Header("体力設定")]
    [SerializeField] private BossHp _bossHp;

    [Header("攻撃設定")]
    [SerializeField] private BossAttack _attack;

    [Header("回転設定")]
    [SerializeField] private BossRotation _rotation;

    [Header("Animator設定")]
    [SerializeField] private BossAnimatorContral _animControl;

    [SerializeField] private BossMaterialChange _materialChange;

    [SerializeField] private BossHitDirection _hit;


    [SerializeField] private Animator _anim;

    [SerializeField] private BossStateMachine _bossState;

    private bool _isMovie = false;

    public BossWait Wait => _wait;
    public bool IsMovie { get => _isMovie; set => _isMovie = value; }
    public BossAttack BossAttack => _attack;
    public BossAnimatorContral AnimControl => _animControl;
    public Animator Animator => _anim;

    public Transform Player => _player;
    public BossMaterialChange MaterialChange => _materialChange;

    public BossHitDirection HitDirection => _hit;
    public BossRotation BossRotation => _rotation;
    public BossHp BossHp => _bossHp;


    private void Awake()
    {
        _bossState.Init(this);
        _rotation.Init(this);
        _attack.Init(this);
        _animControl.Init(this);
        _bossHp.Init(this);
        _wait.Init(this);
    }

    private void Update()
    {
        _bossState.Update();
    }

    private void FixedUpdate()
    {
        _bossState.FixedUpdate();
    }

    private void LateUpdate()
    {
        _bossState.LateUpdate();
    }


    /// <summary>
    /// 映像終了、通常の動きに戻る
    /// </summary>
    public void MoveStart()
    {
        _isMovie = false;
        _anim.Play("Idle");
    }
}
