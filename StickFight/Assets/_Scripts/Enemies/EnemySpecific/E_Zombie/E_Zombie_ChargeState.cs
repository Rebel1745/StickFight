using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Zombie_ChargeState : Enemy_ChargeState
{
    private E_Zombie _e_Zombie;

    public E_Zombie_ChargeState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_ChargeStateData stateData, E_Zombie eZombie) : base(enemy, stateMachine, animBoolName, stateData)
    {
        _e_Zombie = eZombie;
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (_performCloseRangeAction)
        {
            _stateMachine.ChangeState(_e_Zombie.MeleeAttackState);
        }
        else if (!_isDetectingGround || _isDetectingWall)
        {
            _stateMachine.ChangeState(_e_Zombie.LookForPlayerState);
        }
        else if (_isChargeTimeOver)
        {
            if (_isPlayerInMinAgroRange)
            {
                _stateMachine.ChangeState(_e_Zombie.PlayerDetectedState);
            }
            else
            {
                _stateMachine.ChangeState(_e_Zombie.LookForPlayerState);
            }
        }
    }
}
