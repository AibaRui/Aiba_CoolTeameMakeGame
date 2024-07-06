using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttack
{
    [Header("高い位置")]
    [SerializeField] private Transform _highPos;

    [Header("中度の、位置")]
    [SerializeField] private Transform _middlePos;

    [Header("低い位置")]
    [SerializeField] private Transform _lowPos;

    [Header("攻撃するまでの感覚")]
    [SerializeField] private float _attackCoolTime = 10;

    [Header("左腕の当たり判定")]
    [SerializeField] private GameObject _leftArmAttackCollider;
    [Header("左手の当たり判定")]
    [SerializeField] private GameObject _leftHandAttackCollider;

    [Header("右腕の当たり判定")]
    [SerializeField] private GameObject _rightArmAttackCollider;
    [Header("右手の当たり判定")]
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
        // オブジェクトAから見たオブジェクトBの方向
        Vector3 toOther = _bossControl.Player.transform.position - _bossControl.transform.position;

        // 正規化して方向ベクトルにする
        Vector3 toOtherNormalized = toOther.normalized;

        // オブジェクトAの正面方向との角度を計算する
        float angle = Vector3.Angle(_bossControl.transform.forward, toOtherNormalized);

        // 正面から左右90度以内にいるかどうかを確認
        if (angle <= 90)
        {
            // オブジェクトBが右側にあるか左側にあるかを判断する
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
