using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _xInput;

    private bool _jumpInput;
    private bool _grabInput;
    private bool _dashInput;
    private bool _punchInput;
    private bool _kickInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingLedge;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _core.CollisionSenses.Ground;
        _isTouchingWall = _core.CollisionSenses.WallFront;
        _isTouchingLedge = _core.CollisionSenses.LedgeHorizontal;
    }

    public override void Enter()
    {
        base.Enter();

        // we have touched the ground, we can now jump again
        _player.JumpState.ResetAmountOfJumpsLeft();
        _player.DashStandardState.ResetCanDash();
        _core.Movement.ResetGravityScale();
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
        _dashInput = _player.InputHandler.DashInput;
        _punchInput = _player.InputHandler.PunchInput;
        _kickInput = _player.InputHandler.KickInput;

        if (_jumpInput && _player.JumpState.CanJump())
        {
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
        else if (_playerData.CanWallCling && _isTouchingWall && _grabInput && _isTouchingLedge)
        {
            _stateMachine.ChangeState(_player.WallGrabState);
        }
        // TODO: when other types of dash are available change this
        // This should transition to the dash slide state
        else if (_playerData.CanDash && _dashInput && _player.DashStandardState.CheckIfCanDash())
        {
            _stateMachine.ChangeState(_player.DashStandardState);
        }
        else if (_playerData.CanPunch && _punchInput)
        {
            _stateMachine.ChangeState(_player.GroundPunchState);
        }
        else if (_playerData.CanKick && _kickInput)
        {
            _stateMachine.ChangeState(_player.GroundKickState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
