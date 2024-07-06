using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossRotation
{

    [Header("‰ñ“]‘¬“x")]
    [SerializeField] private float _rotateSpeed = 30;

     private BossControl _bossControl;


    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void Rotate()
    {
        Vector3 toPlayerDir = _bossControl.Player.transform.position - _bossControl.transform.position;
        toPlayerDir.y = 0;

        Quaternion setR = Quaternion.LookRotation(toPlayerDir, Vector3.up);
        _bossControl.gameObject.transform.rotation = Quaternion.RotateTowards(_bossControl.gameObject.transform.rotation, setR, Time.deltaTime * _rotateSpeed);
    }

}
