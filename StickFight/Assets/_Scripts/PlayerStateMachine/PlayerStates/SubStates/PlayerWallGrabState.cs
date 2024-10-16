using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 _holdPosition;

    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _holdPosition = _player.transform.position;

        HoldPosition();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isExitingState)
        {
            HoldPosition();

            if (_playerData.CanWallClimb && _yInput > 0)
            {
                // if we are moving up, transition to climbing the wall
                _stateMachine.ChangeState(_player.WallClimbState);
            }
            else if (_playerData.CanWallSlide && _yInput < 0 || !_grabInput)
            {
                // if we are moving down, transition to sliding down the wall
                _stateMachine.ChangeState(_player.WallSlideState);
            }
        }
    }

    // TODO: maybe, intstead of this, create a function on the Player to nullify gravity
    // as done in the previous version of the character controller
    private void HoldPosition()
    {
        _player.transform.position = _holdPosition;
        Movement?.SetVelocityX(0);
        Movement?.SetVelocityY(0);
    }
}
