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
        _player.InputHandler.UseKickInput();
        CheckForHit();
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
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginGroundKick.position, _player.HitBoxSizeGroundKick, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
            }
        }
    }
}
