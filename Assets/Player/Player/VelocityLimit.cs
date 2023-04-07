using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class VelocityLimit : IPlayerAction
{
    private float _limitX = 10;

    private float _limitY = 10;

    private float _limitZ = 10;


    public void SetLimit(float x, float y, float z)
    {
        _limitX = x;
        _limitY = y;
        _limitZ = z;
    }


    public void LimitSpeed()
    {
        if (_playerControl.Rb.velocity.x > _limitX)
        {
            _playerControl.Rb.velocity = new Vector3(_limitX, _playerControl.Rb.velocity.y, _playerControl.Rb.velocity.z);
        }
        else if (_playerControl.Rb.velocity.x < -_limitX)
        {
            _playerControl.Rb.velocity = new Vector3(-_limitX, _playerControl.Rb.velocity.y, _playerControl.Rb.velocity.z);
        }

        if (_playerControl.Rb.velocity.y > _limitY)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, _limitY, _playerControl.Rb.velocity.z);
        }
        else if (_playerControl.Rb.velocity.y < -_limitY)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, -_limitY, _playerControl.Rb.velocity.z);
        }

        if (_playerControl.Rb.velocity.z > _limitZ)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, _playerControl.Rb.velocity.y, _limitZ);
        }
        else if (_playerControl.Rb.velocity.z < -_limitZ)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, _playerControl.Rb.velocity.y, -_limitZ);
        }

    }

}
