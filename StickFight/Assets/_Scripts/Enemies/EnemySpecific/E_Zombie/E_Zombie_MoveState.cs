using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class E_Zombie_MoveState : Enemy_MoveState
{
    private E_Zombie _e_Zombie;

    public E_Zombie_MoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_MoveState stateData, E_Zombie e_Zombie) : base(enemy, stateMachine, animBoolName, stateData)
    {
        _e_Zombie = e_Zombie;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (_isPlayerInMinAgroRange)
        {
            _stateMachine.ChangeState(_e_Zombie.PlayerDetectedState);
        }
        else if (_isDetectingWall || !_isDetectingGround)
        {
            _e_Zombie.IdleState.SetFlipAfterIdle(true);
            _stateMachine.ChangeState(_e_Zombie.IdleState);
        }
    }
}
