using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundKickState : PlayerAbilityState
{
    public PlayerGroundKickState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _core.Movement.SetVelocityZero();
        _core.Movement.CanSetVelocity = false;
        _player.InputHandler.UseKickInput();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        _hits = GetHits(_player.HitCheckOriginGroundKick.position, _player.HitBoxSizeGroundKick, _player.WhatIsEnemy);

        if (_hits.Length == 0) return;

        ApplyKnockbackToHits(_playerData.GroundKickKnockbackAngle, _playerData.GroundKickKnockbackForce, _core.Movement.FacingDirection);
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
        _core.Movement.CanSetVelocity = true;
    }
}
