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
        CheckForHit();
    }

    public override void Exit()
    {
        base.Exit();
        _core.Movement.ResetGravityScale();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= _startTime + _playerData.KickDuration)
            _isAbilityDone = true;
    }

    private void CheckForHit()
    {
        Collider2D[] hits;
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginAirKick.position, _player.HitBoxSizeAirKick, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            // if we hit something, suspend gravity so we can keep hitting
            _core.Movement.SetVelocityZero();
            _core.Movement.SetGravityScaleZero();

            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
                //_player.ResetGravityScale();
            }
        }
    }
}
