using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TurnState : EnemyState
{
    protected D_TurnState _stateData;

    protected bool _isAnimationFinished;

    public Enemy_TurnState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_TurnState stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.AnimHandler.TurnState = this;
        _isAnimationFinished = false;
        _core.Movement.SetVelocityX(0f);
    }

    public virtual void AnimationFinished()
    {
        _isAnimationFinished = true;
    }
}
