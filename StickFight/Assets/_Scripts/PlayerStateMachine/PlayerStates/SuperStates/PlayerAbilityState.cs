using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool _isAbilityDone;
    protected bool _isGrounded;

    protected bool _isMultiAbility = false;
    protected int _maxAbilityCount; // number of different animations to loop through for an ability
    protected int _currentAbilityCount;
    protected float _abilityCountResetTime;
    protected float _lastAbilityTime;

    protected Collider2D[] _hits;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
            _isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = false;

        // check to see if we have a multi-animation ability, if so, reset if the time has elapsed
        if (!_isMultiAbility) return;

        // only reset the count if we have actually done an ability
        if (_currentAbilityCount == 0) return;

        if (Time.time >= _lastAbilityTime + _abilityCountResetTime)
        {
            ResetAbilityCount();
        }
    }

    public override void Exit()
    {
        if (_isMultiAbility)
        {
            // we have done an ability, increment the count and set the last ability time as the current time
            _lastAbilityTime = Time.time;
            IncrementAbilityCount();
        }

        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAbilityDone)
        {
            // for the first frame or so after jumping we are still 'technically' grounded, so only transition if we are not jumping (i.e. dont have a positive y velocity)
            if (_isGrounded && Movement.CurrentVelocity.y < 0.01f)
                _stateMachine.ChangeState(_player.IdleState);
            else
                _stateMachine.ChangeState(_player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public Collider2D[] GetHits(Vector2 origin, Vector2 size, LayerMask mask) => Physics2D.OverlapBoxAll(origin, size, 0f, mask);

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

    public void ApplyKnockbackToHits(Vector2 angle, float force, int direction, float duration, bool ignoreGravity)
    {
        foreach (Collider2D c in _hits)
        {
            Debug.Log("Knockback " + c.gameObject.name);

            IKnockbackable knockbackable = c.gameObject.GetComponentInChildren<IKnockbackable>();

            if (knockbackable != null)
            {
                knockbackable.Knockback(angle, force, direction, duration, ignoreGravity);
            }
            else
            {
                Debug.Log("Knockbackable is null?!");
            }
        }
    }

    public void ResetAbilityCount() => _currentAbilityCount = 0;

    public void IncrementAbilityCount() => _currentAbilityCount = (_currentAbilityCount + 1) % _maxAbilityCount;
}
