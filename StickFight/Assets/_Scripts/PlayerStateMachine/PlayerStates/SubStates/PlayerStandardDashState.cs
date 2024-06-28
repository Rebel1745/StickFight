using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandardDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private float _lastDashTime;

    private int _dashDirection;

    public PlayerStandardDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        _player.InputHandler.UseDashInput();
        _lastDashTime = Time.time;
        _dashDirection = _player.InputHandler.DashDirection;

        _player.SetGravityScaleZero();
        _player.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
    }

    public override void Exit()
    {
        _player.ResetGravityScale();
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time <= _lastDashTime + _playerData.DashTime)
        {
            _player.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
        }
        else
        {
            _isAbilityDone = true;
        }
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= _lastDashTime + _playerData.DashCooldown;
    }

    public void ResetCanDash() => CanDash = true;
}
