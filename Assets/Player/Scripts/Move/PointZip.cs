using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class PointZip
{
    [Header("移動設定---")]
    [SerializeField] private PointZipMove _zipMove;
    [Header("探知---")]
    [SerializeField] private PointZipSearch _pointZipSearch;
    [Header("UI---")]
    [SerializeField] private PointZipUI _pointZipUI;

    [Header("移動開始までの待機時間")]
    [SerializeField] private float _waitTime = 1f;

    [Header("最後のジャンプまでの待機時間")]
    [SerializeField] private float _waitLastJumpTime = 1f;

    [Header("ワイヤーの描画速度")]
    [SerializeField] private float _drowWireSpeed = 1f;

    private float _countWaitTime = 0;

    private float _countLastJumpWaitTime = 0;

    private bool _isEndPointZip = false;

    private bool _isEndWaitTime = false;

    private bool _isHitSearch = false;

    public bool IsHitSearch => _isHitSearch;

    //LineRenderに関する設定
    private float _setWireLongPercent = 0;
    private bool _isDrowWire = false;


    private PlayerControl _playerControl = null;

    public PointZipUI PointZipUI => _pointZipUI;
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
        _pointZipUI.Init(playerControl);
    }


    public bool Search()
    {
        Time.timeScale = 1f;
        //LeftTriggerを押していたら
        if (_playerControl.InputManager.LeftTrigger)
        {
            Time.timeScale = 0.5f;

            _isHitSearch = _pointZipSearch.SearchPointPointZip();

            if (_isHitSearch)
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
        Time.timeScale = 1f;

        //PointZipのUI
        _pointZipUI.SetPointZipUI(false);

        //PointZip用のカメラを使う
        _playerControl.CameraControl.UseCanera(CameraType.PointCamera);

        //Animator設定
        if (_pointZipSearch.MoveTargetPositin.y - _playerControl.PlayerT.position.y >= 0)
        {
            _playerControl.AnimControl.IsSetPointZipUp(true);
        }
        else
        {
            _playerControl.AnimControl.IsSetPointZipUp(false);
        }

        //アニメーションを再生
        _playerControl.AnimControl.IsPointZip();

        //空中に一時とどめる
        _playerControl.Rb.velocity = Vector3.zero;
        _playerControl.Rb.useGravity = false;

        _countWaitTime = 0;
        _isEndWaitTime = false;

        _countLastJumpWaitTime = 0;
        _isEndPointZip = false;

        //LineRenderに関する設定
        _playerControl.LineRenderer.positionCount = 2;
        _setWireLongPercent = 0;
        _isDrowWire = false;

        //移動時の初期設定
        _zipMove.StartSet();
    }

    public void StopPointZip()
    {
        _playerControl.Rb.useGravity = true;
    }


    /// <summary>待機時間を計測 </summary>
    public void CountWaitTime()
    {
        if (_isEndWaitTime) return;

        if (_countWaitTime >= _waitTime)
        {
            _isEndWaitTime = true;

            _isDrowWire = true;
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
            _playerControl.LineRenderer.positionCount = 0;

            _isEndPointZip = true;
            _zipMove.Lastjump();
            _playerControl.AnimControl.IsPointZipJump();
        }
        else
        {
            _countLastJumpWaitTime += Time.deltaTime;
        }
    }


    /// <summary>ワイヤーを描画する</summary>
    public void DrowWire()
    {
        if (!_isDrowWire)
        {
            return;
        }

        _setWireLongPercent += _drowWireSpeed * Time.deltaTime;
        if (_setWireLongPercent > 1)
        {
            _setWireLongPercent = 1;
        }

        float step = Vector3.Distance(_playerControl.Hads.position, _pointZipSearch.RayHitPoint) * _setWireLongPercent;
        Vector3 setPoint = Vector3.MoveTowards(_playerControl.Hads.position, _pointZipSearch.RayHitPoint, step);

        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, setPoint);
    }
}
