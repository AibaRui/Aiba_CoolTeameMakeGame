using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 5;

    [SerializeField] private GameObject _player;

    [SerializeField] private EnemyControl _enemyControl;


    private float _countCoolTime = 0;

    private bool _isCanAttack = true;

    public void AttackCoolTime()
    {
        if (_isCanAttack) return;

        _countCoolTime += Time.deltaTime;
        if(_countCoolTime>_coolTime)
        {
            _countCoolTime = 0;
            _isCanAttack = true;
        }

    }

    public void Attack()
    {
        if(_isCanAttack)
        {

            float distance = Vector3.Distance(_player.transform.position, _enemyControl.EnemyBody.transform.position);

            if (distance < 20)
            {
                _enemyControl.EnemyAnimator.Play("Attack");
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        AttackCoolTime();
    }
}
