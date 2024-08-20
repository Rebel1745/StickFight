using System.Collections;
using System.Collections.Generic;
using StickFight;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerCeilingClingState CeilingClingState { get; private set; }
    public PlayerCeilingMoveState CeilingMoveState { get; private set; }
    public PlayerDashStandardState DashStandardState { get; private set; }
    public PlayerDashPunchState DashPunchState { get; private set; }
    public PlayerDashKickState DashKickState { get; private set; }
    public PlayerDashSlideState DashSlideState { get; private set; }
    public PlayerGroundPunchState GroundPunchState { get; private set; }
    public PlayerAirPunchState AirPunchState { get; private set; }
    public PlayerGroundKickState GroundKickState { get; private set; }
    public PlayerAirKickState AirKickState { get; private set; }
    #endregion

    #region Components
    public Animator Anim;
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public BoxCollider2D Col { get; private set; }
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    [SerializeField] private PlayerData _playerData;

    // create a Vector2 that we can work with for holding velocities
    private Vector2 _workspace;
    #endregion

    #region CheckTransforms
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _ledgeCheck;
    [SerializeField] private Transform _ceilingCheck;
    public LayerMask WhatIsEnemy;
    [Tooltip("Location of the center of the box that checks hits for ground punches")] public Transform HitCheckOriginGroundPunch;
    public Vector2 HitBoxSizeGroundPunch;
    [Tooltip("Location of the center of the box that checks hits for punches while jumping")] public Transform HitCheckOriginAirPunch;
    public Vector2 HitBoxSizeAirPunch;
    [Tooltip("Location of the center of the box that checks hits for dash punches")] public Transform HitCheckOriginDashPunch;
    public Vector2 HitBoxSizeDashPunch;
    [Tooltip("Location of the center of the box that checks hits for ground kicks")] public Transform HitCheckOriginGroundKick;
    public Vector2 HitBoxSizeGroundKick;
    [Tooltip("Location of the center of the box that checks hits for kicks while jumping")] public Transform HitCheckOriginAirKick;
    public Vector2 HitBoxSizeAirKick;
    [Tooltip("Location of the center of the box that checks hits for dash kicks")] public Transform HitCheckOriginDashKick;
    public Vector2 HitBoxSizeDashKick;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, _playerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "Run");
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "Jump_up");
        InAirState = new PlayerInAirState(this, StateMachine, _playerData, "Jump_falling");
        LandState = new PlayerLandState(this, StateMachine, _playerData, "Jump_landing");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "Wall_slide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, _playerData, "Wall_cling");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, _playerData, "Wall_climb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, _playerData, "Jump_falling");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, _playerData, "Ledge_climb");
        CeilingClingState = new PlayerCeilingClingState(this, StateMachine, _playerData, "Hanging");
        CeilingMoveState = new PlayerCeilingMoveState(this, StateMachine, _playerData, "Hanging_move");
        DashStandardState = new PlayerDashStandardState(this, StateMachine, _playerData, "Dash");
        DashPunchState = new PlayerDashPunchState(this, StateMachine, _playerData, "Dash_punch_air");
        DashKickState = new PlayerDashKickState(this, StateMachine, _playerData, "Dash_kick");
        DashSlideState = new PlayerDashSlideState(this, StateMachine, _playerData, "Dash_slide");
        GroundPunchState = new PlayerGroundPunchState(this, StateMachine, _playerData, "Punch_1");
        AirPunchState = new PlayerAirPunchState(this, StateMachine, _playerData, "Air_punch");
        GroundKickState = new PlayerGroundKickState(this, StateMachine, _playerData, "Kick_1");
        AirKickState = new PlayerAirKickState(this, StateMachine, _playerData, "Air_kick");
    }

    private void Start()
    {
        //Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        Col = GetComponent<BoxCollider2D>();

        FacingDirection = 1;

        StateMachine.Initialise(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
        float xDist = xHit.distance;
        _workspace.Set(xDist * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(_ledgeCheck.position + (Vector3)_workspace, Vector2.down, _ledgeCheck.position.y - _wallCheck.position.y, _playerData.WhatIsGround);
        float yDist = yHit.distance;

        _workspace.Set(_wallCheck.position.x + xDist * FacingDirection, _ledgeCheck.position.y - yDist);
        return _workspace;
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishedTrigger();
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion

    #region Check Functions

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _playerData.GroundCheckRadius, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(_wallCheck.position, Vector2.right * -FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(_ledgeCheck.position, Vector2.right * FacingDirection, _playerData.WallCheckDistance, _playerData.WhatIsGround);
    }

    public bool CheckIfTouchingCeiling()
    {
        return Physics2D.Raycast(_ceilingCheck.position, Vector2.up, _playerData.CeilingCheckRadius, _playerData.WhatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Set Functions

    public void ResetGravityScale()
    {
        RB.gravityScale = _playerData.DefaultGravityScale;
    }

    public void SetGravityScale(float newGravityScale)
    {
        RB.gravityScale = newGravityScale;
    }

    public void SetGravityScaleZero()
    {
        RB.gravityScale = 0;
    }

    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void SetVelocity(float vel, Vector2 angle, int direction)
    {
        angle.Normalize();
        _workspace.Set(angle.x * vel * direction, angle.y * vel);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetVelocityX(float vel)
    {
        _workspace.Set(vel, CurrentVelocity.y);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetVelocityY(float vel)
    {
        _workspace.Set(CurrentVelocity.x, vel);
        RB.velocity = _workspace;
        CurrentVelocity = _workspace;
    }

    public void SetLinearDrag(float val)
    {
        RB.drag = val;
    }

    public void ResetLinearDrag()
    {
        RB.drag = _playerData.DefaultLinearDrag;
    }

    public void SetBoxCollider(Vector2 offset, Vector2 size)
    {
        Col.size = size;
        Col.offset = offset;
    }
    #endregion

    #region Debug Functions
    private void OnDrawGizmos()
    {

    }
    #endregion
}
