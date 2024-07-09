using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashPunchState : PlayerAbilityState
{
    private float _remainingDashTime;
    private int _dashDirection;

    public PlayerDashPunchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UsePunchInput();
        _remainingDashTime = _player.DashStandardState.RemainingDashTime;
        _dashDirection = _player.InputHandler.DashDirection;
    }

    public override void Exit()
    {
        base.Exit();
        _player.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckForHit();

        if (Time.time <= _startTime + _remainingDashTime)
        {
            _player.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
        }
        else
        {
            _isAbilityDone = true;
        }
    }

    private void CheckForHit()
    {
        Collider2D[] hits;
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginDashPunch.position, _player.HitBoxSizeDashPunch, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            // if we hit something, suspend gravity so we can keep hitting
            _player.SetVelocityZero();
            _player.SetGravityScaleZero();

            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
            }
            _isAbilityDone = true;
        }
    }
}
