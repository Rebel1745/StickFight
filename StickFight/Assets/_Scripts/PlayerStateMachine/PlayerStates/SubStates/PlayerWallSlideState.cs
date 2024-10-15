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

        // TODO: change all if(!_isExitingState) checks to if(_isExitingState) return;
        // this removes unnecessessary(sp!?) indentation
        if (!_isExitingState)
        {
            Movement?.SetVelocityY(-_playerData.WallSlideVelocity);
            if (_playerData.CanWallCling && _grabInput && _yInput == 0)
            {
                _stateMachine.ChangeState(_player.WallGrabState);
            }
        }
    }
}
