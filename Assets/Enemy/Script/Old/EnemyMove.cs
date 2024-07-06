using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3;

    [Header("探知範囲")]
    [SerializeField] private float _searchAreaRange = 20;

    [Header("プレイヤーのレイヤー")]
    [SerializeField] private LayerMask _playerLayer = default;

    [SerializeField] private GameObject _player;

    [SerializeField] private EnemyControl _enemyControl;

    public bool SarchPlayer()
    {
        Collider[] _player = Physics.OverlapSphere(_enemyControl.EnemyBody.transform.position, _searchAreaRange, _playerLayer);

        if (_player.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move()
    {
        Vector3 dir = _player.transform.position - _enemyControl.EnemyBody.transform.position;


        Quaternion _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        _targetRotation.x = 0;
        _targetRotation.z = 0;

        var rotationSpeed = 100 * Time.deltaTime;

        _enemyControl.EnemyBody.transform.rotation = Quaternion.RotateTowards(_enemyControl.EnemyBody.transform.rotation, _targetRotation, rotationSpeed);

        dir.y = 0;
        float distance = Vector3.Distance(_player.transform.position, _enemyControl.EnemyBody.transform.position);

        if (distance >= 30)
        {
            _enemyControl.Rb.velocity = dir.normalized * _moveSpeed;
            _enemyControl.EnemyAnimator.SetFloat("MoveSpeed", _moveSpeed);
        }
        else
        {
            _enemyControl.EnemyAnimator.SetFloat("MoveSpeed", 0);
        }


    }

    void Start()
    {

    }


}
