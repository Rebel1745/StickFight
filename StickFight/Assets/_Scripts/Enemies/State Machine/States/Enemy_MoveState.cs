using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    protected D_MoveState _stateData;

    protected bool _isDetectingGround;
    protected bool _isDetectingWall;
    protected bool _isPlayerInMinAgroRange;

    public Enemy_MoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isDetectingGround = _enemy.CheckLedge();
        _isDetectingWall = _enemy.CheckWall();
        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.SetVelocityX(_stateData.MovementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
