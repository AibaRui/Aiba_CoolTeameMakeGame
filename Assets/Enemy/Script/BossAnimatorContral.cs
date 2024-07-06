using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAnimatorContral
{
    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void Cry()
    {
        _bossControl.Animator.Play("Movie_Roal");
    }
    public void Damage(BossDamagePoint bossDamagePoint)
    {
        _bossControl.Animator.SetTrigger("Damage");
        if (bossDamagePoint == BossDamagePoint.RightArm)
        {
            _bossControl.Animator.SetInteger("DamageType", 0);
        }
        else if (bossDamagePoint == BossDamagePoint.LeftArm)
        {
            _bossControl.Animator.SetInteger("DamageType", 1);
        }
        else
        {
            _bossControl.Animator.SetInteger("DamageType", 2);
        }
    }

    public void Attack(BossAttackKind kind, bool isRight)
    {
        _bossControl.Animator.SetTrigger("Attack");

        if (kind == BossAttackKind.Front)
        {
            _bossControl.Animator.SetInteger("AttackType", 0);
        }
        else if (kind == BossAttackKind.Back)
        {
            _bossControl.Animator.SetInteger("AttackType", 1);
        }
        else
        {
            if (kind == BossAttackKind.High)
            {
                _bossControl.Animator.SetInteger("AttackType", 2);
                if (isRight)
                {
                    _bossControl.Animator.SetBool("AttackIsRight", true);
                }
                else
                {
                    _bossControl.Animator.SetBool("AttackType", false);
                }
            }
            else if (kind == BossAttackKind.Middle)
            {
                _bossControl.Animator.SetInteger("AttackType", 3);
                if (isRight)
                {
                    _bossControl.Animator.SetBool("AttackIsRight", true);
                }
                else
                {
                    _bossControl.Animator.SetBool("AttackType", false);
                }
            }
            else if (kind == BossAttackKind.Low)
            {
                _bossControl.Animator.SetInteger("AttackType", 4);
                if (isRight)
                {
                    _bossControl.Animator.SetBool("AttackIsRight", true);
                }
                else
                {
                    _bossControl.Animator.SetBool("AttackType", false);
                }
            }
        }
    }

}
