using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Combat : CoreComponent, IDamageable, IKnockbackable
{
    private bool _isKnockbackActive;
    private bool _ignoreGravity;
    private float _knockbackStartTime;
    private float _knockbackDuration;

    public void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        Debug.Log(_core.transform.parent.name + " Damaged!");
    }

    public void Knockback(Vector2 angle, float strength, int direction, float duration, bool ignoreGravity)
    {
        _ignoreGravity = ignoreGravity;
        if (_ignoreGravity) _core.Movement.SetGravityScaleZero();
        _core.Movement.SetVelocity(strength, angle, direction);
        _core.Movement.CanSetVelocity = false;
        _isKnockbackActive = true;
        _knockbackStartTime = Time.time;
        _knockbackDuration = duration;
    }

    private void CheckKnockback()
    {
        if (!_isKnockbackActive) return;

        // if the knockback time has been exceeded or if we don't have a duration specified and we are on the way down and touching the ground, we can set the velocity again
        if ((_knockbackDuration != 0f && Time.time > _knockbackStartTime + _knockbackDuration) || (_knockbackDuration == 0f && _core.Movement.CurrentVelocity.y <= 0.01f && _core.CollisionSenses.Ground))
        {
            _isKnockbackActive = false;
            _core.Movement.CanSetVelocity = true;
            _core.Movement.SetVelocityZero();
            if (_ignoreGravity) _core.Movement.ResetGravityScale();
        }
    }
}
