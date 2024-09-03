using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PlayerDetectedState : EnemyState
{
    protected D_PlayerDetectedStateData _stateData;

    protected bool _isPlayerInMinAgroRange;
    protected bool _isPlayerInMaxAgroRange;
    protected bool _performLongRangeAction;

    public Enemy_PlayerDetectedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetectedStateData stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
        _isPlayerInMaxAgroRange = _enemy.CheckPlayerInMaxAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.SetVelocityX(0);
        _performLongRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (Time.time >= _startTime + _stateData.LongRangeActionTime)
        {
            _performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
