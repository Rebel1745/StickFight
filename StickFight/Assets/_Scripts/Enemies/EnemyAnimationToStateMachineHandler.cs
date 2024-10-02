using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationToStateMachineHandler : MonoBehaviour
{
    public Enemy_AttackState AttackState;
    public Enemy_TurnState TurnState;

    public void TriggerAttack()
    {
        AttackState.TriggerAttack();
    }

    public void FinishAttack()
    {
        AttackState.FinishAttack();
    }

    public void TurnAnimationFinished()
    {
        TurnState.AnimationFinished();
    }
}
