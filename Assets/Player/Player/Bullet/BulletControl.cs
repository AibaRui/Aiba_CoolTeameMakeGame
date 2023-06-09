using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    [SerializeField] private float _maxLong = 20;

    [SerializeField] private BulletMove _bulletMove;

    private Rigidbody _rb;

    private bool _isEnd;

    private GameObject _player;
    public GameObject Player => _player;
    public Rigidbody Rb => _rb;


    public bool IsEnd => _isEnd;

    public void Init(GameObject player, Vector3 dir)
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _player = player;

        _bulletMove.Init(this,dir, _speed);
    }

    void Update()
    {
        CheckDis();
        _bulletMove.Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isEnd = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IDamageble>(out IDamageble damageble);

        if (damageble != null)
        {
            damageble?.Damage();

        }

        _isEnd = true;
    }

    /// <summary>飛距離を確認する </summary>
    public void CheckDis()
    {
        if (!_isEnd)
        {
            float dis = Vector3.Distance(_player.transform.position, transform.position);

            if (dis >= _maxLong)
            {
                _isEnd = true;
            }
        }   //最大飛距離まで飛んでいるかを確認
        else
        {
            float dis = Vector3.Distance(_player.transform.position, transform.position);

            if (dis < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
