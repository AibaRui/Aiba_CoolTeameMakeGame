using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    [Header("敵のアニメーター")]
    [SerializeField] private Animator _anim;

    [Header("敵")]
    [SerializeField] private GameObject _enemyBody;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private EnemyHpControl _enemyHp;

    [SerializeField] private EnemyMove _move;

    [SerializeField] private EnemyNearAttack _attack;
    [SerializeField] private EnemyLongAttack _longAttack;
    private bool _isDead;

    public bool IsDead => _isDead;

    public Rigidbody Rb => _rb;
    public GameObject EnemyBody => _enemyBody;
    public Animator EnemyAnimator => _anim;
    void Start()
    {

    }


    void Update()
    {
        if (!_isDead)
        {
            _move.Move();
            _attack.Attack();
            _longAttack.Attack();
        }
    }

    public void Dead()
    {
        _isDead = true;
        _anim.Play("Dead");
    }
}
