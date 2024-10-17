using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirKickState : PlayerAbilityState
{
    public PlayerAirKickState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UseKickInput();
    }

    public override void Exit()
    {
        base.Exit();
        Movement?.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= _startTime + _playerData.AirKickDuration)
            _isAbilityDone = true;

        if (_canDamage)
            CheckForHits();
    }

    private void CheckForHits()
    {
        _hits = GetHits(_player.HitCheckOriginAirPunch.position, _player.HitBoxSizeAirPunch, _player.WhatIsEnemy);

        if (_hits.Length == 0) return;

        ApplyKnockbackToHits(_playerData.AirPunchKnockbackAngle, _playerData.AirPunchKnockbackForce, Movement.FacingDirection, 0f, false);
        ApplyDamageToHits(_playerData.AirPunchDamage);
        _canDamage = false;

        // if we hit something, suspend gravity so we can keep hitting
        Movement?.SetVelocityZero();
        Movement?.SetGravityScaleZero();
    }
}
