using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNearAttack : MonoBehaviour
{
    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 5;

    [SerializeField] private GameObject _player;

    [SerializeField] private EnemyControl _enemyControl;


    private float _countCoolTime = 0;

    private bool _isCanAttack = true;

    private bool _isNearAttack = false;

    public bool IsNearAttack => _isNearAttack;

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
            int r = Random.Range(0, 3);

            _enemyControl.EnemyAnimator.SetInteger("AttackKind", r);

            _enemyControl.EnemyAnimator.SetBool("IsAttack", true);
            _isCanAttack = false;
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
