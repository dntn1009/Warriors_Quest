using DefineHelper;
using System.Collections;
using UnityEngine;

public abstract class MonsterController : MonsterStat
{
    protected BehaviourState _state; // 현재 상태
    protected int _monNum;
    public int _monNumber { get { return _monNum; } set { _monNum = value; } } // 현재 본인의  Monster Number;
    public bool _isDeath
    {
        get
        {
            if (_state == BehaviourState.DEATH)
                return true;
            return false;
        }
    } // Die 애니메이션

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

    public abstract void SetDemage(AttackType attackType, float damage);
    public abstract void InitMonster(SpawnPos _genTransform);
}
