using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundCheck
{
    [Header("地面のチェック")]
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Vector3 _size;

    [Header("地面に近いことを示すBoxチェックのOffSet")]
    [SerializeField]
    private Vector3 _offsetBoxNearGround;
    [Header("地面に近いことを示すBoxチェックのBoxの大きさ")]
    [SerializeField]
    private Vector3 _sizeBoxNearGround;


    [Header("Swing中の地面のチェック")]
    [SerializeField]
    private Vector3 _offsetBoxSwing;
    [SerializeField]
    private Vector3 _sizeBoxSwing;




    [Header("箱。Swing中の地面までの距離のチェック")]
    [SerializeField]
    private Vector3 _offsetBoxSwingToGroundLong;
    [SerializeField]
    private Vector3 _sizeBoxSwingToGroundLong;

    [Header("Swing中の地面までの距離のチェック")]
    [SerializeField] private float _rayLong = 10;

    [SerializeField]
    private LayerMask _targetLayer;

    [SerializeField]
    private LayerMask _targetLayerSwingToGroundLong;

    [SerializeField]
    private bool _isDrawGizmo = true;

    public Vector3 Offset => _offset;
    public Vector3 Size => _size;
    public LayerMask TargetLayer => _targetLayer;

    private PlayerControl _playerControl;

    /// <summary>
    /// 初期化処理、このクラスを使用するときは、
    /// 最初にこの処理を実行する。
    /// </summary>
    /// <param name="origin"> 原点 </param>
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
        var posX = _playerControl.PlayerT.position.x + _offset.x;
        var posY = _playerControl.PlayerT.position.y + _offset.y;
        var posz = _playerControl.PlayerT.position.z + _offset.z;

        return Physics.OverlapBox(new Vector3(posX, posY, posz), _size, Quaternion.identity, _targetLayer);
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

    public bool IsHitNearGround()
    {
        var posX = _playerControl.PlayerT.position.x + _offsetBoxNearGround.x;
        var posY = _playerControl.PlayerT.position.y + _offsetBoxNearGround.y;
        var posZ = _playerControl.PlayerT.position.z + _offsetBoxNearGround.z;

        var hit = Physics.CheckBox(new Vector3(posX, posY, posZ), _sizeBoxNearGround, Quaternion.identity, _targetLayer);

        return hit;
    }


    public bool DistancePlayerToGround()
    {
        return Physics.Raycast(_playerControl.PlayerT.position, Vector3.down, _rayLong, _targetLayerSwingToGroundLong);
    }

    public bool IsHitSwingGround()
    {
        var posX = _playerControl.PlayerT.position.x + _offsetBoxSwing.x;
        var posY = _playerControl.PlayerT.position.y + _offsetBoxSwing.y;
        var posz = _playerControl.PlayerT.position.z + _offsetBoxSwing.z;

        return Physics.CheckBox(new Vector3(posX, posY, posz), _sizeBoxSwing, Quaternion.identity, _targetLayer);
    }

    /// <summary>地面までの距離とワイヤーの長さを比べる</summary>
    /// <param name="wireHitPoint"></param>
    /// <returns></returns>
    public float IsSwingPlayerToGroundOfLong()
    {
        var posX = _playerControl.PlayerT.position.x + _offsetBoxSwingToGroundLong.x;
        var posY = _playerControl.PlayerT.position.y + _offsetBoxSwingToGroundLong.y;
        var posz = _playerControl.PlayerT.position.z + _offsetBoxSwingToGroundLong.z;

        RaycastHit hit;

        Physics.BoxCast(new Vector3(posX, posY, posz), _sizeBoxSwingToGroundLong, Vector3.down, out hit, Quaternion.identity, Mathf.Infinity, _targetLayerSwingToGroundLong);

        return hit.point.y;
    }

    /// <summary>
    /// Gizmoに範囲を描画する
    /// </summary>
    /// <param name="origin"> 当たり判定の中央を表すTransform </param>
    public void OnDrawGizmos(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(origin.position, Vector3.down * _rayLong);

            Gizmos.color = Color.blue;
            var posXs = origin.position.x + _offsetBoxSwing.x;
            var posYs = origin.position.y + _offsetBoxSwing.y;
            var posZs = origin.position.z + _offsetBoxSwing.z;
            Gizmos.DrawWireCube(new Vector3(posXs, posYs, posZs), _sizeBoxSwing);
            


            Gizmos.color = Color.green;
            var posXNearGround = origin.position.x + _offsetBoxNearGround.x;
            var posYNearGround = origin.position.y + _offsetBoxNearGround.y;
            var posZNearGround = origin.position.z + _offsetBoxNearGround.z;
            Gizmos.DrawWireCube(new Vector3(posXNearGround, posYNearGround, posZNearGround), _sizeBoxNearGround);


            Gizmos.color = Color.red;
            var posX = origin.position.x + _offset.x;
            var posY = origin.position.y + _offset.y;
            var posz = origin.position.z + _offset.z;
            Gizmos.DrawCube(new Vector3(posX, posY, posz), _size);


            //Gizmos.color = Color.yellow;
            //var posXS = origin.position.x + _offsetBoxSwingToGroundLong.x;
            //var posYS = origin.position.y + _offsetBoxSwingToGroundLong.y;
            //var poszS = origin.position.z + _offsetBoxSwingToGroundLong.z;


            //Gizmos.DrawCube(new Vector3(posXS, posYS, poszS), _size);

        }
    }
}

