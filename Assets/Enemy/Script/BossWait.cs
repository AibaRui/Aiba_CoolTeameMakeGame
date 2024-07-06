using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class BossWait 
{
    [SerializeField] private Rig _rig;
    private BossControl _bossControl;
    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void WeightStop()
    {
        _rig.weight = 0f;
    }

    public void StartGame()
    {
        _rig.weight= 1.0f;
    }
}
