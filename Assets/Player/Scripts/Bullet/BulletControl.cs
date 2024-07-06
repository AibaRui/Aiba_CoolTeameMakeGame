using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [Header("’e‚Ì‘¬“x")]
    [SerializeField] private float _speed = 5;

    [Header("’e‚Ì—LŒøŽË’ö")]
    [SerializeField] private float _maxLong = 20;

    [Header("’e")]
    [SerializeField] private GameObject _bullet;

    [Header("’e‚Ì—LŒøŽžŠÔ")]
    [SerializeField] private float _lifeTime = 8;

    [SerializeField] private BulletMove _bulletMove;

    private Rigidbody _rb;

    private Vector3 _startPos;

    private bool _isEnd;

    private GameObject _player;

    private bool _isInit = false;

    public GameObject Bullet => _bullet;
    public GameObject Player => _player;
    public Rigidbody Rb => _rb;


    public bool IsEnd => _isEnd;

    public void Init(GameObject player, GameObject enemy, Vector3 dir)
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _player = player;
        _startPos = _player.transform.position;

        _bulletMove.Init(this, enemy, dir, _speed);
    }

    public void StartMove()
    {
        _isInit = true;
    }

    void Update()
    {
        if (!_isInit) return;
        CheckDis();
        _bulletMove.Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isEnd = true;
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IDamageble>(out IDamageble damageble);

        if (damageble != null)
        {
            damageble?.Damage(DamageType.Player);
        }

        _isEnd = true;
    }

    /// <summary>”ò‹——£‚ðŠm”F‚·‚é </summary>
    public void CheckDis()
    {
        float dis = Vector3.Distance(_startPos, transform.position);

        if (dis >= _maxLong)
        {
            Destroy(gameObject);
        }
    }
}
