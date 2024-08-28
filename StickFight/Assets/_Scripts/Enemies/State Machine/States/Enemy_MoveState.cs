using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    protected D_MoveStateData _stateData;

    protected bool _isDetectingGround;
    protected bool _isDetectingWall;

    public Enemy_MoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_MoveStateData stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isDetectingGround = _enemy.CheckGround();
        _isDetectingWall = _enemy.CheckWall();
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
