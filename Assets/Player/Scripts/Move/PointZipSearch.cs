using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZipSearch
{
    [Header("Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrawGizmo = false;

    [Header("円のサイズ")]
    [SerializeField] private float _castRadius = 5;
    [Header("最大距離")]
    [SerializeField] private float _maxDistance = 30f;

    [Header("着地点の位置補正_Y軸")]
    [SerializeField] private Vector3 _correctionOffsetY = new Vector3(0, 2, 0);

    [Header("着地点の位置補正_前面")]
    [SerializeField] private float _correctionOffsetFrontDistance = 0.2f;

    [Header("レイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("障害物となるレイヤー")]
    [SerializeField] private LayerMask _Nlayer;

    [Header("サイズ")]
    [SerializeField] private Vector3 _boxSize;

    [Header("Offset")]
    [SerializeField] private Vector3 _offSet;

    /// <summary>UIを表示する用のレイがあたったそのままの位置</summary>
    private Vector3 _rayHitPoint;

    /// <summary>移動目標地点</summary>
    private Vector3 _moveTargetPositin;

    public Vector3 RayHitPoint => _rayHitPoint;
    public Vector3 MoveTargetPositin => _moveTargetPositin;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public bool SearchPointPointZip()
    {
        //Quaternion r = Camera.main.transform.rotation;
        //Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        //Vector3 offset = rotation * _offSet;
        //var hits = Physics.OverlapBox(_playerControl.PlayerT.position + offset, _boxSize, r, _layer);

        //レイを飛ばす
        var hits = Physics.SphereCast(_playerControl.PlayerT.position, _castRadius, Camera.main.transform.forward, out RaycastHit hit, _maxDistance, _layer);

        if (hits)
        {
            //目標地点への向き
            Vector3 addF = hit.point - _playerControl.PlayerT.position;
            addF.y = 0;

            //移動目標地点
            Vector3 targetPosition = hit.point + _correctionOffsetY + addF.normalized * _correctionOffsetFrontDistance;


            Vector3 topTraget = targetPosition + new Vector3(0, _playerControl.PlayerCollider.height / 2, 0);
            Vector3 topTargetDir = topTraget - _playerControl.ModelTop.position;
            Vector3 downTraget = targetPosition + new Vector3(0, -_playerControl.PlayerCollider.height / 2, 0);
            Vector3 downTargetDir = downTraget - _playerControl.ModelDown.position;

            var topHit = Physics.Raycast(_playerControl.ModelTop.position, topTargetDir, 60, _Nlayer);
            var downHit = Physics.Raycast(_playerControl.ModelDown.position, downTargetDir, 60, _Nlayer);

            //障害物が無いので実行可能
            if (!topHit && !downHit)
            {
                _moveTargetPositin = targetPosition;
                _rayHitPoint = hit.point;

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    public void OnDrawGizmos(Transform player)
    {
        if (!_isDrawGizmo) return;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        Gizmos.color = Color.cyan;

        Quaternion cameraR = default;
        var q = Camera.main.transform.rotation.eulerAngles;
        cameraR = Quaternion.Euler(q);

        Gizmos.DrawSphere(player.transform.position + (player.transform.forward * 20), _castRadius);

        Gizmos.matrix = Matrix4x4.TRS(player.position, cameraR, player.localScale);
        Gizmos.DrawCube(_offSet, _boxSize / 2);
        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
    }
}
