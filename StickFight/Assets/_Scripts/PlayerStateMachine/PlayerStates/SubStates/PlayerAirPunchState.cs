using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirPunchState : PlayerAbilityState
{
    public PlayerAirPunchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UsePunchInput();
    }

    public override void Exit()
    {
        base.Exit();
        _core.Movement.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= _startTime + _playerData.AirPunchDuration)
            _isAbilityDone = true;

        CheckForHits();
    }

    private void CheckForHits()
    {
        _hits = GetHits(_player.HitCheckOriginAirPunch.position, _player.HitBoxSizeAirPunch, _player.WhatIsEnemy);

        if (_hits.Length == 0) return;

        ApplyKnockbackToHits(_playerData.AirPunchKnockbackAngle, _playerData.AirPunchKnockbackForce, _core.Movement.FacingDirection, 0f, false);
        ApplyDamageToHits(_playerData.AirPunchDamage);

        Collider2D[] hits;
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginAirPunch.position, _player.HitBoxSizeAirPunch, 0f, _player.WhatIsEnemy);

        // if we hit something, suspend gravity so we can keep hitting
        _core.Movement.SetVelocityZero();
        _core.Movement.SetGravityScaleZero();
    }
}
