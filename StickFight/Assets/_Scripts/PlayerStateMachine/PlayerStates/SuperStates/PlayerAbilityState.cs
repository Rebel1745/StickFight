using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool _isAbilityDone;
    protected bool _isGrounded;

    protected Collider2D[] _hits;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAbilityDone)
        {
            // for the first frame or so after jumping we are still 'technically' grounded, so only transition if we are not jumping (i.e. dont have a positive y velocity)
            if (_isGrounded && _core.Movement.CurrentVelocity.y < 0.01f)
                _stateMachine.ChangeState(_player.IdleState);
            else
                _stateMachine.ChangeState(_player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public Collider2D[] GetHits(Vector2 origin, Vector2 size, LayerMask mask)
    {
        return Physics2D.OverlapBoxAll(origin, size, 0f, mask);
    }

    public void ApplyDamageToHits(float damage)
    {
        foreach (Collider2D c in _hits)
        {
            Debug.Log("Hit " + c.gameObject.name);

            IDamageable damageable = c.gameObject.GetComponentInChildren<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(damage);
            }
            else
            {
                Debug.Log("Damageable is null?!");
            }
        }
    }

    public void ApplyKnockbackToHits(Vector2 angle, float force, int direction)
    {
        foreach (Collider2D c in _hits)
        {
            Debug.Log("Knockback " + c.gameObject.name);

            IKnockbackable knockbackable = c.gameObject.GetComponentInChildren<IKnockbackable>();

            if (knockbackable != null)
            {
                knockbackable.Knockback(angle, force, direction);
            }
            else
            {
                Debug.Log("Knockbackable is null?!");
            }
        }
    }
}
