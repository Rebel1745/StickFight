using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSlideState : PlayerAbilityState
{
    private float _remainingDashTime;
    private int _dashDirection;

    public PlayerDashSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UseKickInput();
        _remainingDashTime = _player.DashStandardState.RemainingDashTime;
        _dashDirection = _player.InputHandler.DashDirection;
    }

    public override void Exit()
    {
        base.Exit();
        _core.Movement.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _hits = GetHits(_player.HitCheckOriginDashKick.position, _player.HitBoxSizeDashKick, _player.WhatIsEnemy);

        if (_hits.Length > 0)
        {
            _core.Movement.SetVelocityZero();
            ApplyKnockbackToHits(_playerData.DashSlideKnockbackAngle, _playerData.DashSlideKnockbackForce, _core.Movement.FacingDirection, 0f, false);
            ApplyDamageToHits(_playerData.DashSlideDamage);
            _isAbilityDone = true;
        }
        else
        {
            if (Time.time <= _startTime + _remainingDashTime)
            {
                _core.Movement.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
            }
            else
            {
                _isAbilityDone = true;
            }
        }
    }
}
