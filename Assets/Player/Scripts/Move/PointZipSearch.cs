using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZipSearch
{
    [Header("Gizmo��\�����邩�ǂ���")]
    [SerializeField] private bool _isDrawGizmo = false;

    [Header("�~�̃T�C�Y")]
    [SerializeField] private float _castRadius = 5;
    [Header("�ő勗��")]
    [SerializeField] private float _maxDistance = 30f;

    [Header("���n�_�̈ʒu�␳_Y��")]
    [SerializeField] private Vector3 _correctionOffsetY = new Vector3(0, 2, 0);

    [Header("���n�_�̈ʒu�␳_�O��")]
    [SerializeField] private float _correctionOffsetFrontDistance = 0.2f;

    [Header("���C���[")]
    [SerializeField] private LayerMask _layer;

    [Header("��Q���ƂȂ郌�C���[")]
    [SerializeField] private LayerMask _Nlayer;

    [Header("�T�C�Y")]
    [SerializeField] private Vector3 _boxSize;

    [Header("Offset")]
    [SerializeField] private Vector3 _offSet;

    /// <summary>UI��\������p�̃��C�������������̂܂܂̈ʒu</summary>
    private Vector3 _rayHitPoint;

    /// <summary>�ړ��ڕW�n�_</summary>
    private Vector3 _moveTargetPositin;

    public Vector3 RayHitPoint => _rayHitPoint;
    public Vector3 MoveTargetPositin => _moveTargetPositin;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
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

        //���C���΂�
        var hits = Physics.SphereCast(_playerControl.PlayerT.position, _castRadius, Camera.main.transform.forward, out RaycastHit hit, _maxDistance, _layer);

        if (hits)
        {
            //�ڕW�n�_�ւ̌���
            Vector3 addF = hit.point - _playerControl.PlayerT.position;
            addF.y = 0;

            //�ړ��ڕW�n�_
            Vector3 targetPosition = hit.point + _correctionOffsetY + addF.normalized * _correctionOffsetFrontDistance;


            Vector3 topTraget = targetPosition + new Vector3(0, _playerControl.PlayerCollider.height / 2, 0);
            Vector3 topTargetDir = topTraget - _playerControl.ModelTop.position;
            Vector3 downTraget = targetPosition + new Vector3(0, -_playerControl.PlayerCollider.height / 2, 0);
            Vector3 downTargetDir = downTraget - _playerControl.ModelDown.position;

            var topHit = Physics.Raycast(_playerControl.ModelTop.position, topTargetDir, 60, _Nlayer);
            var downHit = Physics.Raycast(_playerControl.ModelDown.position, downTargetDir, 60, _Nlayer);

            //��Q���������̂Ŏ��s�\
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