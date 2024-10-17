using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/Enemy State Data/Move State Data")]
public class D_MoveState : ScriptableObject
{
    public float MovementSpeed = 3f;
    public Transform FootstepParticleSpawnPoint;
    public GameObject FootstepParticles;
}
