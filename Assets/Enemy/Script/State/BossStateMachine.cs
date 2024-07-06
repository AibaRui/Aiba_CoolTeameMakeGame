using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossStateMachine : StateMachine
{
    [SerializeField] private BossAttackState _attackState;
    [SerializeField] private BossDamageState _damageState;
    [SerializeField] private BossIdleState _idleState;
    [SerializeField] private BossWaitState _eventState;
    public BossWaitState EventState => _eventState;
    public BossAttackState AttackState => _attackState;
    public BossDamageState DamageState => _damageState;
    public BossIdleState IdleState => _idleState;

    private BossControl _bossControl;

    public BossControl BossControl => _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        Initialize(_idleState);
    }

    protected override void StateInit()
    {
        _idleState.Init(this);
        _damageState.Init(this);
        _attackState.Init(this);
        _eventState.Init(this);
    }
}
