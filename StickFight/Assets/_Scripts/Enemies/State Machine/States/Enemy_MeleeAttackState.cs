using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MeleeAttackState : Enemy_AttackState
{
    protected D_MeleeAttackState _stateData;

    protected AttackDetails _attackDetails;

    public Enemy_MeleeAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName, attackPosition)
    {
        _stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        _attackDetails = new AttackDetails
        {
            DamageAmount = _stateData.AttackDamage,
            Position = _enemy.transform.position
        };
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpate()
    {
        base.LogicUpate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attackPosition.position, _stateData.AttackRadius, _stateData.WhatIsPlayer);

        foreach (Collider2D detectedObject in detectedObjects)
        {
            IDamageable damageable = detectedObject.gameObject.GetComponentInChildren<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(_stateData.AttackDamage);
            }
            else
            {
                Debug.Log("Damageable is null?!");
            }

            IKnockbackable knockbackable = detectedObject.gameObject.GetComponentInChildren<IKnockbackable>();

            if (knockbackable != null)
            {
                knockbackable.Knockback(_stateData.KnockbackAngle, _stateData.KnockbackStrength, Movement.FacingDirection, 0f, false);
            }
            else
            {
                Debug.Log("Knockbackable is null?!");
            }
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
