using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpUI : MonoBehaviour
{
    // �I�u�W�F�N�g
    [SerializeField] Transform _enemyTarget;

    [SerializeField] RectTransform _enemyHpUI;

    private RectTransform _parentUI;


    public void U()
    {
        var cameraTransform = Camera.main.transform;

        // �J�����̌����x�N�g��
        var cameraDir = cameraTransform.forward;
        // �I�u�W�F�N�g�̈ʒu
        var targetWorldPos = _enemyTarget.position +Vector3.up;
        // �J��������^�[�Q�b�g�ւ̃x�N�g��
        var targetDir = targetWorldPos - cameraTransform.position;

        // ���ς��g���ăJ�����O�����ǂ����𔻒�
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // �J�����O���Ȃ�UI�\���A����Ȃ��\��
       _enemyHpUI.gameObject.SetActive(isFront);
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
        _enemyHpUI.localPosition = uiLocalPos;
    }

    void Start()
    {
        _parentUI = _enemyHpUI.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
      //  U();
    }
}