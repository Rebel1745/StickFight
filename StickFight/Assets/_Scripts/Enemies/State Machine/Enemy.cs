using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine StateMachine;

    public D_Enemy EnemyData;

    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public GameObject EnemyModelGO { get; private set; }

    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _groundCheck;

    public int FacingDirection { get; private set; }
    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        FacingDirection = 1;

        EnemyModelGO = transform.Find("EnemyModel").gameObject;
        RB = GetComponent<Rigidbody2D>();
        Anim = EnemyModelGO.GetComponent<Animator>();

        StateMachine = new EnemyStateMachine();
    }

    public virtual void Update()
    {
        StateMachine.CurrentState.LogicUpate();
    }

    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityX(float velocityX)
    {
        velocityWorkspace.Set(FacingDirection * velocityX, RB.velocity.y);
        RB.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(_wallCheck.position, EnemyModelGO.transform.right, EnemyData.WallCheckDistance, EnemyData.WhatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.Raycast(_groundCheck.position, Vector2.down, EnemyData.GroundCheckDistance, EnemyData.WhatIsGround);
    }

    public virtual void Flip()
    {
        FacingDirection *= -1;
        EnemyModelGO.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position + (Vector3)(Vector2.right * FacingDirection * EnemyData.WallCheckDistance));
        Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + (Vector3)(Vector2.down * EnemyData.GroundCheckDistance));
    }
}
