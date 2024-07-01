using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashKickState : PlayerAbilityState
{
    public PlayerDashKickState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
}
