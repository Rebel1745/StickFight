using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    protected Transform _attackPosition;

    protected bool _isAnimationFinished;
    protected bool _isPlayerInMinAgroRange;

    public Enemy_AttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition) : base(enemy, stateMachine, animBoolName)
    {
        _attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.AnimHandler.AttackState = this;
        _isAnimationFinished = false;
        _core.Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpate()
    {
        base.LogicUpate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack() { }

    public virtual void FinishAttack()
    {
        _isAnimationFinished = true;
    }
}
