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
    public AnimationToStateMachineHandler AnimHandler { get; private set; }

    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _playerCheck;

    public int FacingDirection { get; private set; }
    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        FacingDirection = 1;

        EnemyModelGO = transform.Find("EnemyModel").gameObject;
        RB = GetComponent<Rigidbody2D>();
        Anim = EnemyModelGO.GetComponent<Animator>();
        AnimHandler = EnemyModelGO.GetComponent<AnimationToStateMachineHandler>();

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
        return Physics2D.Raycast(_wallCheck.position, transform.right, EnemyData.WallCheckDistance, EnemyData.WhatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.down, EnemyData.LedgeCheckDistance, EnemyData.WhatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.Raycast(_groundCheck.position, Vector2.down, EnemyData.GroundCheckDistance, EnemyData.WhatIsGround);
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

    public virtual void Flip()
    {
        FlipFacingDirection();
        transform.Rotate(0f, 180f, 0f);
    }

    public virtual void FlipFacingDirection()
    {
        FacingDirection *= -1;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_wallCheck.position, _wallCheck.position + (Vector3)(Vector2.right * FacingDirection * EnemyData.WallCheckDistance));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + (Vector3)(Vector2.down * EnemyData.GroundCheckDistance));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_ledgeCheck.position, _ledgeCheck.position + (Vector3)(Vector2.down * EnemyData.LedgeCheckDistance));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_playerCheck.position, _playerCheck.position + (Vector3)(Vector2.right * FacingDirection * EnemyData.MinAgroDistance));
    }
}
