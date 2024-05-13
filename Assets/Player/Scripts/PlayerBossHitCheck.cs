using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBossHitCheck
{
    [SerializeField] private bool _isDrowGizmo = false;

    [Header("下面のチェック")]
    [SerializeField] private Vector3 _offsetDown;
    [SerializeField] private Vector3 _sizeDown;
    [Header("正面のチェック")]
    [SerializeField] private Vector3 _offsetFront;
    [SerializeField] private Vector3 _sizeFront;
    [Header("上面のチェック")]
    [SerializeField] private Vector3 _offsetUp;
    [SerializeField] private Vector3 _sizeUp;


    [Header("レイヤー")]
    [SerializeField] private LayerMask _targetLayer;
    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public Collider[] GetCollider()
    {
        var posX = _playerControl.PlayerT.position.x + _offsetDown.x;
        var posY = _playerControl.PlayerT.position.y + _offsetDown.y;
        var posz = _playerControl.PlayerT.position.z + _offsetDown.z;

        return Physics.OverlapBox(new Vector3(posX, posY, posz), _sizeDown, Quaternion.identity, _targetLayer);
    }

    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public bool IsHit()
    {
        if (GetCollider().Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsHitBossDown()
    {
        var posX = _playerControl.PlayerT.position.x + _offsetDown.x;
        var posY = _playerControl.PlayerT.position.y + _offsetDown.y;
        var posZ = _playerControl.PlayerT.position.z + _offsetDown.z;

        var hit = Physics.CheckBox(new Vector3(posX, posY, posZ), _sizeDown, Quaternion.identity, _targetLayer);

        return hit;
    }

    public bool IsHitBossFront()
    {
        Quaternion rotation = Quaternion.LookRotation(_playerControl.PlayerT.forward, Vector3.up);
        Vector3 offset = rotation * _offsetFront;
        Quaternion setR = Quaternion.Euler(0, _playerControl.PlayerT.rotation.y, 0);
        Quaternion r = _playerControl.PlayerT.rotation;
        r.x = 0;
        r.z = 0;

        var hit = Physics.CheckBox(_playerControl.PlayerT.position + offset, _sizeFront, setR, _targetLayer);

        return hit;
    }

    public bool IsHitBossUp()
    {
        var posX = _playerControl.PlayerT.position.x + _offsetUp.x;
        var posY = _playerControl.PlayerT.position.y + _offsetUp.y;
        var posZ = _playerControl.PlayerT.position.z + _offsetUp.z;

        var hit = Physics.CheckBox(new Vector3(posX, posY, posZ), _sizeUp, Quaternion.identity, _targetLayer);

        return hit;
    }


    /// <summary>
    /// Gizmoに範囲を描画する
    /// </summary>
    /// <param name="origin"> 当たり判定の中央を表すTransform </param>
    public void OnDrawGizmos(Transform origin)
    {
        if (_isDrowGizmo)
        {
            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            //正面
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_offsetFront, _sizeFront * 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

            Gizmos.color = Color.red;
            var posXNearGround = origin.position.x + _offsetUp.x;
            var posYNearGround = origin.position.y + _offsetUp.y;
            var posZNearGround = origin.position.z + _offsetUp.z;
            Gizmos.DrawWireCube(new Vector3(posXNearGround, posYNearGround, posZNearGround), _sizeUp / 2);

            Gizmos.color = Color.green;
            var posXDown = origin.position.x + _offsetDown.x;
            var posYDown = origin.position.y + _offsetDown.y;
            var posZDown = origin.position.z + _offsetDown.z;
            Gizmos.DrawWireCube(new Vector3(posXDown, posYDown, posZDown), _sizeDown / 2);
        }
    }
}
