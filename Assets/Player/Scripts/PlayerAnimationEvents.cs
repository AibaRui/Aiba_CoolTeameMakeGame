using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerBossHit _hit;

    public void ExitHitState()
    {
        _hit.BossHitEnd();
    }

    public void AddFront()
    {
        _hit.FrontAddSpeed();
    }

    public void AddBack()
    {
        _hit.BackAddSpeed();
    }

}
