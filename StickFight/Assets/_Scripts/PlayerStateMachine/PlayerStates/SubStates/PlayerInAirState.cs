using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInAirState : PlayerState
{
    #region Checks
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    private bool _isTouchingCeiling;
    private bool _isTouchingLedge;
    #endregion

    #region Inputs
    private int _xInput;
    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _grabInput;
    private bool _dashInput;
    private bool _punchInput;
    private bool _kickInput;
    #endregion

    private bool _coyoteTime;
    private bool _wallJumpCoyoteTime;
    private bool _isJumping;

    private float _startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        if (CollisionSenses)
        {
            _isGrounded = CollisionSenses.Ground;
            _isTouchingWall = CollisionSenses.WallFront;
            _isTouchingWallBack = CollisionSenses.WallBack;
            _isTouchingCeiling = CollisionSenses.Ceiling;
            _isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        /*if (_isTouchingWall && !_isTouchingLedge)
        {
            _player.LedgeClimbState.SetDetectedPosition(_player.transform.position);
        }*/

        if (!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _isTouchingWall = false;
        _isTouchingWallBack = false;
        _oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
        Movement?.SetBoxCollider(_playerData.DefaultHitboxOffset, _playerData.DefaultHitboxSize);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        _xInput = _player.InputHandler.NormInputX;
        _jumpInput = _player.InputHandler.JumpInput;
        _jumpInputStop = _player.InputHandler.JumpInputStop;
        _grabInput = _player.InputHandler.GrabInput;
        _dashInput = _player.InputHandler.DashInput;
        _punchInput = _player.InputHandler.PunchInput;
        _kickInput = _player.InputHandler.KickInput;

        CheckJumpMultiplier();

        if (_isGrounded && Movement?.CurrentVelocity.y < 0.01f)
        {
            // Changed from landed state to grounded state to stop the (perhaps) unnecessary landing animation playing
            //_stateMachine.ChangeState(_player.LandState);
            _stateMachine.ChangeState(_player.IdleState);
        }
        // we no longer transition to climbing the ledge unless we are grabbing the wall
        /*else if (_isTouchingWall && !_isTouchingLedge && !_isGrounded)
        {
            _stateMachine.ChangeState(_player.LedgeClimbState);
        }*/
        else if (_playerData.CanWallJump && _jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            _isTouchingWall = CollisionSenses.WallFront;
            _player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            _stateMachine.ChangeState(_player.WallJumpState);
        }
        else if (_jumpInput && _player.JumpState.CanJump())
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
        else if (_playerData.CanCeilingCling && _isTouchingCeiling && _grabInput)
        {
            _stateMachine.ChangeState(_player.CeilingClingState);
        }
        else if (_playerData.CanWallCling && _isTouchingWall && _grabInput && _isTouchingLedge)
        {
            _stateMachine.ChangeState(_player.WallGrabState);
        }
        else if (_playerData.CanWallSlide && _isTouchingWall && (_xInput == Movement.FacingDirection || _xInput == 0) && Movement.CurrentVelocity.y <= 0)
        {
            _stateMachine.ChangeState(_player.WallSlideState);
        }
        else if (_playerData.CanDash && _dashInput && _player.DashStandardState.CheckIfCanDash())
        {
            _stateMachine.ChangeState(_player.DashStandardState);
        }
        else if (_playerData.CanAirPunch && _punchInput)
        {
            _stateMachine.ChangeState(_player.AirPunchState);
        }
        else if (_playerData.CanAirKick && _kickInput)
        {
            _stateMachine.ChangeState(_player.AirKickState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(_xInput);
            Movement?.SetVelocityX(_playerData.MovementVelocity * _xInput);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime()
    {
        if (_coyoteTime && Time.time > _startTime + _playerData.CoyoteTime)
        {
            _coyoteTime = false;
            _player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => _coyoteTime = true;

    private void CheckWallJumpCoyoteTime()
    {
        // TODO: change _playerData.CoyoteTime to a new variable of WallJumpCoyoteTime
        if (_wallJumpCoyoteTime && _startWallJumpCoyoteTime > _startTime + _playerData.CoyoteTime)
        {
            _wallJumpCoyoteTime = false;
            _player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartWallJumpCoyoteTime()
    {
        _startWallJumpCoyoteTime = Time.time;
        _wallJumpCoyoteTime = true;
    }
    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;

    private void CheckJumpMultiplier()
    {
        if (_isJumping)
        {
            if (_jumpInputStop)
            {
                Movement?.SetGravityScale(_playerData.DownwardMovementGravityScale);
                _isJumping = false;
            }
            else if (Movement?.CurrentVelocity.y <= 0f)
            {
                Movement?.SetGravityScale(_playerData.DownwardMovementGravityScale);
                _isJumping = false;
            }
        }
        else
        {
            Movement?.SetGravityScale(_playerData.DownwardMovementGravityScale);
        }
    }

    public void SetIsJumping() => _isJumping = true;
}
