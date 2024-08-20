using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool _isGrounded;
    protected bool _isTouchingWall;
    protected bool _isTouchingCeiling;
    protected bool _isTouchingLedge;
    protected int _xInput;
    protected int _yInput;
    protected bool _jumpInput;
    protected bool _grabInput;

    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _player.CheckIfGrounded();
        _isTouchingWall = _player.CheckIfTouchingWall();
        _isTouchingCeiling = _player.CheckIfTouchingCeiling();
        _isTouchingLedge = _player.CheckIfTouchingLedge();

        /*if (_isTouchingWall && !_isTouchingLedge)
        {
            _player.LedgeClimbState.SetDetectedPosition(_player.transform.position);
        }*/
    }

    public override void Enter()
    {
        base.Enter();
        _player.ResetGravityScale();
        _player.SetBoxCollider(_playerData.WallHitboxOffset, _playerData.WallHitboxSize);
    }

    public override void Exit()
    {
        _player.SetBoxCollider(_playerData.DefaultHitboxOffset, _playerData.DefaultHitboxSize);
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = _player.InputHandler.NormInputX;
        _yInput = _player.InputHandler.NormInputY;
        _jumpInput = _player.InputHandler.JumpInput;
        _grabInput = _player.InputHandler.GrabInput;

        // if we are jumping, perform a wall jump
        if (_jumpInput)
        {
            _player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            _stateMachine.ChangeState(_player.WallJumpState);
        }
        // if we touch the ground and stop clinging, go idle
        else if (_isGrounded && !_grabInput)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
        // if we aren't touching a wall or if we are trying to move in a direction away from the wall, let go and be airborne
        else if (!_isTouchingWall || (_xInput != 0 && _xInput != _player.FacingDirection && !_grabInput))
        {
            _stateMachine.ChangeState(_player.InAirState);
        }
        else if (_isTouchingWall && !_isTouchingLedge && _grabInput && _yInput == 1)
        {
            _stateMachine.ChangeState(_player.LedgeClimbState);
        }
        // if we are touching the ceiling and moving away from the wall, move along the ceiling
        else if (_isTouchingCeiling && _xInput != 0 && _xInput != _player.FacingDirection)
        {
            _stateMachine.ChangeState(_player.CeilingMoveState);
        }
    }
}
