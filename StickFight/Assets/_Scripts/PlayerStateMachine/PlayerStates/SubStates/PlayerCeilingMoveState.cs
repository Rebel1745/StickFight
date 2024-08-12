using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCeilingMoveState : PlayerState
{
    private bool _isTouchingCeiling;
    private bool _isTouchingWall;
    private int _xInput;
    private int _yInput;
    private bool _grabInput;

    public PlayerCeilingMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isTouchingCeiling = _player.CheckIfTouchingCeiling();
        _isTouchingWall = _player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetGravityScaleZero();
    }

    public override void Exit()
    {
        base.Exit();
        _player.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = _player.InputHandler.NormInputX;
        _yInput = _player.InputHandler.NormInputY;
        _grabInput = _player.InputHandler.GrabInput;

        _player.CheckIfShouldFlip(_xInput);

        _player.SetVelocityX(_xInput * _playerData.WallClimbVelocity);

        // if we let go of grab, we are in the air
        if (!_grabInput || !_isTouchingCeiling)
        {
            _stateMachine.ChangeState(_player.InAirState);
        }
        // if we are touching a wall and holding down, start sliding down the wall
        else if (_isTouchingWall && _yInput < -0.1f)
        {
            _stateMachine.ChangeState(_player.WallSlideState);
        }
        // if we stop moving, cling to the ceiling
        else if (_xInput == 0)
        {
            _stateMachine.ChangeState(_player.CeilingClingState);
        }
    }
}
