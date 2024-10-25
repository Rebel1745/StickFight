using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackDetails
{
    public string name;
    public float AttackRadius;
    public float AttackDamage;
    public Vector2 KnockbackAngle;
    public float KnockbackStrength;
    public bool DisableGravity;
}
