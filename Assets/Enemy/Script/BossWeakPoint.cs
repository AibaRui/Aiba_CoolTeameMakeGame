using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour, IDamageble
{
    [SerializeField] private BossHitDirection _hit;

    [SerializeField] private LayerMask _layer;

    public void Damage(DamageType type)
    {
        _hit.BossDamageStartDirection();
    }


}
