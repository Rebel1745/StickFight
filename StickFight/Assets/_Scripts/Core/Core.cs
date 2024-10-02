using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public CC_Movement Movement
    {
        get => GenericNotImplementedError<CC_Movement>.TryGet(_movement, transform.parent.name);
        private set => _movement = value;
    }
    public CC_CollisionSenses CollisionSenses
    {
        get => GenericNotImplementedError<CC_CollisionSenses>.TryGet(_collisionSenses, transform.parent.name);
        private set => _collisionSenses = value;
    }
    public CC_Combat Combat
    {
        get => GenericNotImplementedError<CC_Combat>.TryGet(_combat, transform.parent.name);
        private set => _combat = value;
    }

    private CC_Movement _movement;
    private CC_CollisionSenses _collisionSenses;
    private CC_Combat _combat;

    private void Awake()
    {
        Movement = GetComponentInChildren<CC_Movement>();
        CollisionSenses = GetComponentInChildren<CC_CollisionSenses>();
        Combat = GetComponentInChildren<CC_Combat>();
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
        Combat.LogicUpdate();
    }
}
