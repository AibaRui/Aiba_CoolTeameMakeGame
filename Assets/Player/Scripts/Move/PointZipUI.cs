using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointZipUI
{
    [Header("LockOn��UI")]
    [SerializeField] private GameObject _pointZipUI;

    [Header("Canvas")]
    [SerializeField] private RectTransform _parentUI;

    private PlayerControl _playerControl = null;

    /// <summary>StateMacine���Z�b�g����֐�</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetPointZipUI(bool isLockOn)
    {
        _pointZipUI.SetActive(isLockOn);
    }



    // UI�̈ʒu���X�V����
    public void UpdatePointZipUIPosition()
    {
        if (!_playerControl.InputManager.LeftTrigger) return;

        if (_playerControl.PointZip.IsHitSearch)
        {
            _pointZipUI.SetActive(true);
        }
        else
        {
            _pointZipUI.SetActive(false);
            return;
        }

        var cameraTransform = Camera.main.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;

        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = _playerControl.PointZip.PointZipSearch.RayHitPoint;

        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - _playerControl.PlayerT.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
        _pointZipUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransform�̃��[�J�����W���X�V
        _pointZipUI.transform.localPosition = uiLocalPos;
    }


}