using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSlideState : PlayerAbilityState
{
    public PlayerDashSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
}
