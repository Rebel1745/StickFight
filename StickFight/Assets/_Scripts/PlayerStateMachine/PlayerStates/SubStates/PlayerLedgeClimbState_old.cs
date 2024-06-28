using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState_old : PlayerState
{
    // TODO: I don't like anything here apart from the detect corner position
    // Change it to match the old version of the code by first moving to the
    // corner pos.x + offset.x then to the corner pos.y + y offset
    private Vector2 _detectedPosition;
    private Vector2 _cornerPosition;
    private Vector2 _startPosition;
    private Vector2 _stopPosition;
    // _isHanging should default to false if we had an animation for hanging
    // we don't so for now default it to true
    private bool _isHanging = true;
    private bool _isClimbing;

    private int _xInput;
    private int _yInput;
    private bool _jumpInput;

    public PlayerLedgeClimbState_old(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.SetVelocityZero();
        _player.transform.position = _detectedPosition;
        _cornerPosition = _player.DetermineCornerPosition();

        _startPosition.Set(_cornerPosition.x - (_player.FacingDirection * _playerData.StartOffset.x), _cornerPosition.y - _playerData.StartOffset.y);
        _stopPosition.Set(_cornerPosition.x + (_player.FacingDirection * _playerData.StopOffset.x), _cornerPosition.y + _playerData.StopOffset.y);

        _player.transform.position = _startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;
        if (_isClimbing)
        {
            _player.transform.position = _stopPosition;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // below should be if(isAnimationFinished) if we have a climb animation
        // we don't atm so if we are climbing, move to the stop position
        if (_isClimbing)
        {
            // TODO: instead of snapping to the position, write function to move smoothly
            _stateMachine.ChangeState(_player.IdleState);
        }
        else
        {
            _xInput = _player.InputHandler.NormInputX;
            _yInput = _player.InputHandler.NormInputY;
            _jumpInput = _player.InputHandler.JumpInput;

            _player.SetVelocityZero();
            _player.transform.position = _startPosition;

            if (_xInput == _player.FacingDirection && _isHanging && !_isClimbing)
            {
                _isClimbing = true;
                // start playing the climbing animation if we had one
            }
            else if (_yInput == -1 && _isHanging && !_isClimbing)
            {
                _stateMachine.ChangeState(_player.InAirState);
            }
            else if (_jumpInput && !_isClimbing)
            {
                _player.WallJumpState.DetermineWallJumpDirection(true);
                _stateMachine.ChangeState(_player.WallJumpState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        // if we had an animation for grabbing the ledge, call this function when the anim completes
        // in our current case we don't so it will just be hard-coded from the start
        _isHanging = true;
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
    }

    public void SetDetectedPosition(Vector2 pos)
    {
        _detectedPosition = pos;
    }
}
