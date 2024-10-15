using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCeilingClingState : PlayerState
{
    private bool _isTouchingWall;
    private int _xInput;
    private int _yInput;
    private bool _grabInput;

    public PlayerCeilingClingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
            _isTouchingWall = CollisionSenses.WallFront;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetGravityScaleZero();
    }

    public override void Exit()
    {
        base.Exit();
        Movement?.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = _player.InputHandler.NormInputX;
        _yInput = _player.InputHandler.NormInputY;
        _grabInput = _player.InputHandler.GrabInput;

        // if we let go of grab, we are in the air
        if (!_grabInput)
        {
            _stateMachine.ChangeState(_player.InAirState);
        }
        // if we are touching a wall and holding down, start sliding down the wall
        else if (_playerData.CanWallSlide && _isTouchingWall && _yInput > 0.1f)
        {
            _stateMachine.ChangeState(_player.WallSlideState);
        }
        else if (_playerData.CanCeilingMove && _xInput != 0)
        {
            _stateMachine.ChangeState(_player.CeilingMoveState);
        }
    }
}
