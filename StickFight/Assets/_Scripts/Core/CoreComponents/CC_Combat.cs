using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject _damageParticles;
    [SerializeField] private Transform _damageParticleSpawnPoint;

    private bool _isKnockbackActive;
    private bool _ignoreGravity;
    private float _knockbackStartTime;
    private float _knockbackDuration;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        Stats?.DecreaseHealth(amount);
        ParticleManager?.StartParticlesWithRandomRotation(_damageParticles, _damageParticleSpawnPoint.position);
    }

    public void Knockback(Vector2 angle, float strength, int direction, float duration, bool ignoreGravity)
    {
        _ignoreGravity = ignoreGravity;
        if (_ignoreGravity) Movement?.SetGravityScaleZero();
        Movement?.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;
        _isKnockbackActive = true;
        _knockbackStartTime = Time.time;
        _knockbackDuration = duration;
    }

    private void CheckKnockback()
    {
        if (!_isKnockbackActive) return;

        // if the knockback time has been exceeded or if we don't have a duration specified and we are on the way down and touching the ground, we can set the velocity again
        if ((_knockbackDuration != 0f && Time.time > _knockbackStartTime + _knockbackDuration) || (_knockbackDuration == 0f && Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground))
        {
            _isKnockbackActive = false;
            Movement.CanSetVelocity = true;
            Movement?.SetVelocityZero();
            if (_ignoreGravity) Movement?.ResetGravityScale();
        }
    }
}
