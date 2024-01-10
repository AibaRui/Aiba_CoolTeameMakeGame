using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SwingRotation
{
    [Header("右側角度")]
    [SerializeField] private Vector3 _rightRotate = new Vector3(0, 0, 20);

    [Header("左側角度")]
    [SerializeField] private Vector3 _leftRotate = new Vector3(0, 0, 20);


    [Header("回転速度")]
    [SerializeField] private float _rotateSpeed = 100;

    [Header("戻すときの回転速度")]
    [SerializeField] private float _rotateSpeedReset = 100;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void DoModelRotate()
    {
        if (_playerControl.Rb.velocity.y >= 0) return;

        // プレイヤーの正面方向ベクトルを取得
        Vector3 playerForward = _playerControl.PlayerT.forward;

        // プレイヤーから座標へのベクトルを計算
        Vector3 playerToTarget = _playerControl.SearchSwingPoint.SwingPos - _playerControl.PlayerT.position;

        // 外積を計算して、座標が左右どちらにあるかを判断
        Vector3 crossProduct = Vector3.Cross(playerForward, playerToTarget);


        if (crossProduct.y > 0)
        {
            Quaternion r = Quaternion.Euler(_rightRotate);
            _playerControl.ModelT.localRotation = Quaternion.RotateTowards(_playerControl.ModelT.localRotation, r, _rotateSpeed * Time.deltaTime);
        }
        else if (crossProduct.y < 0)
        {
            Quaternion r = Quaternion.Euler(_leftRotate);
            _playerControl.ModelT.localRotation = Quaternion.RotateTowards(_playerControl.ModelT.localRotation, r, _rotateSpeed * Time.deltaTime);
        }
        else
        {

        }

    }

    public void ResetDoModelRotate()
    {
        Quaternion targetRotation = Quaternion.Euler(Vector3.zero);
        _playerControl.ModelT.localRotation = Quaternion.RotateTowards(_playerControl.ModelT.localRotation, targetRotation, _rotateSpeedReset * Time.deltaTime);
    }

    public void ResetModelRotate()
    {
        if (_playerControl == null) return;
        _playerControl.ModelT.localRotation = Quaternion.Euler(0, 0, 0);
    }

}
