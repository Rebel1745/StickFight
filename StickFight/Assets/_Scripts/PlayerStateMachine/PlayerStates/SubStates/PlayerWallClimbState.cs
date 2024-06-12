using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isExitingState)
        {
            _player.SetVelocityY(_playerData.WallClimbVelocity);

            if (_yInput != 1)
            {
                _stateMachine.ChangeState(_player.WallGrabState);
            }
        }
    }
}
