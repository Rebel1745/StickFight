using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _xInput;

    private bool _jumpInput;
    private bool _grabInput;
    private bool _isGrounded;
    private bool _isTouchingWall;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _player.CheckIfGrounded();
        _isTouchingWall = _player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();

        // we have touched the ground, we can now jump again
        _player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = _player.InputHandler.NormInputX;
        _jumpInput = _player.InputHandler.JumpInput;
        _grabInput = _player.InputHandler.GrabInput;

        if (_jumpInput && _player.JumpState.CanJump())
        {
            _player.InputHandler.UseJumpInput();
            _stateMachine.ChangeState(_player.JumpState);
        }
        // if we walk of a platform and are no longer grounded
        // transition to the InAir state
        else if (!_isGrounded)
        {
            // unless we have multiple jumps, we should not be able to jump in this state
            // however we have coyote time so start it here then when we transition to the In Air state
            // we can jump if we press the button within the coyote time window
            _player.InAirState.StartCoyoteTime();
            _stateMachine.ChangeState(_player.InAirState);
        }
        else if (_isTouchingWall && _grabInput)
        {
            _stateMachine.ChangeState(_player.WallGrabState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
