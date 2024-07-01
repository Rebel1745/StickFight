using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirPunchState : PlayerAbilityState
{
    public PlayerAirPunchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UsePunchInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= _startTime + _playerData.PunchDuration)
            _isAbilityDone = true;
    }
}
