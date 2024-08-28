using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine _stateMachine;
    protected Enemy _enemy;

    protected float _startTime;

    protected string _animBoolName;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        _startTime = Time.time;
        _enemy.Anim.SetBool(_animBoolName, true);
    }

    public virtual void Exit()
    {
        _enemy.Anim.SetBool(_animBoolName, false);
    }

    public virtual void LogicUpate() { }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }
}
