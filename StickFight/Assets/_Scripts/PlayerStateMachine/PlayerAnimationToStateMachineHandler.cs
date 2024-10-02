using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationToStateMachineHandler : MonoBehaviour
{
    public Player Player;

    public void AnimationFinishedTrigger()
    {
        Player.AnimationFinishedTrigger();
    }

    public void AnimationTrigger()
    {
        Player.AnimationTrigger();
    }
}
