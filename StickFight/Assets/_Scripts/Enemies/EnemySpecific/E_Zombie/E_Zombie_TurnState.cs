using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Zombie_TurnState : Enemy_TurnState
{
    private E_Zombie _e_Zombie;

    public E_Zombie_TurnState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_TurnState stateData, E_Zombie e_Zombie) : base(enemy, stateMachine, animBoolName, stateData)
    {
        _e_Zombie = e_Zombie;
    }
}
