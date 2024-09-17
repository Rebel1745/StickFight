using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Zombie_TurnState : Enemy_TurnState
{
    private E_Zombie _e_Zombie;

    private Quaternion _initialModelRotation;

    public E_Zombie_TurnState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_TurnState stateData, E_Zombie e_Zombie) : base(enemy, stateMachine, animBoolName, stateData)
    {
        _e_Zombie = e_Zombie;
    }

    public override void Enter()
    {
        base.Enter();
        _initialModelRotation = _enemy.EnemyModelBones.transform.rotation;
    }

    public override void LogicUpate()
    {
        base.LogicUpate();

        if (_isAnimationFinished)
        {
            _e_Zombie.ChangeRotationOfModelBones(_initialModelRotation);
            _e_Zombie.IdleState.SetFlipBeforeIdle(true);
            _stateMachine.ChangeState(_e_Zombie.IdleState);
        }
    }
}
