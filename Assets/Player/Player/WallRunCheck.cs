using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRunCheck : IPlayerAction
{
    [Header("Gizmoを描画するかどうか")]
    [SerializeField] private bool _isGizmo = true;

    [Header("壁のレイヤー")]
    [SerializeField]
    private LayerMask _wallLayer = default;

    [Header("横側のBoxのサイズ")]
    [SerializeField] private Vector3 _boxSizeSide = default;

    [Header("前側のBoxのサイズ")]
    [SerializeField] private Vector3 _boxSizeFront = default;

    [Header("横側のRayの補正")]
    [SerializeField] private Vector3 _frontPos = default;

    [Header("右側のRayの補正")]
    [SerializeField] private Vector3 _rightPos = default;

    [Header("左側のRayの補正")]
    [SerializeField] private Vector3 _leftPos = default;

    private TatchWall _tatchWall;

    public TatchWall TatchingWall => _tatchWall;

    /// <summary>壁のある方向</summary>
    private Vector3 _wallDir = Vector3.zero;

    private Vector3 _wallCrossRight = Vector3.zero;

    public Vector3 WallCrossRight => _wallCrossRight;

    public Vector3 WallDir => _wallDir;

    private RaycastHit _hit;

    public RaycastHit Hit => _hit;

    private bool _isWallHitRight = false;

    public bool IsWallRightHit => _isWallHitRight;

    /// <summary>接触している壁の方向</summary>
    public enum TatchWall
    {
        /// <summary>正面で接触</summary>
        Forward,
        Right,
        Left,
    }



    public bool CheckWall()
    {
        if (_tatchWall == TatchWall.Forward)
        {
            return CheckWallFront();
        }
        else if (_tatchWall == TatchWall.Left)
        {
            return CheckWallSide(false);
        }
        else
        {
            return CheckWallSide(true);
        }

    }

    public bool CheckWalAlll()
    {
        if (CheckWallFront())
        {
            return true;
        }

        if (CheckWallSide(true))
        {
            return true;
        }

        if (CheckWallSide(false))
        {
            _isWallHitRight = false;
            return true;
        }

        return false;
    }

    /// <summary>WallRun移行後の壁を検出する</summary>
    public bool CheckHitWall()
    {
        //法線を取る
        Vector3 wallNomal = _hit.normal;
        //外積を使い、進行方向を取る
        Vector3 wallForward = Vector3.Cross(wallNomal, Vector3.up);

        //壁と垂直のベクトルをとる
        Vector3 wallDir = Vector3.Cross(wallForward.normalized, Vector3.up);

        if ((_playerControl.WallRunCheck.Hit.normal - wallDir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -wallDir).magnitude)
        {
            wallDir = -wallDir;
        }

        // _wallDir = wallDir;

        _wallDir = _hit.normal;

        RaycastHit raycast;

        bool isHit = Physics.Raycast(_playerControl.PlayerT.position, -_wallDir, out raycast, 10, _wallLayer);

        if (isHit)
        {
            _hit = raycast;
        }

        _wallCrossRight = Vector3.Cross(_hit.normal, Vector3.up);

        Debug.DrawRay(_playerControl.PlayerT.position, -wallNomal * 10, Color.red);
        return isHit;
    }


    /// <summary>自身の横側の壁を検知</summary>
    /// <param name="isRight"></param>
    /// <returns></returns>
    public bool CheckWallSide(bool isRight)
    {
        Vector3 addPosSide = Vector3.zero;
        Vector3 rayDir = Vector3.zero;

        if (isRight)
        {
            addPosSide = _rightPos;
            rayDir = _playerControl.PlayerT.transform.right;
        }
        else
        {
            addPosSide = _leftPos;
            rayDir = -_playerControl.PlayerT.transform.right;
        }

        var horizontalRotation = Quaternion.AngleAxis(_playerControl.PlayerT.eulerAngles.y, Vector3.up);
        Vector3 addPos = horizontalRotation * new Vector3(addPosSide.x, addPosSide.y, addPosSide.z);

        Quaternion q = _playerControl.PlayerT.rotation;
        q.x = 0;
        q.z = 0;

        RaycastHit raycast;

        // bool isHit = Physics.BoxCast(_playerControl.PlayerT.position + addPos, _boxSizeSide, rayDir, out raycast, _playerControl.PlayerT.rotation, 0.2f, _wallLayer);

        bool isHit = Physics.Raycast(_playerControl.PlayerT.position, rayDir, out raycast, 2, _wallLayer);

        if (isHit)
        {
            _hit = raycast;
            _wallCrossRight = Vector3.Cross(_hit.normal, Vector3.up);


            if (isRight)
            {
                _playerControl.WallRun.SetMoveDir(WallRun.MoveDirection.Left);

                _tatchWall = TatchWall.Right;
                _isWallHitRight = true;
            }
            else
            {
                _playerControl.WallRun.SetMoveDir(WallRun.MoveDirection.Right);
                _tatchWall = TatchWall.Left;
                _isWallHitRight = false;
            }
        }

        return isHit;
    }

    public bool CheckWallFront()
    {
        var horizontalRotation = Quaternion.AngleAxis(_playerControl.PlayerT.eulerAngles.y, Vector3.up);
        Vector3 addPos = horizontalRotation * new Vector3(_frontPos.x, _frontPos.y, _frontPos.z);

        RaycastHit raycast;

        //bool isHit = Physics.BoxCast(_playerControl.PlayerT.position + addPos, _boxSizeFront, _playerControl.PlayerT.transform.forward, out raycast, _playerControl.PlayerT.rotation, 0.2f, _wallLayer);


        bool isHit = Physics.Raycast(_playerControl.PlayerT.position, _playerControl.PlayerT.forward, out raycast, 2, _wallLayer);

        Debug.DrawRay(_playerControl.PlayerT.position, _playerControl.PlayerT.forward * 10, Color.red);

        if (isHit)
        {
            _hit = raycast;
            _wallCrossRight = Vector3.Cross(_hit.normal, Vector3.up);


            _tatchWall = TatchWall.Forward;
            _playerControl.WallRun.SetMoveDir(WallRun.MoveDirection.Up);
        }

        return isHit;
    }

    public void OnDrawGizmos(Transform player)
    {
        if (_isGizmo)
        {
            Gizmos.color = Color.white;
            Debug.DrawRay(player.position, player.transform.forward);

            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(player.position, player.rotation, player.localScale);

            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }



}
