using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/Enemy State Data/Melee Attack State Data")]
public class D_MeleeAttackState : ScriptableObject
{
    public AttackDetails MeleeAttackDetails;

    public LayerMask WhatIsPlayer;
}
