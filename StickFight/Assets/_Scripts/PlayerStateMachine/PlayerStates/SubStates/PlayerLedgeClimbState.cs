using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    // TODO: I don't like anything here apart from the detect corner position
    // Change it to match the old version of the code by first moving to the
    // corner pos.x + offset.x then to the corner pos.y + y offset
    private Vector2 _detectedPosition;
    private Vector2 _cornerPosition;
    private Vector2 _startPosition;
    private Vector2 _stopPosition;

    private bool _isClimbing;
    private bool _isClimbingUp;
    private bool _isMovingAcross;

    private int _xInput;
    private int _yInput;
    private bool _jumpInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.SetVelocityZero();
        _cornerPosition = _player.DetermineCornerPosition();
        _stopPosition.Set(_cornerPosition.x + (_player.FacingDirection * _playerData.StopOffset.x), _cornerPosition.y + _playerData.StopOffset.y);

        // start the movement towards the stop position
        _isClimbing = true;
        // we start by moving up
        _isClimbingUp = true;
        // we then move across
        _isMovingAcross = false;
    }

    public override void Exit()
    {
        base.Exit();
        _isMovingAcross = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isClimbing)
        {
            // we have finished climbing, time to transition back to idle
            _stateMachine.ChangeState(_player.IdleState);
            return;
        }

        if (_isClimbingUp)
        {
            _player.SetVelocityY(_playerData.WallClimbVelocity);
            if (_player.transform.position.y >= _stopPosition.y)
            {
                _isClimbingUp = false;
                _isMovingAcross = true;
            }
        }

        if (_isMovingAcross)
        {
            Debug.Log("Moving across");
            _player.SetVelocityX(_playerData.MovementVelocity * _player.FacingDirection);
            if (Mathf.Abs(_player.transform.position.x - _stopPosition.x) < 0.01f)
            {
                // if we have reached our stop position, stop climbing
                _isClimbing = false;
            }
        }

        // if we haven't reached the stop position x value, move at our climb speed towards it
        /*if (_player.transform.position.x < _stopPosition.x)
        {
            _player.SetVelocityX(_playerData.WallClimbVelocity);
        }
        // if we haven't reached the stop position y value, move at our movement speed towards it
        else if (Vector2.Distance(_player.transform.position, _stopPosition) >= 0.01f)
        {
            _player.SetVelocityY(_playerData.MovementVelocity);
        }*/
    }
}
