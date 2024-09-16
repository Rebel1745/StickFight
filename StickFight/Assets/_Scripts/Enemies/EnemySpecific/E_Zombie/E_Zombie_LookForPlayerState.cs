using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Zombie_LookForPlayerState : Enemy_LookForPlayerState
{
    private E_Zombie _e_Zombie;

    public E_Zombie_LookForPlayerState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_LookForPlayer stateData, E_Zombie e_Zombie) : base(enemy, stateMachine, animBoolName, stateData)
    {
        _e_Zombie = e_Zombie;
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (_isPlayerInMinAgroRange)
        {
            _stateMachine.ChangeState(_e_Zombie.PlayerDetectedState);
        }
        else if (_isAllTurnTimeDone)
        {
            _stateMachine.ChangeState(_e_Zombie.MoveState);
        }
    }
}
