using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _player.SetVelocityY(-_playerData.WallSlideVelocity);

        if (_grabInput && _yInput == 0)
        {
            _stateMachine.ChangeState(_player.WallGrabState);
        }
    }
}
