using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachineHandler : MonoBehaviour
{
    public Enemy_AttackState AttackState;

    public void TriggerAttack()
    {
        AttackState.TriggerAttack();
    }

    public void FinishAttack()
    {
        AttackState.FinishAttack();
    }
}
