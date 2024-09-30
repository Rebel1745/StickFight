using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Combat : CoreComponent, IDamageable, IKnockbackable
{
    public void Damage(float amount)
    {
        Debug.Log(_core.transform.parent.name + " Damaged!");
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        _core.Movement.SetVelocity(strength, angle, direction);
    }
}
