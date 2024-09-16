using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookForPlayerStateData", menuName = "Data/Enemy State Data/Look For Player State Data")]
public class D_LookForPlayer : ScriptableObject
{
    public int AmountOfTurns = 2;
    public float TimeBetweenTurns = 0.75f;
}
