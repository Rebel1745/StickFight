using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChargeState : EnemyState
{
    protected D_ChargeState _stateData;

    protected bool _isPlayerInMinAgroRange;
    protected bool _isDetectingWall;
    protected bool _isDetectingGround;
    protected bool _isChargeTimeOver;
    protected bool _performCloseRangeAction;

    public Enemy_ChargeState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
        if (CollisionSenses)
        {
            _isDetectingGround = CollisionSenses.LedgeVertical;
            _isDetectingWall = CollisionSenses.WallFront;
        }

        _performCloseRangeAction = _enemy.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        _isChargeTimeOver = false;
        Movement?.SetVelocityX(_stateData.ChargeSpeed * Movement.FacingDirection);
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
