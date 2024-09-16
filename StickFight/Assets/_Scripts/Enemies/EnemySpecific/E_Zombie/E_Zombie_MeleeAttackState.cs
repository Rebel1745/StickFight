using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Zombie_MeleeAttackState : Enemy_MeleeAttackState
{
    private E_Zombie _e_Zombie;

    public E_Zombie_MeleeAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, E_Zombie e_Zombie) : base(enemy, stateMachine, animBoolName, attackPosition, stateData)
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

        if (_isAnimationFinished)
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
