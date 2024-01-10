using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZipMove
{
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed = 20f;

    [Header("回転速度")]
    [SerializeField] private float _rotateSpeed = 50f;

    [Header("ジャンプの向き")]
    [SerializeField] private Vector3 _lastJumpDir = new Vector3(0, 1, 0);

    [Header("ジャンプパワー")]
    [SerializeField] private float _lastjumpPower = 30;

    private bool _isMoveEnd = false;

    private Vector3 _targetDir = default;
    private Vector3 _targetPosition = default;

    public bool IsMoveEnd => _isMoveEnd;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void StartSet()
    {
        _isMoveEnd = false;
        _targetDir = _playerControl.PointZip.PointZipSearch.MoveTargetPositin - _playerControl.PlayerT.position;
        _targetPosition = _playerControl.PointZip.PointZipSearch.MoveTargetPositin;
    }

    public void SetRotation()
    {
        if (_isMoveEnd) return;

        Vector3 dir = _targetDir;
        dir.y = 0;

        Quaternion targetR = Quaternion.LookRotation(dir);
        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, targetR, Time.deltaTime * _rotateSpeed);
    }


    public void Move()
    {
        if (!_playerControl.PointZip.IsEndWaitTime || _isMoveEnd) return;

        _playerControl.Rb.velocity = _targetDir.normalized * _moveSpeed;

        if (Vector3.Distance(_targetPosition, _playerControl.PlayerT.position) < 1f)
        {
            _isMoveEnd = true;
            _playerControl.Rb.velocity = Vector3.zero;

            _playerControl.AnimControl.IsPointZipMoveEndTrigger();
        }
    }

    public void Lastjump()
    {
        Vector3 d = _targetDir;
        _targetDir.y = 0;
        Vector3 dir = d + _lastJumpDir;
        _playerControl.Rb.AddForce(dir.normalized * _lastjumpPower, ForceMode.Impulse);
    }


}
