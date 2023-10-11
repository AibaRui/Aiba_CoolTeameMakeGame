using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Zip
{
    [Header("=====Zipの移動設定=====")]
    [SerializeField] private ZipMove _zipMove;

    [Header("FrontZipの回数制限")]
    [SerializeField] private float _frontZipDoMaxCount = 2;

    [Header("FrontZipの実行するまでの時間")]
    [SerializeField] private float _frontZipWaitTime = 0.4f;

    [Header("FrontZipの実行時間")]
    [SerializeField] private float _frontZipTime = 1;

    [Header("速度制限")]
    [SerializeField] private Vector3 _limitSpeed = new Vector3(20, 20, 20);


    private float _frontZipTimeCount = 0;

    private float _frontZipWaitTimeCount = 0;

    private bool _isEndFrontZip = false;

    private int _frontZipDoCount = 0;

    private bool _isZip = false;

    private bool _isCanZip = false;

    public bool IsEndFrontZip => _isEndFrontZip;

    public bool IsCanZip => _isCanZip;
    public ZipMove ZipMove => _zipMove;

    Quaternion targetRotation;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _zipMove.Init(this, playerControl);
    }


    /// <summary>前方に加速する</summary>
    public void ZipFirstSetting()
    {
        //速度設定
        _playerControl.VelocityLimit.SetLimit(_limitSpeed.x, _limitSpeed.y, -10, _limitSpeed.z);

        _playerControl.Rb.velocity = Vector3.zero;

        //Zipのコインを打つ音
        _playerControl.PlayerAudioManager.ZipAudio.ZipCoinFire();

        //回転方向の設定
        _zipMove.SetRotation();
    }


    /// <summary>Zipの実行時間を計測する</summary>
    public void CountFrotZipTime()
    {
        if (!_isZip)
        {
            _frontZipWaitTimeCount += Time.deltaTime;

            if (_frontZipWaitTimeCount >= _frontZipWaitTime)
            {
                _isZip = true;
                _playerControl.AnimControl.ZipAnim.SetDoZip(true);
                _playerControl.PlayerAudioManager.ZipAudio.ZipAudioPlay();
                _playerControl.PlayerAudioManager.MantAudio.PlayMant();
                _zipMove.ZipAddVelocity(_frontZipDoCount);
            }
        }
        else
        {
            _frontZipTimeCount += Time.deltaTime;

            if (_frontZipTimeCount >= _frontZipTime)
            {
                _isEndFrontZip = true;
            }
        }
    }



    /// <summary>Zipが終わった時の処理</summary>
    public void EndZip()
    {
        //Zip終了のBoolを次のZipの為にリセット
        _isEndFrontZip = false;

        _isZip = false;

        _frontZipWaitTimeCount = 0;

        //Zip実行時間計測用のタイマーをリセット
        _frontZipTimeCount = 0;

        //回数を増加
        _frontZipDoCount++;

        if (_frontZipDoCount <= _frontZipDoMaxCount)
        {
            _isCanZip = false;
        }

        _playerControl.AnimControl.ZipAnim.SetDoZip(false);
    }


    /// <summary>Zipの実行を可能にする</summary>
    public void SetCanZip()
    {
        //Zipの実行回数をリセット
        _frontZipDoCount = 0;
        //Zipを実行可能にする
        _isCanZip = true;
    }
}
