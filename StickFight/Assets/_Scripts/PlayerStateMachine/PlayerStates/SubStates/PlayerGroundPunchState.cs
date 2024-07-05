using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundPunchState : PlayerAbilityState
{
    public PlayerGroundPunchState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UsePunchInput();
        CheckForHit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= _startTime + _playerData.PunchDuration)
            _isAbilityDone = true;
    }

    private void CheckForHit()
    {
        Collider2D[] hits;
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginGroundPunch.position, _player.HitBoxSizeGroundPunch, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
            }
        }
    }
}
