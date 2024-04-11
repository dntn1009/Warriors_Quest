using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public abstract class MonsterFSM : MonsterStat
{
    protected BehaviourState _state; // 현재 상태

    public virtual void Initialize()
    {
        ChangeState(_state);
    }

    protected virtual void Update()
    {
        StateUpdate(_state);
    }

    protected virtual void FixedUpdate()
    {
        StateFixedUpdate(_state);
    }

    public virtual void ChangeState(BehaviourState newState)
    {
        if (newState == _state)
            return;
        OnStateExit(_state);
        _state = newState;
        OnStateEnter(_state);
    }

    public abstract void OnStateEnter(BehaviourState state);

    public abstract void StateUpdate(BehaviourState state);

    public abstract void StateFixedUpdate(BehaviourState state);

    public abstract void OnStateExit(BehaviourState state);
}
