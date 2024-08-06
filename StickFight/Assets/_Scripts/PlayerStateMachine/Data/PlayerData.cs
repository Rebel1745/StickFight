using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float MovementVelocity = 10f;

    [Header("Jump State")]
    public float JumpVelocity = 15f;
    public int AmountOfJumps = 1;
    public float DefaultGravityScale = 1f;
    public float UpwardMovementGravityScale = 1.7f;
    public float DownwardMovementGravityScale = 3f;

    [Header("In Air State")]
    public float CoyoteTime = 0.1f;
    public float VariableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide State")]
    public float WallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float WallClimbVelocity = 3f;

    [Header("Wall Jump State")]
    public float WallJumpVelocity = 20f;
    public float WallJumpTime = 0.4f;
    public Vector2 WallJumpAngle = new Vector2(1, 2);

    [Header("Ledge Climb State")]
    public Vector2 StartOffset;
    public Vector2 StopOffset;

    [Header("Dash State")]
    public float DashCooldown = 0.5f;
    public float DashTime = 0.3f;
    public float DashVelocity = 30f;
    public float DefaultLinearDrag = 0f;
    public float PostDashIdleLinearDrag = 10f;

    [Header("Punch State")]
    public float PunchDuration = 0.1f;
    public float PostPunchKnockupPower = 1f;

    [Header("Kick State")]
    public float KickDuration = 0.1f;
    public float PostKickKnockupPower = 5f;

    [Header("Check Variables")]
    public float GroundCheckRadius = 0.3f;
    public float WallCheckDistance = 0.5f;
    public LayerMask WhatIsGround;
}
