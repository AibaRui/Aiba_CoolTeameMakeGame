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

    [Header("弾")]
    [SerializeField] private GameObject _bullet;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _enemyLayer = default;

    private float _coolTimeCount = 0;

    private Collider[] _enemys;

    private bool _isCanAttack = true;

    public bool IsCanAttack => _isCanAttack;



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

    public void AttackEnemy()
    {
        var bullet = _playerControl.InstantiateObject(_bullet);

        bullet.transform.position = _muzzlePos.position;

        Vector3 dir = Camera.main.transform.forward;

        bullet.GetComponent<BulletControl>().Init(_playerControl.gameObject,_playerControl.AimAssist.GetLockOnEnemy(), dir);

        _isCanAttack = false;
        Debug.Log("Attack");
    }

}
