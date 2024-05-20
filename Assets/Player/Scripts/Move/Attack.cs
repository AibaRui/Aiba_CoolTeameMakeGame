using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack : IPlayerAction
{
    [Header("探知範囲")]
    [SerializeField] private Transform _muzzlePos;

    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 3;

    [Header("HitStopの速度")]
    [SerializeField] private float _hitStopSpeed = 0.1f;

    [Header("弾")]
    [SerializeField] private GameObject _bullet;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _enemyLayer = default;

    [Header("HitStopの実行時間")]
    [SerializeField] private float _slowTime = 1f;

    [Header("攻撃の実行時間")]
    [SerializeField] private float _attackTime = 2f;

    private float _coolTimeCount = 0;

    private float _timeCount = 0;

    private Collider[] _enemys;

    private bool _isCanAttack = true;

    private bool _isEndHitStop = false;

    private bool _isEndAttack = false;

    private GameObject _spownBullet;

    public bool IsCanAttack => _isCanAttack;
    public bool IsEndAttack => _isEndAttack;

    public void AttackEnter()
    {
        SpownBullet();

        _playerControl.AnimControl.Attack();

        _playerControl.CameraBrain.m_IgnoreTimeScale = true;

        Time.timeScale = _hitStopSpeed;
        _timeCount = 0;
        _isEndAttack = false;
        _isEndHitStop = false;
        //var impulseSource = _playerControl.gameObject.GetComponent<CinemachineImpulseSource>();
        //impulseSource.GenerateImpulse();

        Vector3 dir = -_playerControl.transform.forward;
        dir.y = 0.6f;
        _playerControl.Rb.velocity = dir * 60;

        //コントローラーを振動させる
        _playerControl.ControllerVibrationManager.StartVibration(VivrationPower.Power);
    }

    public void AttacFixedUpdata()
    {
        //if (!_isEndHitStop)
        //{
        //    _playerControl.Rb.velocity = Vector3.zero;
        //}
    }

    public void AttackExit()
    {

    }

    public void AttackUpdata()
    {

        _timeCount += Time.unscaledDeltaTime;

        if (_timeCount > 0.4f)
        {
            //コントローラーを振動させる
            _playerControl.ControllerVibrationManager.StopVibration();
        }

        if (!_isEndHitStop && _spownBullet != null)
        {
            _spownBullet.transform.position = _muzzlePos.position;
        }

        if (_timeCount > _slowTime && !_isEndHitStop)
        {
            AttackEnemy();
            Time.timeScale = 1f;
            _playerControl.CameraBrain.m_IgnoreTimeScale = false;
            _isEndHitStop = true;
        }



        if (_timeCount > _attackTime && !_isEndAttack)
        {
            _isEndAttack = true;
        }

    }


    public void AttackCoolTime()
    {
        if (!_isCanAttack)
        {
            _coolTimeCount += Time.deltaTime;

            if (_coolTimeCount > _coolTime)
            {
                _coolTimeCount = 0;
                _isCanAttack = true;
            }
        }
    }

    void SpownBullet()
    {
        _spownBullet = _playerControl.InstantiateObject(_bullet);
        _spownBullet.transform.position = _muzzlePos.position;

        Vector3 dir = Camera.main.transform.forward;
        _spownBullet.GetComponent<BulletControl>().Init(_playerControl.gameObject, _playerControl.AimAssist.GetLockOnEnemy(), dir);
    }

    public void AttackEnemy()
    {
        _spownBullet.GetComponent<BulletControl>().StartMove();

        _isCanAttack = false;
        _spownBullet = null;
    }

}
