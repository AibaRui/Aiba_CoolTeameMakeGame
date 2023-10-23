using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipLineRenderer
{
    [Header("メダルのオブジェクト")]
    [SerializeField] private GameObject _medal;

    [Header("メダルのRigidBody")]
    [SerializeField] private Rigidbody _medalRb;

    [Header("メダルの初期位置")]
    [SerializeField] private Transform _medalStartPos;

    [Header("メダルを飛ばす速度")]
    [SerializeField] private float _speed = 20;

    [Header("ワイヤーの最大距離")]
    [SerializeField] private float _wireDistanceMax = 70;


    /// <summary>目標地点</summary>
    private Vector3 _targetPos;

    private bool _isMoveEnd = false;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    /// <summary>Lineを設定</summary>
    public void StartZipLine()
    {
        //メダルを表示
        _medal.gameObject.SetActive(true);

        //メダルの位置を設定
        _medal.transform.localPosition = _medalStartPos.position;

        _isMoveEnd = false;

        // LineRendererを分割
        _playerControl.LineRenderer.positionCount = 2;

        //位置を設定
        Vector3 dir = Camera.main.transform.forward;
        dir.y = 0;
        _targetPos = _playerControl.PlayerT.position + (dir * _wireDistanceMax);
    }

    public void MedalPosition()
    {
        if (_isMoveEnd)
        {
            _medal.transform.position = _targetPos;
        }
        else
        {
            Vector3 dir = _targetPos - _medal.transform.position;
            _medalRb.velocity = dir.normalized * _speed;

            if (Vector3.Distance(_medal.transform.position, _targetPos) < 0.1f)
            {
                _isMoveEnd = true;
                _medalRb.velocity = Vector3.zero;
            }
        }
    }

    public void HitMedal(Vector3 pos)
    {
        _isMoveEnd = true;
        _targetPos = pos;
    }

    public void ResetZipLine()
    {
        _playerControl.LineRenderer.positionCount = 0;
        _medal.SetActive(false);
    }

    /// <summary>Lineの波を設定</summary>
    public void SetZipLineWave()
    {
        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, _targetPos);
    }

}
