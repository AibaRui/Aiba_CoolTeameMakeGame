using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [Header("�e�̑��x")]
    [SerializeField] private float _speed = 5;

    [Header("�e�̗L���˒�")]
    [SerializeField] private float _maxLong = 20;

    [Header("�e")]
    [SerializeField] private GameObject _bullet;

    [SerializeField] private BulletMove _bulletMove;

    private Rigidbody _rb;

    private bool _isEnd;

    private GameObject _player;

    public GameObject Bullet => _bullet;
    public GameObject Player => _player;
    public Rigidbody Rb => _rb;


    public bool IsEnd => _isEnd;

    public void Init(GameObject player, GameObject enemy, Vector3 dir)
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _player = player;

        _bulletMove.Init(this, enemy, dir, _speed);
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

    /// <summary>�򋗗����m�F���� </summary>
    public void CheckDis()
    {
        if (!_isEnd)
        {
            float dis = Vector3.Distance(_player.transform.position, transform.position);

            if (dis >= _maxLong)
            {
                _isEnd = true;
            }
        }   //�ő�򋗗��܂Ŕ��ł��邩���m�F
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