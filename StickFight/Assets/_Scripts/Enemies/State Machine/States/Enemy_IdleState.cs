using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    protected D_IdleStateData _stateData;

    protected bool _flipAfterIdle;
    protected bool _isIdleTimeOver;
    protected bool _isPlayerInMinAgroRange;

    protected float _idleTime;

    public Enemy_IdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_IdleStateData stateData) : base(enemy, stateMachine, animBoolName)
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

        _enemy.SetVelocityX(0);
        _isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (_flipAfterIdle)
        {
            _enemy.Flip();
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

    public void SetFlipAfterIdle(bool flipAfterIdle)
    {
        _flipAfterIdle = flipAfterIdle;
    }

    private void SetRandomIdleTime()
    {
        _idleTime = Random.Range(_stateData.MinIdleTime, _stateData.MaxIdleTime);
    }
}
