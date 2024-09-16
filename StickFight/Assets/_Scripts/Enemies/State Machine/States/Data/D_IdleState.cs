using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/Enemy State Data/Idle State Data")]
public class D_IdleState : ScriptableObject
{
    public float MinIdleTime = 1f;
    public float MaxIdleTime = 2f;
}
