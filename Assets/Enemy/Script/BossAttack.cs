using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttack
{
    [Header("�����ʒu")]
    [SerializeField] private Transform _highPos;

    [Header("���x�́A�ʒu")]
    [SerializeField] private Transform _middlePos;

    [Header("�Ⴂ�ʒu")]
    [SerializeField] private Transform _lowPos;

    [Header("�U������܂ł̊��o")]
    [SerializeField] private float _attackCoolTime = 10;

    [Header("���r�̓����蔻��")]
    [SerializeField] private GameObject _leftArmAttackCollider;
    [Header("����̓����蔻��")]
    [SerializeField] private GameObject _leftHandAttackCollider;

    [Header("�E�r�̓����蔻��")]
    [SerializeField] private GameObject _rightArmAttackCollider;
    [Header("�E��̓����蔻��")]
    [SerializeField] private GameObject _rightHandAttackCollider;


    private BossControl _bossControl;

    private float _timeCount = 0;

    private bool _isAttackNow = false;

    private bool _isAttack = false;

    public bool IsAttack => _isAttack;
    public bool IsAttackNow => _isAttackNow;
    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void AttackEnter()
    {
        Debug.Log("Enter");
    }

    public void AttackExit()
    {
        _isAttack = false;
        _isAttackNow = false;
        _timeCount = 0;
    }

    public void AttackEnd()
    {
        _isAttackNow = false;

        _rightArmAttackCollider.SetActive(false);
        _rightHandAttackCollider.SetActive(false);
        _leftHandAttackCollider.SetActive(false);
        _leftArmAttackCollider.SetActive(false);
    }

    public void AttackCoolTimeCount()
    {
        _timeCount += Time.deltaTime;
        if (_timeCount >= _attackCoolTime)
        {
            _isAttack = true;
            _isAttackNow = true;
        }

    }

    public void Attack()
    {
        // �I�u�W�F�N�gA���猩���I�u�W�F�N�gB�̕���
        Vector3 toOther = _bossControl.Player.transform.position - _bossControl.transform.position;

        // ���K�����ĕ����x�N�g���ɂ���
        Vector3 toOtherNormalized = toOther.normalized;

        // �I�u�W�F�N�gA�̐��ʕ����Ƃ̊p�x���v�Z����
        float angle = Vector3.Angle(_bossControl.transform.forward, toOtherNormalized);

        // ���ʂ��獶�E90�x�ȓ��ɂ��邩�ǂ������m�F
        if (angle <= 90)
        {
            // �I�u�W�F�N�gB���E���ɂ��邩�����ɂ��邩�𔻒f����
            float dotProduct = Vector3.Dot(_bossControl.transform.right, toOther);

            if (dotProduct > 0)
            {
                _rightArmAttackCollider.SetActive(true);
                _rightHandAttackCollider.SetActive(true);
                _bossControl.AnimControl.Attack(CheckHigh(), true);
            }
            else if (dotProduct < 0)
            {
                _leftArmAttackCollider.SetActive(true);
                _leftHandAttackCollider.SetActive(true);
                _bossControl.AnimControl.Attack(CheckHigh(), false);
            }
            else
            {
                _bossControl.AnimControl.Attack(BossAttackKind.Front, false);
            }
        }
        else
        {
            _bossControl.AnimControl.Attack(BossAttackKind.Back, false);
        }
    }

    public BossAttackKind CheckHigh()
    {
        float high = Mathf.Abs(_highPos.position.y - _bossControl.Player.position.y);
        float middle = Mathf.Abs(_middlePos.position.y - _bossControl.Player.position.y);
        float low = Mathf.Abs(_lowPos.position.y - _bossControl.Player.position.y);

        if (high < middle && high < low)
        {
            return BossAttackKind.High;
        }
        else if (middle <= high && middle <= low)
        {
            return BossAttackKind.Middle;
        }
        else
        {
            return BossAttackKind.Low;
        }
    }

}

public enum BossAttackKind
{
    High,
    Middle,
    Low,
    Front,
    Back,
}