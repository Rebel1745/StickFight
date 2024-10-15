using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected Core _core;

    protected CC_Movement Movement { get => _movement ??= _core.GetCoreComponent<CC_Movement>(); }
    private CC_Movement _movement;
    protected CC_CollisionSenses CollisionSenses { get => _collisionSenses ??= _core.GetCoreComponent<CC_CollisionSenses>(); }
    private CC_CollisionSenses _collisionSenses;
    protected CC_Stats Stats { get => _stats ??= _core.GetCoreComponent<CC_Stats>(); }
    private CC_Stats _stats;

    protected virtual void Awake()
    {
        _core = transform.parent.GetComponent<Core>();

        if (!_core) Debug.LogError("There is no core on the parent");

        _core.AddComponent(this);
    }

    public virtual void LogicUpdate() { }
}
