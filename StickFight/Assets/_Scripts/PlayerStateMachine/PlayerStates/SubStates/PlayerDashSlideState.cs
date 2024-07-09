using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSlideState : PlayerAbilityState
{
    private float _remainingDashTime;
    private int _dashDirection;

    public PlayerDashSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UseKickInput();
        _remainingDashTime = _player.DashStandardState.RemainingDashTime;
        _dashDirection = _player.InputHandler.DashDirection;
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
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginDashKick.position, _player.HitBoxSizeDashKick, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
                c.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 10f);
            }
            _isAbilityDone = true;
        }
    }
}
