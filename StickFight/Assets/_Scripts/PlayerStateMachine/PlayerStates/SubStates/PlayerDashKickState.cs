using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashKickState : PlayerAbilityState
{
    private float _remainingDashTime;
    private int _dashDirection;

    public PlayerDashKickState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
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
        _core.Movement.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _hits = GetHits(_player.HitCheckOriginDashKick.position, _player.HitBoxSizeDashKick, _player.WhatIsEnemy);

        if (_hits.Length > 0)
        {
            _core.Movement.SetVelocityZero();
            ApplyKnockbackToHits(_playerData.DashKickKnockbackAngle, _playerData.DashKickKnockbackForce, _core.Movement.FacingDirection, _playerData.DashKickDuration, true);
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

    private void CheckForHit()
    {
        Collider2D[] hits;
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginDashKick.position, _player.HitBoxSizeDashKick, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            // if we hit something, suspend gravity so we can keep hitting
            _core.Movement.SetVelocityZero();
            _core.Movement.SetGravityScaleZero();

            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
            }
            _isAbilityDone = true;
        }
    }
}
