using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRun : IPlayerAction
{
    [Header("歩く速度")]
    [SerializeField] private float _moveSpeed = 4;

    [Header("無移動時のプレイヤーの回転の速さ")]
    [SerializeField] private float _rotateSpeed = 200;

    [Header("移動時のプレイヤーの回転の速さ")]
    [SerializeField] private float _moveRotateSpeed = 20;

    private float _noMoveTimeCount = 0;

    private bool _isEndNoMove = false;

    private Vector3 _useMoveDir;

    private MoveDirection _moveDirection = MoveDirection.Up;




    public bool IsEndNoMove => _isEndNoMove;
    public Vector3 UseMoveDir => _useMoveDir;
    public MoveDirection MoveDir => _moveDirection;

    public enum MoveDirection
    {
        Up,
        Right,
        Left,

    }

    public void SetMoveDir(MoveDirection moveDirection)
    {
        _moveDirection = moveDirection;

        //外積を使い、進行方向を取る
        Vector3 wallForward = _playerControl.WallRunCheck.WallCrossRight;

        if (moveDirection == MoveDirection.Up)
        {
            _useMoveDir = Vector3.up;
        }
        else if (moveDirection == MoveDirection.Right)
        {
            _useMoveDir = wallForward;
        }
        else
        {
            _useMoveDir = -wallForward;
        }
    }

    public void CountNoMove()
    {
        if (_isEndNoMove)
        {
            float h = _playerControl.InputManager.HorizontalInput;
            float v = _playerControl.InputManager.VerticalInput;

            if (h != 0 || v > 0)
            {
                _isEndNoMove = false;
                _noMoveTimeCount = 0;
            }

            _noMoveTimeCount += Time.deltaTime;

            if (_noMoveTimeCount > 1.5f)
            {
                _isEndNoMove = false;
                _playerControl.Rb.velocity = Vector3.zero;
                _noMoveTimeCount = 0;
            }
        }
    }

    public void SetNoMove(bool set)
    {
        _isEndNoMove = set;

        if (!set)
        {
            _noMoveTimeCount = 0;
        }
    }


    /// <summary>外積を求める関数</summary>
    /// <param name="nomal"></param>
    /// <param name="foward"></param>
    /// <returns></returns>
    public Vector3 GetCross(Vector3 nomal, Vector3 foward)
    {
        //法線を取る
        Vector3 wallNomal = nomal;
        //外積を使い、進行方向を取る
        Vector3 wallForward = Vector3.Cross(wallNomal, _playerControl.PlayerT.up);

        //プラスとマイナスの外積ベクトルと自身の向いている方向を比べる。
        //近い方を進む方向とする
        if ((foward - wallForward).magnitude > (foward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        return wallForward;
    }


    /// <summary>止まっている際のプレイヤーの向きを決める</summary>
    public void MidleDir()
    {



    }

    /// <summary>壁の方向に力を加える</summary>
    public void AddWall()
    {
        if (_playerControl.WallRunCheck.Hit.collider == null)
        {
            return;
        }   //nullチェック

        //壁と平衡のベクトル。外積で求める
        Vector3 wallForward = _playerControl.WallRunCheck.WallCrossRight;

        //壁と垂直のベクトルをとる
        Vector3 dir = Vector3.Cross(wallForward, Vector3.up);

        //dirの向きが壁と反対の方向だった際には、反転させる。
        if ((_playerControl.WallRunCheck.Hit.normal - dir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -dir).magnitude)
        {
            dir = -dir;
        }

        Debug.DrawRay(_playerControl.PlayerT.position, dir, Color.blue);


        _playerControl.Rb.AddForce(-dir * 5);

        if (_playerControl.Rb.velocity.y < 0)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
        }
    }


    /// <summary>移動方向にキャラを回転させる</summary>
    public void CharactorRotateToMoveDirection(float hInput)
    {
        //外積を使い、進行方向を取る
        Vector3 wallCrossRight = _playerControl.WallRunCheck.WallCrossRight;

        //回転したい方向
        Quaternion _targetRotation = default;

        if (_moveDirection == MoveDirection.Up)
        {
            if (hInput > 0.5f)
            {
                _targetRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
            }
            else if (hInput < -0.5f)
            {
                _targetRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);
            }
            else
            {
                Vector3 dir = -_playerControl.WallRunCheck.WallDir;
                _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            }

        }
        else if (_moveDirection == MoveDirection.Right)
        {
            if (hInput < -0.5f)
            {
                Vector3 dir = -_playerControl.WallRunCheck.WallDir;
                _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            }
            else
            {
                _targetRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
            }
        }
        else if (_moveDirection == MoveDirection.Left)
        {
            if (hInput > 0.5f)
            {
                Vector3 dir = -_playerControl.WallRunCheck.WallDir;
                _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            }
            else
            {
                _targetRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);
            }
        }

        //回転させる
        Quaternion toAngle = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * _moveRotateSpeed);
        toAngle.x = 0;
        toAngle.z = 0;



        //現在の回転と、回転終了との角度を比べる
        float y = Quaternion.Angle(_targetRotation, _playerControl.PlayerT.rotation);

        if (y > 1)
        {
            _playerControl.PlayerT.rotation = toAngle;
        }   //角度が1度以内にまで収まったら終了

        Quaternion upRotation = Quaternion.LookRotation(-_playerControl.WallRunCheck.WallDir, Vector3.up);
        Quaternion rightRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
        Quaternion leftRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);

        float angleUp = Quaternion.Angle(_playerControl.PlayerT.rotation, upRotation);
        float angleRight = Quaternion.Angle(_playerControl.PlayerT.rotation, rightRotation);
        float angleLeft = Quaternion.Angle(_playerControl.PlayerT.rotation, leftRotation);

        if (angleUp <= 70)
        {
            _moveDirection = MoveDirection.Up;
            _playerControl.AnimControl.WallRunUpSet(true);
        }
        else if (angleRight <= 40)
        {
            _moveDirection = MoveDirection.Right;
            _playerControl.AnimControl.WallRunUpSet(false);
        }
        else if (angleLeft <= 70)
        {
            _moveDirection = MoveDirection.Left;
            _playerControl.AnimControl.WallRunUpSet(false);
        }

        // Debug.Log(_moveDirection);
    }


    public void WallMove()
    {
        //壁と平衡のベクトル。外積で求める
        Vector3 wallForward = Vector3.Cross(_playerControl.WallRunCheck.Hit.normal, Vector3.up);

        //壁と垂直のベクトルをとる
        Vector3 dir = Vector3.Cross(wallForward, Vector3.up);

        if ((_playerControl.WallRunCheck.Hit.normal - dir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -dir).magnitude)
        {
            dir = -dir;
        }

        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;


        Vector3 moveDir = default;

        if (_moveDirection == MoveDirection.Up)
        {
            if (h > 0.2f)
            {
                moveDir = wallForward;
            }
            else if (h < -0.2f)
            {
                moveDir = -wallForward;
            }
            else
            {
                moveDir = Vector3.up;
            }
        }
        else if (_moveDirection == MoveDirection.Right)
        {
            if (h < 0)
            {
                moveDir = Vector3.up;
            }
            else
            {
                moveDir = wallForward;
            }
        }
        else if (_moveDirection == MoveDirection.Left)
        {
            if (h > 0f)
            {
                moveDir = Vector3.up;
            }
            else
            {
                moveDir = -wallForward;
            }
        }

        //Playerの回転
          CharactorRotateToMoveDirection(h);

        float angle = Vector3.Angle(_useMoveDir, moveDir);

        float rotationAngle = Time.deltaTime * 200f;

        if ((_useMoveDir != moveDir))
        {
            Quaternion rotation = Quaternion.FromToRotation(_useMoveDir, moveDir);
            _useMoveDir = Quaternion.RotateTowards(Quaternion.identity, rotation, rotationAngle) * _useMoveDir;
        }

        //  Debug.DrawRay(_playerControl.PlayerT.position, _useMoveDir * 50, Color.green);
        //Debug.DrawRay(_playerControl.PlayerT.position, _playerControl.Rb.velocity.normalized * 40,Color.white);

        _playerControl.Rb.velocity = (_useMoveDir * _moveSpeed) + -dir * 1;

        if (_playerControl.Rb.velocity.y < 0)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
        }
    }

    public void LastJump(bool isMove)
    {
        if (isMove)
        {
            if (_playerControl.WallRun.MoveDir == MoveDirection.Up)
            {
                _playerControl.Rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            }
            else
            {
                Vector3 dir = _playerControl.Rb.velocity.normalized + _playerControl.WallRunCheck.Hit.normal;
                _playerControl.Rb.AddForce(dir * 5, ForceMode.Impulse);
            }
        }
        else
        {
            _playerControl.Rb.AddForce(_playerControl.WallRunCheck.Hit.normal * 5, ForceMode.Impulse);
        }

        _playerControl.ModelT.rotation = _playerControl.PlayerT.rotation;
    }

}
