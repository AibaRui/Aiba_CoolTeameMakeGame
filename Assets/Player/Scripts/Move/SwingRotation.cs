using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SwingRotation
{
    [Header("�E���p�x")]
    [SerializeField] private Vector3 _rightRotate = new Vector3(0, 0, 20);

    [Header("�����p�x")]
    [SerializeField] private Vector3 _leftRotate = new Vector3(0, 0, 20);


    [Header("��]���x")]
    [SerializeField] private float _rotateSpeed = 100;

    [Header("�߂��Ƃ��̉�]���x")]
    [SerializeField] private float _rotateSpeedReset = 100;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void DoModelRotate()
    {
        if (_playerControl.Rb.velocity.y >= 0) return;

        // �v���C���[�̐��ʕ����x�N�g�����擾
        Vector3 playerForward = _playerControl.PlayerT.forward;

        // �v���C���[������W�ւ̃x�N�g�����v�Z
        Vector3 playerToTarget = _playerControl.SearchSwingPoint.SwingPos - _playerControl.PlayerT.position;

        // �O�ς��v�Z���āA���W�����E�ǂ���ɂ��邩�𔻒f
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