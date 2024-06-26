using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
        _amountOfJumpsLeft = _playerData.AmountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UseJumpInput();
        _player.SetVelocityY(_playerData.JumpVelocity);
        _player.SetGravityScale(_playerData.UpwardMovementGravityScale);
        _isAbilityDone = true;
        // as we are jumping straight away, decrease the number of jumps left
        DecreaseAmountOfJumpsLeft();
        _player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {
        if (_amountOfJumpsLeft > 0)
            return true;
        else return false;
    }

    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = _playerData.AmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
}
