using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Defaults")]
    public float DefaultGravityScale = 1f;
    public Vector2 DefaultHitboxOffset;
    public Vector2 DefaultHitboxSize;
    public float DefaultLinearDrag = 0f;

    [Header("Abilities")]
    public bool CanDash = false;
    public bool CanDashSlide = false;
    public bool CanDashPunch = false;
    public bool CanDashKick = false;
    public bool CanPunch = false;
    public bool CanAirPunch = false;
    public bool CanKick = false;
    public bool CanAirKick = false;
    public bool CanWallJump = false;
    public bool CanWallCling = false;
    public bool CanWallClimb = false;
    public bool CanWallSlide = false;
    public bool CanCeilingCling = false;
    public bool CanCeilingMove = false;
    public bool CanLedgeClimb = false;

    [Header("Move State")]
    public float MovementVelocity = 10f;

    [Header("Jump State")]
    public float JumpVelocity = 15f;
    public int AmountOfJumps = 1;
    public float UpwardMovementGravityScale = 1.7f;
    public float DownwardMovementGravityScale = 3f;
    public Vector2 JumpingHitboxOffset;
    public Vector2 JumpingHitboxSize;

    [Header("In Air State")]
    public float CoyoteTime = 0.1f;
    public float VariableJumpHeightMultiplier = 0.5f;

    [Header("Wall States")]
    public Vector2 WallHitboxOffset;
    public Vector2 WallHitboxSize;

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
    public float PostDashIdleLinearDrag = 10f;
    public Vector2 DashStandardHitboxOffset;
    public Vector2 DashStandardHitboxSize;

    [Header("Ground Punch State")]
    public AttackDetails GroundPunchAttackDetails;
    public bool IsPunchMultiAbility = true;
    public int MaxPunchCount = 2;
    public float MultiPunchResetTime = 1f;

    [Header("Air Punch State")]
    public AttackDetails AirPunchAttackDetails;
    public float AirPunchDuration = 0.1f;

    [Header("Ground Kick State")]
    public AttackDetails GroundKickAttackDetails;
    public bool IsKickMultiAbility = true;
    public int MaxKickCount = 2;
    public float MultiKickResetTime = 1f;

    [Header("Air Kick State")]
    public AttackDetails AirKickAttackDetails;
    public float AirKickDuration = 0.1f;

    [Header("Dash Slide State")]
    public AttackDetails DashSlideAttackDetails;

    [Header("Dash Kick State")]
    public AttackDetails DashKickAttackDetails;
}
