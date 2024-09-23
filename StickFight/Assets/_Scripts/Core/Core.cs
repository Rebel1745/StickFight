using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public CC_Movement Movement { get; private set; }
    public CC_CollisionSenses CollisionSenses { get; private set; }

    private void Awake()
    {
        Movement = GetComponentInChildren<CC_Movement>();
        CollisionSenses = GetComponentInChildren<CC_CollisionSenses>();

        if (!Movement || !CollisionSenses)
        {
            Debug.LogError("Missing core component");
        }
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
