using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void Damage(DamageType type);

}

public enum DamageType
{
    /// <summary>プレイヤーが大きく吹き飛ぶレベルの攻撃 </summary>
    BossBigDamage,
    /// <summary>プレイヤーが軽く離れるレベルの攻撃 </summary>
    BossReleaseDamage,
    Player
}
