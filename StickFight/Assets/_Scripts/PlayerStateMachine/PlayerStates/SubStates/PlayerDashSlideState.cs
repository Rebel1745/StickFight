using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashSlideState : PlayerAbilityState
{
    private float _remainingDashTime;
    private int _dashDirection;
    private bool _checkForHit;

    public PlayerDashSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.InputHandler.UseKickInput();
        _remainingDashTime = _player.DashStandardState.RemainingDashTime;
        _dashDirection = _player.InputHandler.DashDirection;
        _checkForHit = true;
    }

    public override void Exit()
    {
        base.Exit();
        _core.Movement.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_checkForHit)
            CheckForHit();

        if (Time.time <= _startTime + _remainingDashTime)
        {
            _core.Movement.SetVelocityX((Vector2.right * _playerData.DashVelocity).x * _dashDirection);
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
            _checkForHit = false;
            foreach (Collider2D c in hits)
            {
                c.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, _playerData.PostKickKnockupPower);
                Debug.Log("Dash Slide Collided with " + c.name);
                _isAbilityDone = true;
            }
        }
    }
}
