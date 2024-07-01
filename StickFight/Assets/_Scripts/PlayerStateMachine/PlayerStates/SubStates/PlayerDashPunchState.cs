using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashPunchState : PlayerAbilityState
{
    public PlayerDashPunchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
}
