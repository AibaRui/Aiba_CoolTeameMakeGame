using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipMove
{
    [Header("前方に加速する方向")]
    [SerializeField] private Vector3 _frontZipDir = new Vector3(0, 0.1f, 1);

    [Header("1回目に前方に加速する力")]
    [SerializeField] private float _frontZipSpeedFirst = 15;

    [Header("1回目、地面に近いときに前方に加速する力")]
    [SerializeField] private float _frontZipSpeedFirstNearGround = 5;

    [Header("2回目に前方に加速する力")]
    [SerializeField] private float _frontZipSpeedSecond = 2;
    private Quaternion targetRotation;

    private PlayerControl _playerControl;

    private Zip _zip;


    public void Init(Zip zip, PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _zip = zip;
    }

    public void ZipFirstSetting()
    {

    }

    /// <summary>Zip時の速度</summary>
    public void ZipAddVelocity(int count)
    {
        //カメラのY軸の角度
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //カメラの正面のベクトルを変える
        Vector3 dir = horizontalRotation * new Vector3(_frontZipDir.x, _frontZipDir.y, _frontZipDir.z).normalized;

        if (count == 0)
        {
            if (_playerControl.GroundCheck.IsHitNearGround())
            {
                _playerControl.Rb.velocity = (dir * _frontZipSpeedFirstNearGround);
            }
            else
            {
                _playerControl.Rb.velocity = (dir * _frontZipSpeedFirst);
            }

        } //初回は、強く
        else
        {
            _playerControl.Rb.AddForce(dir * _frontZipSpeedSecond, ForceMode.Impulse);
        }   //2回目移行は遅い

    }


    public void SetRotation()
    {
        //向きのベクトル設定
        Vector3 dir = Camera.main.transform.localEulerAngles;
        dir.x = 0;
        dir.z = 0;
        targetRotation = Quaternion.Euler(dir);
    }

    /// <summary>Zip中のプレイヤーの角度設定</summary>
    public void ZipSetPlayerRotation()
    {
        var rotationSpeed = 300 * Time.deltaTime;

        if (Mathf.Abs(_playerControl.PlayerT.rotation.y - targetRotation.y) > 0.1f)
        {
            Quaternion setRotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, targetRotation, rotationSpeed);
            setRotation.x = 0;
            setRotation.z = 0;
            _playerControl.PlayerT.rotation = setRotation;
        }
    }

}
