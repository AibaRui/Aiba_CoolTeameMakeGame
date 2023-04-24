using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLongAttack : MonoBehaviour
{
    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 7;

    [Header("弾の速度")]
    [SerializeField] private float _bulletSpeed = 10;

    [SerializeField] private GameObject _bullet;

    [SerializeField] private Transform[] _muzzle = new Transform[3];

    [SerializeField] private GameObject _player;

    [SerializeField] private EnemyControl _enemyControl;


    private float _countCoolTime = 0;

    private bool _isCanAttack = true;

    public void AttackCoolTime()
    {
        if (_isCanAttack) return;

        _countCoolTime += Time.deltaTime;
        if (_countCoolTime > _coolTime)
        {
            _countCoolTime = 0;
            _isCanAttack = true;
        }

    }

    public void Attack()
    {
        if (_isCanAttack)
        {
            float distance = Vector3.Distance(_player.transform.position, _enemyControl.EnemyBody.transform.position);



            if (distance < 60)
            {
                _enemyControl.EnemyAnimator.Play("Attack");

                for (int i = 0; i < 3; i++)
                {
                    Vector3 toPlayerDir = _player.transform.position - _muzzle[i].position;

                    var go = Instantiate(_bullet);
                    go.transform.position = _muzzle[i].position;

                    var rX = Random.Range(-10, 10);
                    var rY = Random.Range(-10, 10);
                    var rZ = Random.Range(-10, 10);

                    Vector3 dir = default;

                    if (i == 0)
                    {
                        dir = toPlayerDir;
                    }
                    else
                    {
                        dir = new Vector3(rX, rY, rZ) + toPlayerDir;
                    }
                    go.GetComponent<Rigidbody>().velocity = dir.normalized * _bulletSpeed;
                }
                _isCanAttack = false;
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
