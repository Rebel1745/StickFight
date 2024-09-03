using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/Enemy State Data/Player Detected State Data")]
public class D_PlayerDetectedStateData : ScriptableObject
{
    public float LongRangeActionTime = 1.5f;
    public float ShortRangeActionTime = 0.5f;
}
