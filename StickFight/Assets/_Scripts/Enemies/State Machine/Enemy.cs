using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine StateMachine;

    public D_Enemy EnemyData;

    public Animator Anim { get; private set; }
    public GameObject EnemyModelGO { get; private set; }
    public GameObject EnemyModelBones { get; private set; } // these are the bones controlled by the individual animations
    public EnemyAnimationToStateMachineHandler AnimHandler { get; private set; }
    public Core Core { get; private set; }

    [SerializeField] private Transform _playerCheck;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        EnemyModelGO = transform.Find("EnemyModel").gameObject;
        EnemyModelBones = EnemyModelGO.transform.GetChild(0).gameObject;

        Anim = EnemyModelGO.GetComponent<Animator>();
        AnimHandler = EnemyModelGO.GetComponent<EnemyAnimationToStateMachineHandler>();

        StateMachine = new EnemyStateMachine();
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpate();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right, EnemyData.MinAgroDistance, EnemyData.WhatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right, EnemyData.MaxAgroDistance, EnemyData.WhatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(_playerCheck.position, transform.right, EnemyData.CloseRangeActionDistance, EnemyData.WhatIsPlayer);
    }

    public virtual void ChangeRotationOfModelBones(Quaternion angle)
    {
        EnemyModelBones.transform.rotation = angle;
    }
}
