using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundPunchState : PlayerAbilityState
{
    public PlayerGroundPunchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
        _isMultiAbility = playerData.IsPunchMultiAbility;
        _maxAbilityCount = playerData.MaxPunchCount;
        _abilityCountResetTime = playerData.MultiPunchResetTime;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        Movement.CanSetVelocity = false;
        _player.InputHandler.UsePunchInput();
        _player.Anim.SetInteger("Punch_Count", _currentAbilityCount);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        _hits = GetHits(_player.HitCheckOriginGroundPunch.position, _player.HitBoxSizeGroundPunch, _player.WhatIsEnemy);

        if (_hits.Length == 0) return;

        ApplyKnockbackToHits(_playerData.GroundPunchKnockbackAngle, _playerData.GroundPunchKnockbackForce, Movement.FacingDirection, 0f, false);
        ApplyDamageToHits(_playerData.GroundPunchDamage);
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        _isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();
        Movement.CanSetVelocity = true;
    }
}
