using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZip
{
    [Header("移動設定---")]
    [SerializeField] private PointZipMove _zipMove;
    [Header("探知---")]
    [SerializeField] private PointZipSearch _pointZipSearch;


    [Header("移動開始までの待機時間")]
    [SerializeField] private float _waitTime = 1f;

    [Header("最後のジャンプまでの待機時間")]
    [SerializeField] private float _waitLastJumpTime = 1f;


    private float _countWaitTime = 0;

    private float _countLastJumpWaitTime = 0;

    private bool _isEndPointZip = false;

    private bool _isEndWaitTime = false;

    private PlayerControl _playerControl = null;

    public bool IsEndPointZip => _isEndPointZip;
    public PointZipMove PointZipMove => _zipMove;
    public bool IsEndWaitTime => _isEndWaitTime;

    public PointZipSearch PointZipSearch => _pointZipSearch;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _pointZipSearch.Init(playerControl);
        _zipMove.Init(playerControl);
    }


    public bool Search()
    {
        Time.timeScale = 1f;
        //LeftTriggerを押していたら
        if (_playerControl.InputManager.LeftTrigger)
        {
            Time.timeScale = 0.5f;

            bool isHit = _pointZipSearch.SearchPointPointZip();

            Debug.Log(isHit);

            if (isHit)
            {
                if (_playerControl.InputManager.RightTrigger)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>PointZip開始時に実行</summary>
    public void StartPointZip()
    {
        _playerControl.Rb.useGravity = false;
        Time.timeScale = 1f;

        _playerControl.AnimControl.IsPointZip();

        _playerControl.Rb.velocity = Vector3.zero;

        _countWaitTime = 0;
        _isEndWaitTime = false;

        _countLastJumpWaitTime = 0;
        _isEndPointZip = false;

        _zipMove.StartSet();
    }

    public void StopPointZip()
    {
        _playerControl.Rb.useGravity = true;

        _playerControl.AnimControl.IsPointZip();
    }


    /// <summary>待機時間を計測 </summary>
    public void CountWaitTime()
    {
        if (_isEndWaitTime) return;

        if (_countWaitTime >= _waitTime)
        {
            _isEndWaitTime = true;
        }
        else
        {
            _countWaitTime += Time.deltaTime;
        }
    }


    public void JumpCount()
    {
        if (!_zipMove.IsMoveEnd) return;

        if (_countLastJumpWaitTime >= _waitLastJumpTime)
        {
            _isEndPointZip = true;
            _zipMove.Lastjump();
            _playerControl.AnimControl.IsPointZipJump();
        }
        else
        {
            _countLastJumpWaitTime += Time.deltaTime;
        }
    }



}
