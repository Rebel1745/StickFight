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
        CheckForHit();
    }

    public override void Exit()
    {
        base.Exit();
        _player.ResetGravityScale();
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
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginAirPunch.position, _player.HitBoxSizeAirPunch, 0f, _player.WhatIsEnemy);

        if (hits.Length > 0)
        {
            // if we hit something, suspend gravity so we can keep hitting
            _player.SetVelocityZero();
            _player.SetGravityScaleZero();

            foreach (Collider2D c in hits)
            {
                Debug.Log("Collided with " + c.name);
                c.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, _playerData.PostPunchKnockupPower);
                _player.SetVelocityY(_playerData.PostPunchKnockupPower);
            }
        }
    }
}
