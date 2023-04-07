using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack : IPlayerAction
{
    [Header("探知範囲")]
    [SerializeField] private float _searchAreaRange = 4;

    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 3;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _enemyLayer = default;

    private float _coolTimeCount = 0;

    private Collider[] _enemys;

    private bool _isCanAttack = true;

    public bool IsCanAttack => _isCanAttack;

    public bool SearchEnemy()
    {
        _enemys = Physics.OverlapSphere(_playerControl.PlayerT.position, _searchAreaRange, _enemyLayer);

        if (_enemys.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
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

    public void AttackEnemy()
    {
        foreach (var e in _enemys)
        {
            e.TryGetComponent<IDamageble>(out IDamageble enemy);
            enemy?.Damage();
        }
        _isCanAttack = false;
        Debug.Log("Attack");
    }

}
