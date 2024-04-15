using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IDamageble>(out IDamageble d);
        d.Damage(DamageType.BossBigDamage);
    }
}
