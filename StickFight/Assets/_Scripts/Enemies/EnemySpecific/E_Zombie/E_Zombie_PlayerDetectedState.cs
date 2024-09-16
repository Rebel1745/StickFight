using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Zombie_PlayerDetectedState : Enemy_PlayerDetectedState
{
    private E_Zombie _e_Zombie;
    public E_Zombie_PlayerDetectedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, E_Zombie e_Zombie) : base(enemy, stateMachine, animBoolName, stateData)
    {
        _e_Zombie = e_Zombie;
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

        if (_performCloseRangeAction)
        {
            _stateMachine.ChangeState(_e_Zombie.MeleeAttackState);
        }
        else if (_performLongRangeAction)
        {
            _stateMachine.ChangeState(_e_Zombie.ChargeState);
        }
        else if (!_isPlayerInMaxAgroRange)
        {
            _stateMachine.ChangeState(_e_Zombie.LookForPlayerState);
        }
    }
}
