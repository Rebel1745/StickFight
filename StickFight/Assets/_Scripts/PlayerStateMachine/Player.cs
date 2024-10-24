using System.Collections;
using System.Collections.Generic;
using StickFight;
using UnityEngine;

public class Player : MonoBehaviour, IKnockbackable
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    //public PlayerLandState LandState { get; private set; }
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
    public PlayerKnockedBackState KnockedBackState { get; private set; }
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim;
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerAnimationToStateMachineHandler AnimHandler { get; private set; }
    #endregion

    #region Other Variables

    [SerializeField] private PlayerData _playerData;

    // create a Vector2 that we can work with for holding velocities
    private Vector2 _workspace;
    #endregion

    #region CheckTransforms
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
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, _playerData, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, _playerData, "Run");
        JumpState = new PlayerJumpState(this, StateMachine, _playerData, "Jump");
        InAirState = new PlayerInAirState(this, StateMachine, _playerData, "Falling");
        //LandState = new PlayerLandState(this, StateMachine, _playerData, "Jump_landing");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, _playerData, "Wall_Slide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, _playerData, "Wall_Cling");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, _playerData, "Wall_Climb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, _playerData, "Falling");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, _playerData, "Ledge_Climb");
        CeilingClingState = new PlayerCeilingClingState(this, StateMachine, _playerData, "Hanging");
        CeilingMoveState = new PlayerCeilingMoveState(this, StateMachine, _playerData, "Hanging_Move");
        DashStandardState = new PlayerDashStandardState(this, StateMachine, _playerData, "Dash");
        DashPunchState = new PlayerDashPunchState(this, StateMachine, _playerData, "Dash_Punch");
        DashKickState = new PlayerDashKickState(this, StateMachine, _playerData, "Dash_Kick");
        DashSlideState = new PlayerDashSlideState(this, StateMachine, _playerData, "Dash_Slide");
        GroundPunchState = new PlayerGroundPunchState(this, StateMachine, _playerData, "Punch");
        AirPunchState = new PlayerAirPunchState(this, StateMachine, _playerData, "Air_Punch");
        GroundKickState = new PlayerGroundKickState(this, StateMachine, _playerData, "Kick");
        AirKickState = new PlayerAirKickState(this, StateMachine, _playerData, "Air_Kick");
        KnockedBackState = new PlayerKnockedBackState(this, StateMachine, _playerData, "Dash_Punch");
    }

    private void Start()
    {
        //Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        AnimHandler = GetComponentInChildren<PlayerAnimationToStateMachineHandler>();
        AnimHandler.Player = this;

        StateMachine.Initialise(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Other Functions

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    public void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishedTrigger();
    #endregion

    #region Global Set Functions
    // these functions set the flags for 'interrupter flags' i.e. damage, knockback, and knockup
    public void Knockback(Vector2 angle, float strength, int direction, float duration, bool ignoreGravity)
    {
        StateMachine.CurrentState.SetKnockedBack(true);
        KnockedBackState.SetKnockbackVariables(StateMachine.CurrentState, angle, strength, direction, duration, ignoreGravity);
    }
    #endregion

    #region Debug Functions
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(HitCheckOriginGroundKick.position, HitBoxSizeGroundKick);
    }
    #endregion
}
