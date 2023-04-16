using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletMove
{
    private float _speed;

    private Vector3 _dir;

    private BulletControl _bulletControl;

    public void Init(BulletControl bulletControl,  Vector3 dir, float speed)
    {
        _bulletControl = bulletControl;
        _speed = speed;
        _dir = dir;
    }

    public void Move()
    {
        if (_bulletControl != null)
        {
            Vector3 dir = default;

            if (_bulletControl.IsEnd)
            {
                Vector3 playerDir = _bulletControl.Player.transform.position - _bulletControl.gameObject.transform.position;
                dir = playerDir.normalized;
            }
            else
            {
                dir = _dir;
            }

            _bulletControl.Rb.velocity = dir * _speed;
        }
    }
}
