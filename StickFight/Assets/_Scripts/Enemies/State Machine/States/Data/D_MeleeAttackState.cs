using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/Enemy State Data/Melee Attack State Data")]
public class D_MeleeAttackState : ScriptableObject
{
    public float AttackRadius = 0.5f;
    public float AttackDamage = 10f;

    public LayerMask WhatIsPlayer;
}
