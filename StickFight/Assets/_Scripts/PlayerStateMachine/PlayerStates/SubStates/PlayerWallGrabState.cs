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

        HoldPosition();

        if (_yInput > 0)
        {
            _stateMachine.ChangeState(_player.WallClimbState);
        }
        else if (_yInput < 0 || !_grabInput)
        {
            _stateMachine.ChangeState(_player.WallSlideState);
        }
    }

    // TODO: maybe, intstead of this, create a function on the Player to nullify gravity
    // as done in the previous version of the character controller
    private void HoldPosition()
    {
        _player.transform.position = _holdPosition;
        _player.SetVelocityX(0);
        _player.SetVelocityY(0);
    }
}
