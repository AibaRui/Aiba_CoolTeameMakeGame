using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AimAssist
{
    [Header("��_��T�m����~�̔��a")]
    [SerializeField] private float _targettingRange;

    [Header("��_���^�[�Q�b�g����̂ɂ����鎞��")]
    [SerializeField] private float _targettingTime;

    [Header("�G�̎�_�̃��C���[")]
    [SerializeField] private LayerMask _enemyLayer;

    [Header("======UI=====")]
    [Header("�˒������ɖ����A�������p�l��")]
    [SerializeField] private GameObject _noTargetInAreaPanel;

    [Header("���b�N�I��������p�l��")]
    [SerializeField] private GameObject _lockOnPanel;

    [Header("���b�N�I���������̃p�l��")]
    [SerializeField] private GameObject _lockOnSuccsecPanel;

    [Header("Canvus")]
    [SerializeField] private RectTransform _parentUI;

    /// <summary>�G�̎�_��T�m�ł��Ă��邩�ǂ���</summary>
    private bool _isTargetting;

    private bool _isSuccsesTarget;

    /// <summary>���ݒT�m���Ă���G�̎�_ </summary>
    private GameObject _targettingObj;

    /// <summary>1f�O�̒T�m���Ă����G�̎�_</summary>
    private GameObject _targettedObj;

    /// <summary>�^�[�Q�b�g����܂ł̎��Ԃ��v��</summary>
    private float _countTargettingTime = 0;




    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Targetting()
    {
        if (_playerControl.InputManager.IsSetUp <= 0)
        {
            _isSuccsesTarget = false;
            _isTargetting = false;
            return;
        }   //���g���K�[�������Ă��Ȃ��Ȃ牽�����Ȃ�

        //��_��T�m
        if (SearchWeakPoints())
        {
            if (_targettingObj != _targettedObj)
            {
                if (_lockOnPanel.activeSelf) _lockOnPanel.SetActive(false);
                if (_lockOnSuccsecPanel.activeSelf) _lockOnSuccsecPanel.SetActive(false);
                _isSuccsesTarget = false;
                _countTargettingTime = 0;
            }
            Debug.Log("�G���m�F");
            if (_noTargetInAreaPanel.activeSelf) _noTargetInAreaPanel.SetActive(false);
            CountTargettingTime();
            _targettedObj = _targettingObj;
        }
        else
        {
            Debug.Log("�G�����Ȃ�");
            _isTargetting = false;
            _targettedObj = null;
        }


    }

    private void CountTargettingTime()
    {
        _countTargettingTime += Time.deltaTime;

        if (_countTargettingTime > _targettingTime && !_isSuccsesTarget)
        {
            _isSuccsesTarget = true;
            _lockOnSuccsecPanel.SetActive(true);
        }
    }

    public GameObject GetLockOnEnemy()
    {
        if (_isSuccsesTarget)
        {
            return _targettingObj;
        }
        return null;
    }

    /// <summary>
    /// ��ʓ��ɉf���Ă����ԋ߂���_��T�m����
    /// </summary>
    private bool SearchWeakPoints()
    {
        Collider[] hits = Physics.OverlapSphere(_playerControl.PlayerT.position, _targettingRange, _enemyLayer);

        if (hits.Length > 0)
        {
            List<GameObject> checkObj = new List<GameObject>();
            GameObject setObj = null;

            foreach (var a in hits)
            {
                if (IsObjectInViewport(a.gameObject))
                {
                    checkObj.Add(a.gameObject);
                }
            }   //�T�m������_�����ʓ��Ɉڂ��Ă�����̂�I�o

            if (checkObj.Count == 0)
            {
                return false;
            }   //��ʓ��ɉf���Ă��Ȃ������烍�b�N�I���s��
            else if (checkObj.Count == 1 && checkObj[0] == _targettedObj)
            {
                _isTargetting = true;
                return true;
            }   //�T�m��������1�ŁA�T�m���Ă����̂Ɠ����������炱���ŏI���ɂł���

            foreach (var a in checkObj)
            {
                if (setObj == null)
                {
                    setObj = a;
                }
                else
                {
                    float disA = Vector3.Distance(_playerControl.PlayerT.position, a.gameObject.transform.position);
                    float disB = Vector3.Distance(_playerControl.PlayerT.position, setObj.transform.position);

                    if (disA < disB)
                    {
                        setObj = a.gameObject;
                    }
                }
            }   //��ԋ����̋߂���_�����߂�

            _isTargetting = true;
            _targettingObj = setObj;
            return true;
        }
        else
        {
            _isTargetting = false;
            _targettingObj = null;
            return false;
        }
    }

    // �I�u�W�F�N�g����ʓ��ɂ��邩�ǂ����𔻒f����֐�
    bool IsObjectInViewport(GameObject obj)
    {
        // �I�u�W�F�N�g�̈ʒu�����[���h���W����r���[�|�[�g���W�ɕϊ�
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(obj.transform.position);

        // �r���[�|�[�g���W��0����1�͈͓̔����ǂ����𔻒f
        if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0)
        {
            // ��ʓ��ɂ���
            return true;
        }
        else
        {
            // ��ʊO�ɂ���
            return false;
        }
    }

    /// <summary>
    /// �A�V�X�g��UI�̕`��ݒ�
    /// </summary>
    public void AssistUISetting()
    {
        if (_playerControl.InputManager.IsSetUp <= 0)
        {
            if (_lockOnPanel.activeSelf) _lockOnPanel.SetActive(false);
            if (_lockOnSuccsecPanel.activeSelf) _lockOnSuccsecPanel.SetActive(false);
            if (_noTargetInAreaPanel.activeSelf) _noTargetInAreaPanel.SetActive(false);
            return;
        }   //�A�V�X�g�@�\���g���Ă��Ȃ������͑S�Ẵp�l�����\���ɂ���


        if (!_isTargetting)
        {
            if (_lockOnPanel.activeSelf) _lockOnPanel.SetActive(false);
            if (_lockOnSuccsecPanel.activeSelf) _lockOnSuccsecPanel.SetActive(false);
            if (!_noTargetInAreaPanel.activeSelf) _noTargetInAreaPanel.SetActive(true);
            return;
        }   //�^�[�Q�b�g��T�m�ł��Ă��Ȃ�

        //�}�[�J�[�̈ʒu���X�N���[����ʂɕϊ����ĕ\������
        var targetWorldPos = _targettingObj.transform.position;
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        _lockOnPanel.transform.position = targetScreenPos;

        var cameraDir = Camera.main.transform.forward;
        var targetDir = targetWorldPos - Camera.main.transform.position;

        var isFront = Vector3.Dot(targetDir, cameraDir) > 0;
        _lockOnPanel.SetActive(isFront);

        //var cameraTransform = Camera.main.transform;

        //// �J�����̌����x�N�g��
        //var cameraDir = cameraTransform.forward;
        //// �I�u�W�F�N�g�̈ʒu
        //var targetWorldPos = _targettingObj.transform.position + Vector3.up;
        //// �J��������^�[�Q�b�g�ւ̃x�N�g��
        //var targetDir = targetWorldPos - cameraTransform.position;

        //// ���ς��g���ăJ�����O�����ǂ����𔻒�
        //var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        //// �J�����O���Ȃ�UI�\���A����Ȃ��\��
        //_lockOnPanel.gameObject.SetActive(isFront);
        //if (!isFront)
        //{
        //    _countTargettingTime = 0;
        //    _isSuccsesTarget = false;
        //    return;
        //}
        //// �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
        //var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        //// �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    _parentUI,
        //    targetScreenPos,
        //    null,
        //    out var uiLocalPos
        //);

        //Debug.Log(uiLocalPos);
        //// RectTransform�̃��[�J�����W���X�V
        //_lockOnPanel.transform.localPosition = uiLocalPos;
    }
}