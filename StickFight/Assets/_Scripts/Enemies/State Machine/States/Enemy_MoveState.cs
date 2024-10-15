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
        if (CollisionSenses)
        {
            _isDetectingGround = CollisionSenses.LedgeVertical;
            _isDetectingWall = CollisionSenses.WallFront;
        }
        _isPlayerInMinAgroRange = _enemy.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(_stateData.MovementSpeed * Movement.FacingDirection);
    }

    public override void LogicUpate()
    {
        base.LogicUpate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
