using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    protected D_IdleState _stateData;

    protected bool _flipBeforeIdle;
    protected bool _flipAfterIdle;
    protected bool _isIdleTimeOver;
    protected bool _isPlayerInMinAgroRange;

    protected float _idleTime;

    public Enemy_IdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        if (_flipBeforeIdle)
        {
            Movement?.Flip();
        }

        Movement?.SetVelocityX(0);
        _isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (_flipAfterIdle)
        {
            Movement?.Flip();
        }
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (Time.time >= _startTime + _idleTime)
        {
            _isIdleTimeOver = true;
        }
    }

    public void SetFlipBeforeIdle(bool flipBeforeIdle)
    {
        _flipBeforeIdle = flipBeforeIdle;
    }

    public void SetFlipAfterIdle(bool flipAfterIdle)
    {
        _flipAfterIdle = flipAfterIdle;
    }

    private void SetRandomIdleTime()
    {
        _idleTime = Random.Range(_stateData.MinIdleTime, _stateData.MaxIdleTime);
    }
}
