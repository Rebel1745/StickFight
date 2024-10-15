using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LookForPlayerState : EnemyState
{
    protected D_LookForPlayer _stateData;

    protected bool _turnImmediately;
    protected bool _isPlayerInMinAgroRange;
    protected bool _isAllTurnsDone;
    protected bool _isAllTurnTimeDone;

    protected float _lastTurnTime;

    protected int _amountOfTurnsDone;

    public Enemy_LookForPlayerState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_LookForPlayer stateData) : base(enemy, stateMachine, animBoolName)
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

        _isAllTurnsDone = false;
        _isAllTurnTimeDone = false;

        _lastTurnTime = _startTime;
        _amountOfTurnsDone = 0;

        Movement?.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (_turnImmediately)
        {
            Movement?.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurnsDone++;
            _turnImmediately = false;
        }
        else if (Time.time >= _lastTurnTime + _stateData.TimeBetweenTurns && !_isAllTurnsDone)
        {
            Movement?.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurnsDone++;
        }

        if (_amountOfTurnsDone >= _stateData.AmountOfTurns)
        {
            _isAllTurnsDone = true;
        }

        if (Time.time >= _lastTurnTime + _stateData.TimeBetweenTurns && _isAllTurnsDone)
        {
            _isAllTurnTimeDone = true;
        }
    }

    public void SetTurnImmediately(bool turn)
    {
        _turnImmediately = turn;
    }
}
