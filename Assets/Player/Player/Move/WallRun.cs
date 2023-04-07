using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRun : IPlayerAction
{
    [Header("歩く速度")]
    [SerializeField] private float _moveSpeed = 4;


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


    /// <summary>壁を背にする形のベクトルを求め,モデルの方向を決める</summary>
    public void MidleDir()
    {
        //壁の外積を取る
        Vector3 wallForward = GetCross(_playerControl.WallRunCheck.Hit.normal, Camera.main.transform.forward);

        //壁と垂直のベクトルをとる
        Vector3 dir = GetCross(wallForward, _playerControl.WallRunCheck.Hit.normal);

        var rotationSpeed = 400 * Time.deltaTime;

        Quaternion _targetRotation = Quaternion.LookRotation(-dir, Vector3.up);

        float y = Quaternion.Angle(_targetRotation, _playerControl.ModelT.rotation);

        if (y < 1) return;

        _playerControl.ModelT.rotation = Quaternion.RotateTowards(_playerControl.ModelT.rotation, _targetRotation, -rotationSpeed);
    }

    /// <summary>壁の方向に力を加える</summary>
    public void AddWall()
    {
        if (_playerControl.WallRunCheck.Hit.collider!=null)
        {
            //壁と平衡のベクトル。外積で求める
            Vector3 wallForward = GetCross(_playerControl.WallRunCheck.Hit.normal, Camera.main.transform.forward);

            //壁と垂直のベクトルをとる
            Vector3 dir = Vector3.Cross(wallForward, _playerControl.PlayerT.up);

            if ((_playerControl.WallRunCheck.Hit.normal - dir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -dir).magnitude)
            {
                dir = -dir;
            }

            _playerControl.Rb.AddForce(-dir * 5);
        }
    }


    /// <summary>移動方向にキャラを回転させる</summary>
    /// <returns>回転が終えたかどうか</returns>
    public bool CharactorRotateToMoveDirection()
    {
        //外積を使い、進行方向を取る
        Vector3 _wallForward = GetCross(_playerControl.WallRunCheck.Hit.normal, Camera.main.transform.forward);

        //回転速度
        var rotationSpeed = 400 * Time.deltaTime;
        //回転したい方向
        Quaternion _targetRotation = Quaternion.LookRotation(_wallForward, Vector3.up);
        //回転させる
        _playerControl.ModelT.rotation = Quaternion.RotateTowards(_playerControl.ModelT.rotation, _targetRotation, rotationSpeed);

        //現在の回転と、回転終了との角度を比べる
        float y = Quaternion.Angle(_targetRotation, _playerControl.ModelT.rotation);

        if (y < 1)
        {
            return true;
        }   //角度が1度以内にまで収まったら終了
        else
        {
            return false;
        }   //収まっていなかったら、続行
    }


    public void WallMove()
    {
        //壁と平衡のベクトル。外積で求める
        Vector3 wallForward = GetCross(_playerControl.WallRunCheck.Hit.normal, Camera.main.transform.forward);

        //壁と垂直のベクトルをとる
        Vector3 dir = Vector3.Cross(wallForward, _playerControl.PlayerT.up);

        if ((_playerControl.WallRunCheck.Hit.normal - dir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -dir).magnitude)
        {
            dir = -dir;
        }


        //外積を使い、進行方向を取る
        Vector3 _wallForward = GetCross(_playerControl.WallRunCheck.Hit.normal, Camera.main.transform.forward);

        if (CharactorRotateToMoveDirection())
        {
            if (_wallForward != null)
            {
                _playerControl.Rb.AddForce(-dir * 1);
                _playerControl.Rb.AddForce(_wallForward * _moveSpeed);
                _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x,0, _playerControl.Rb.velocity.z);
            }
        }
        else
        {
            _playerControl.Rb.velocity = Vector3.zero;
        }
    }

    public void LastJump()
    {
        if (_playerControl.WallRunCheck.TatchingWall == WallRunCheck.TatchWall.Forward)
        {
            _playerControl.Rb.AddForce(-_playerControl.PlayerT.forward * 5);
        }
        else if (_playerControl.WallRunCheck.TatchingWall == WallRunCheck.TatchWall.Right)
        {
            _playerControl.Rb.AddForce(-_playerControl.PlayerT.right * 5);
        }
        else
        {
            _playerControl.Rb.AddForce(_playerControl.PlayerT.right * 5);
        }

        _playerControl.ModelT.rotation = _playerControl.PlayerT.rotation;

    }

}
