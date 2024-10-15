using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected CC_Movement Movement { get => _movement ??= _core.GetCoreComponent<CC_Movement>(); }
    private CC_Movement _movement;
    protected CC_CollisionSenses CollisionSenses { get => _collisionSenses ??= _core.GetCoreComponent<CC_CollisionSenses>(); }
    private CC_CollisionSenses _collisionSenses;

    protected EnemyStateMachine _stateMachine;
    protected Enemy _enemy;
    protected Core _core;

    protected float _startTime;

    protected string _animBoolName;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
        _animBoolName = animBoolName;
        _core = _enemy.Core;
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
