using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashStandardState : PlayerAbilityState
{
    public bool CanDash { get; private set; }
    public float RemainingDashTime { get; private set; }

    private float _lastDashTime;

    private int _dashDirection;

    private bool _punchInput;
    private bool _kickInput;

    public PlayerDashStandardState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
            _isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        _player.InputHandler.UseDashInput();
        _lastDashTime = Time.time;
        _dashDirection = _player.InputHandler.DashDirection;

        Movement?.SetGravityScaleZero();
        Movement?.SetVelocityZero();
        Movement?.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
        Movement?.SetBoxCollider(_playerData.DashStandardHitboxOffset, _playerData.DashStandardHitboxSize);
    }

    public override void Exit()
    {
        Movement?.ResetGravityScale();
        Movement?.SetBoxCollider(_playerData.DefaultHitboxOffset, _playerData.DefaultHitboxSize);
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if we have to move to a dash attack state, this will tell us how long to remain dashing
        RemainingDashTime = (_lastDashTime + _playerData.DashTime) - Time.time;

        _punchInput = _player.InputHandler.PunchInput;
        _kickInput = _player.InputHandler.KickInput;

        if (Time.time <= _lastDashTime + _playerData.DashTime)
        {
            Movement?.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
        }
        else
        {
            _isAbilityDone = true;
        }

        if (_playerData.CanDashPunch && _punchInput)
        {
            _stateMachine.ChangeState(_player.DashPunchState);
        }
        else if (_kickInput)
        {
            if (_playerData.CanDashSlide && _isGrounded)
                _stateMachine.ChangeState(_player.DashSlideState);
            else if (_playerData.CanDashKick)
                _stateMachine.ChangeState(_player.DashKickState);
        }
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= _lastDashTime + _playerData.DashCooldown;
    }

    public void ResetCanDash() => CanDash = true;
}
