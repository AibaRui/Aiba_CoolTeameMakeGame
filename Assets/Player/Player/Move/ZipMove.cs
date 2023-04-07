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

    [Header("2回目に前方に加速する力")]
    [SerializeField] private float _frontZipSpeedSecond = 2;

    [Header("速度制限")]
    [SerializeField] private Vector3 _limitSpeed = new Vector3(20,20,20);



    private float _frontZipTimeCount = 0;

    private bool _isEndFrontZip = false;

    private int _frontZipDoCount = 0;

    private bool _isCanZip = false;

    public bool IsEndFrontZip => _isEndFrontZip;

    public bool IsCanZip => _isCanZip;



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
            _playerControl.Rb.velocity = (dir * _frontZipSpeedFirst);
        } //初回は、強く
        else
        {
            _playerControl.Rb.AddForce(dir * _frontZipSpeedSecond, ForceMode.Impulse);
        }   //2回目移行は遅い

        _frontZipDoCount++;
        Debug.Log(_frontZipDoCount);
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
        }else if(_frontZipTimeCount<_frontZipTime)
        { 

        }


    }

    public void ResetFrontZip(bool isResetDoCount)
    {
        _frontZipTimeCount = 0;
        _isEndFrontZip = false;

        if (isResetDoCount)
        {
            _frontZipDoCount = 0;
            _isCanZip = true;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
