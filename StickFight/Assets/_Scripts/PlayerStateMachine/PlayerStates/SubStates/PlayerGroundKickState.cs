using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundKickState : PlayerAbilityState
{
    public PlayerGroundKickState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
        _isMultiAbility = playerData.IsKickMultiAbility;
        _maxAbilityCount = playerData.MaxKickCount;
        _abilityCountResetTime = playerData.MultiKickResetTime;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        Movement.CanSetVelocity = false;
        _player.InputHandler.UseKickInput();
        _player.Anim.SetInteger("Kick_Count", _currentAbilityCount);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        _hits = GetHits(_player.HitCheckOriginGroundKick.position, _player.HitBoxSizeGroundKick, _player.WhatIsEnemy);

        if (_hits.Length == 0) return;

        ApplyKnockbackToHits(_playerData.GroundKickKnockbackAngle, _playerData.GroundKickKnockbackForce, Movement.FacingDirection, 0f, false);
        ApplyDamageToHits(_playerData.GroundKickDamage);
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
