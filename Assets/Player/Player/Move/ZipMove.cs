using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipMove : IPlayerAction
{
    [Header("FrontZipの回数制限")]
    [SerializeField] private float _frontZipDoMaxCount = 2;

    [Header("FrontZipの実行時間")]
    [SerializeField] private float _frontZipTime = 1;

    [Header("前方に加速する方向")]
    [SerializeField] private Vector3 _frontZipDir = new Vector3(0, 0.1f, 1);

    [Header("1回目に前方に加速する力")]
    [SerializeField] private float _frontZipSpeedFirst = 15;

    [Header("1回目、地面に近いときに前方に加速する力")]
    [SerializeField] private float _frontZipSpeedFirstNearGround = 5;


    [Header("2回目に前方に加速する力")]
    [SerializeField] private float _frontZipSpeedSecond = 2;

    [Header("速度制限")]
    [SerializeField] private Vector3 _limitSpeed = new Vector3(20, 20, 20);

    [Header("1回目のカメラの距離")]
    [SerializeField] private float _firstCameraDistance = 7;

    [Header("1回目以降のカメラの距離")]
    [SerializeField] private float _otherCameraDistance = 5.3f;

    private float _frontZipTimeCount = 0;

    private bool _isEndFrontZip = false;

    private int _frontZipDoCount = 0;

    private bool _isCanZip = false;

    public bool IsEndFrontZip => _isEndFrontZip;

    public bool IsCanZip => _isCanZip;

    Quaternion targetRotation;

    /// <summary>前方に加速する</summary>
    public void FrontZip()
    {
        _playerControl.VelocityLimit.SetLimit(_limitSpeed.x, _limitSpeed.y, _limitSpeed.z);

        //カメラのY軸の角度
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //カメラの正面のベクトルを変える
        Vector3 dir = horizontalRotation * new Vector3(_frontZipDir.x, _frontZipDir.y, _frontZipDir.z).normalized;

        if (_frontZipDoCount == 0)
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





        if (_frontZipDoCount <= _frontZipDoMaxCount)
        {
            _isCanZip = false;
        }
    }

    public void CountFrotZipTime()
    {
        _frontZipTimeCount += Time.deltaTime;

        if (_frontZipTimeCount >= _frontZipTime)
        {
            _isEndFrontZip = true;
        }
    }

    public void ZipSetPlayerRotation()
    {
        var rotationSpeed = 300 * Time.deltaTime;

        if (Mathf.Abs(_playerControl.PlayerT.rotation.y - targetRotation.y) > 0.1f)
        {
            _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, targetRotation, rotationSpeed);
        }
    }



    /// <summary>Zipが終わった時の処理</summary>
    public void EndZip()
    {
        //Zip終了のBoolを次のZipの為にリセット
        _isEndFrontZip = false;

        //Zip実行時間計測用のタイマーをリセット
        _frontZipTimeCount = 0;

        //回数を増加
        _frontZipDoCount++;
    }


    /// <summary>Zipの実行を可能にする</summary>
    public void SetCanZip()
    {
        //Zipの実行回数をリセット
        _frontZipDoCount = 0;
        //Zipを実行可能にする
        _isCanZip = true;
    }


    public void SetCameraDistance()
    {
        targetRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);

        float cameraDistance = 0;

        if (_frontZipDoCount == 0)
        {
            cameraDistance = _firstCameraDistance;
        }
        else
        {
            cameraDistance = _otherCameraDistance;
        }

        _playerControl.CameraControl.ZipMoveCamera(cameraDistance);
    }
}
