using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipLineRenderer
{
    [Header("���_���̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _medal;

    [Header("���_����RigidBody")]
    [SerializeField] private Rigidbody _medalRb;

    [Header("���_���̏����ʒu")]
    [SerializeField] private Transform _medalStartPos;

    [Header("���_�����΂����x")]
    [SerializeField] private float _speed = 20;

    [Header("���C���[�̍ő勗��")]
    [SerializeField] private float _wireDistanceMax = 70;


    /// <summary>�ڕW�n�_</summary>
    private Vector3 _targetPos;

    private bool _isMoveEnd = false;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    /// <summary>Line��ݒ�</summary>
    public void StartZipLine()
    {
        //���_����\��
        _medal.gameObject.SetActive(true);

        //���_���̈ʒu��ݒ�
        _medal.transform.localPosition = _medalStartPos.position;

        _isMoveEnd = false;

        // LineRenderer�𕪊�
        _playerControl.LineRenderer.positionCount = 2;

        //�ʒu��ݒ�
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

    /// <summary>Line�̔g��ݒ�</summary>
    public void SetZipLineWave()
    {
        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, _targetPos);
    }

}