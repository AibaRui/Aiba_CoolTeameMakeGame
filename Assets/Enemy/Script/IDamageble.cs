using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void Damage(DamageType type);

}

public enum DamageType
{
    /// <summary>�v���C���[���傫��������ԃ��x���̍U�� </summary>
    BossBigDamage,
    /// <summary>�v���C���[���y������郌�x���̍U�� </summary>
    BossReleaseDamage,
    Player
}
