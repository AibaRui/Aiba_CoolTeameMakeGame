using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossHit : MonoBehaviour
{
    [Header("ボス戦かどうか")]
    [SerializeField] private bool _isBossBattle = false;

    [Header("ボスのtag")]
    [SerializeField] private string _tagName = "Boss";

    [Header("設置判定")]
    [SerializeField] private PlayerBossHitCheck _check;

    [Header("正面、設置判定時間")]
    [SerializeField] private float _frontHittingTime = 1;
    [Header("上面、設置判定時間")]
    [SerializeField] private float _upHittingTime = 1;
    [Header("下面、設置判定時間")]
    [SerializeField] private float _downHittingTime = 1;

    private float _countTime = 0f;

    [SerializeField] private PlayerControl _playerControl;

    private PlayerBossHitType _hitType = PlayerBossHitType.Up;

    private bool _isHitting = false;

    private bool _isHitBoss = false;

    private bool _isHitEnd = false;

    private int _count = 0;

    public bool IsHitBoss => _isHitBoss;
    public PlayerBossHitCheck Check => _check;
    private void Awake()
    {
        if (_isBossBattle)
        {
            _check.Init(_playerControl);
        }
    }


    public void CheckHittingTime()
    {
        if (!_isHitting || _isHitBoss) return;

        _countTime += Time.deltaTime;

        if (_check.IsHitBossDown())
        {
            if (_countTime > _downHittingTime)
            {
                _hitType = PlayerBossHitType.Sliding;
                _playerControl.SetEventType(PlayerEventType.BossStage_HitBoss);
                _isHitBoss = true;
            }
            return;
        }

        if (_check.IsHitBossFront())
        {
            if (_countTime > _frontHittingTime)
            {
                _hitType = PlayerBossHitType.Front;
                _playerControl.SetEventType(PlayerEventType.BossStage_HitBoss);
                _isHitBoss = true;
            }
            return;
        }

        if (_check.IsHitBossUp())
        {
            if (_countTime > _upHittingTime)
            {
                _hitType = PlayerBossHitType.Up;
                _playerControl.SetEventType(PlayerEventType.BossStage_HitBoss);
                _isHitBoss = true;
            }
            return;
        }
    }

    /// <summary>State移行の初回に呼ぶ</summary>
    public void EnterBossHit()
    {
        if (_hitType == PlayerBossHitType.Up)
        {
            _playerControl.AnimControl.BossHit(PlayerBossHitType.Up, false);
        }
        else if (_hitType == PlayerBossHitType.Front)
        {
            _playerControl.AnimControl.BossHit(PlayerBossHitType.Front, false);
        }
        else if (_hitType == PlayerBossHitType.Sliding)
        {
            _playerControl.AnimControl.BossHit(PlayerBossHitType.Sliding, false);
        }

        Debug.Log(_hitType);
    }

    public void UpdataBossHit()
    {

    }

    public void FixedBossHit()
    {
        if (_hitType == PlayerBossHitType.Sliding)
        {
            if (_playerControl.InputManager.HorizontalInput != 0 || _playerControl.InputManager.VerticalInput != 0)
                _playerControl.Move.AirMove();
            //速度の減衰
            _playerControl.VelocityLimit.SlowToSpeedUp();

            //左右回転設定
            _playerControl.PlayerModelRotation.ResetDoModelRotate();
        }


    }

    /// <summary>Animationから呼ぶ</summary>
    public void BossHitEnd()
    {
        _isHitBoss = false;
        _countTime = 0;
    }

    public void BackAddSpeed()
    {
        _playerControl.Rb.velocity = Vector3.zero;
        Vector3 dir = -_playerControl.gameObject.transform.forward;
        dir.y += 1f;

        _playerControl.Rb.AddForce(dir * 20);
    }

    public void FrontAddSpeed()
    {
        _playerControl.Rb.velocity = Vector3.zero;
        Vector3 dir = _playerControl.gameObject.transform.forward;
        dir.y += 0.8f;

        _playerControl.Rb.velocity = (dir * 40);
    }

    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == _tagName)
        {

            _isHitting = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == _tagName)
        {

            _isHitting = false;
            _countTime = 0;

            if (_hitType == PlayerBossHitType.Sliding)
            {
                _playerControl.AnimControl.BossHit(PlayerBossHitType.Sliding, true);
            }

        }
    }

    private void OnDrawGizmos()
    {
        _check.OnDrawGizmos(_playerControl.PlayerT);
    }

}

public enum PlayerBossHitType
{
    Sliding,
    Front,
    Up,

}
