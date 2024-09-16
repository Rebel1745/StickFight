using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeDetectedStateData", menuName = "Data/Enemy State Data/Charge State Data")]
public class D_ChargeState : ScriptableObject
{
    public float ChargeSpeed = 3f;

    public float ChargeTime = 0.5f;
}
