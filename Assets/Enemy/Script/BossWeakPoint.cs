using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour, IDamageble
{
    [Header("Žã“_‚ÌˆÊ’u")]
    [SerializeField] private BossDamagePoint _damagePoint;

     private BossHp _hp;

    public void Init(BossHp bossHp)
    {
        _hp= bossHp;
    }

    public void Damage(DamageType type)
    {
        _hp.Damage(type,_damagePoint);
    }


}
