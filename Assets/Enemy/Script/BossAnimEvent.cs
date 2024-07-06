using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvent : MonoBehaviour
{
    [SerializeField] private BossControl _bossControl;

    public void DamageEnd()
    {
        _bossControl.BossHp.DamageEnd();
    }

    public void AttackEnd()
    {
        _bossControl.BossAttack.AttackEnd();
    }
}
