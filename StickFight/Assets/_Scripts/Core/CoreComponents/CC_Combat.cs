using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Combat : CoreComponent, IDamageable, IKnockbackable
{
    private bool _isKnockbackActive;
    private float _knockbackStartTime;

    public void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        Debug.Log(_core.transform.parent.name + " Damaged!");
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        _core.Movement.SetVelocity(strength, angle, direction);
        _core.Movement.CanSetVelocity = false;
        _isKnockbackActive = true;
        _knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        // if we are on the way down and touching the ground, we can set the velocity again
        if (_isKnockbackActive && _core.Movement.CurrentVelocity.y <= 0.01f && _core.CollisionSenses.Ground)
        {
            _isKnockbackActive = false;
            _core.Movement.CanSetVelocity = true;
        }
    }
}
