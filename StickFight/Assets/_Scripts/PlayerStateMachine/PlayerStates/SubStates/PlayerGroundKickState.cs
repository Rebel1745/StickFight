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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        CheckForHit();
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

    // TODO: use the below code on all the different attacks.  Only use knockback for dash slide and air punch/kick
    private void CheckForHit()
    {
        Collider2D[] hits;
        hits = Physics2D.OverlapBoxAll(_player.HitCheckOriginGroundKick.position, _player.HitBoxSizeGroundKick, 0f, _player.WhatIsEnemy);

        if (hits.Length <= 0) return;

        foreach (Collider2D c in hits)
        {
            Debug.Log("Hit " + c.gameObject.name);

            IDamageable damageable = c.gameObject.GetComponentInChildren<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(10f);
            }
            else
            {
                Debug.Log("Damageable is null?!");
            }

            IKnockbackable knockbackable = c.gameObject.GetComponentInChildren<IKnockbackable>();

            if (knockbackable != null)
            {
                knockbackable.Knockback(new Vector2(1f, 1f), 10, _core.Movement.FacingDirection);
            }
            else
            {
                Debug.Log("Knockbackable is null?!");
            }
        }
    }
}
