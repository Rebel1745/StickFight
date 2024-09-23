using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.InputHandler.UseJumpInput();
        _player.JumpState.ResetAmountOfJumpsLeft();
        _core.Movement.SetVelocity(_playerData.WallJumpVelocity, _playerData.WallJumpAngle, _wallJumpDirection);
        _core.Movement.CheckIfShouldFlip(_wallJumpDirection);
        _player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= _startTime + _playerData.WallJumpTime)
        {
            _isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            _wallJumpDirection = -_core.Movement.FacingDirection;
        }
        else
        {
            _wallJumpDirection = _core.Movement.FacingDirection;
        }
    }
}
