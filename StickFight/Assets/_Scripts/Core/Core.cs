using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public CC_Movement Movement
    {
        get
        {
            if (_movement) return _movement;

            Debug.LogError("No Movement Core Component on " + transform.parent.name);
            return null;
        }
        private set { _movement = value; }
    }
    public CC_CollisionSenses CollisionSenses
    {
        get
        {
            if (_collisionSenses) return _collisionSenses;

            Debug.LogError("No Collision Senses Core Component on " + transform.parent.name);
            return null;

        }
        private set { _collisionSenses = value; }
    }

    private CC_Movement _movement;
    private CC_CollisionSenses _collisionSenses;

    private void Awake()
    {
        Movement = GetComponentInChildren<CC_Movement>();
        CollisionSenses = GetComponentInChildren<CC_CollisionSenses>();
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
