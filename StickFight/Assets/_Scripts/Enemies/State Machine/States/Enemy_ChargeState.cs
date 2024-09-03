using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChargeState : EnemyState
{
    protected D_ChargeStateData _stateData;

    protected bool _isPlayerInMinAgroRange;
    protected bool _isDetectingWall;
    protected bool _isDetectingGround;
    protected bool _isChargeTimeOver;

    public Enemy_ChargeState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_ChargeStateData stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
        _isDetectingGround = _enemy.CheckGround();
        _isDetectingWall = _enemy.CheckWall();
    }

    public override void Enter()
    {
        base.Enter();

        _isChargeTimeOver = false;
        _enemy.SetVelocityX(_stateData.ChargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (Time.time >= _startTime + _stateData.ChargeTime)
        {
            _isChargeTimeOver = true;
        }
    }
}
