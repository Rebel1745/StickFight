using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TurnState : EnemyState
{
    protected D_TurnState _stateData;

    public Enemy_TurnState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, D_TurnState stateData) : base(enemy, stateMachine, animBoolName)
    {
        _stateData = stateData;
    }
}
