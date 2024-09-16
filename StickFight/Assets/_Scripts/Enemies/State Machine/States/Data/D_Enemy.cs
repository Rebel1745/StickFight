using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class D_Enemy : ScriptableObject
{
    public float WallCheckDistance = 0.2f;
    public float LedgeCheckDistance = 0.4f;
    public float GroundCheckDistance = 0.2f;

    public float MinAgroDistance = 3f;
    public float MaxAgroDistance = 4f;

    public float CloseRangeActionDistance = 1f;

    public LayerMask WhatIsGround;
    public LayerMask WhatIsPlayer;
}
