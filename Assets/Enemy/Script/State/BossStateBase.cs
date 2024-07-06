using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BossStateBase :IState
{
    [NonSerialized]
    protected BossStateMachine _stateMachine = null;

    /// <summary>StateMacine‚ğƒZƒbƒg‚·‚éŠÖ”</summary>
    /// <param name="stateMachine"></param>
    public void Init(BossStateMachine stateMachine)
    {
        _stateMachine = stateMachine;

    }



    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedUpdate();
    public abstract void Update();

    public abstract void LateUpdate();
}
