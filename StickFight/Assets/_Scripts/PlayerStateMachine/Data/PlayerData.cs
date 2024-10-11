using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/PlayerData/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Defaults")]
    public float DefaultGravityScale = 1f;
    public Vector2 DefaultHitboxOffset;
    public Vector2 DefaultHitboxSize;
    public float DefaultLinearDrag = 0f;

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

    [Header("Punch State")]
    public float PunchDuration = 0.1f;
    public float PostPunchKnockupPower = 1f;
    public Vector2 GroundPunchKnockbackAngle = Vector2.one;
    public float GroundPunchKnockbackForce = 10f;
    public float GroundPunchDamage = 10f;
    public bool IsPunchMultiAbility = true;
    public int MaxPunchCount = 2;
    public float MultiPunchResetTime = 1f;

    [Header("Ground Kick State")]
    public float KickDuration = 0.1f;
    public float PostKickKnockupPower = 5f;
    public Vector2 GroundKickKnockbackAngle = Vector2.one;
    public float GroundKickKnockbackForce = 10f;
    public float GroundKickDamage = 20f;
    public bool IsKickMultiAbility = true;
    public int MaxKickCount = 2;
    public float MultiKickResetTime = 1f;

    [Header("Dash Slide State")]
    public float DashSlideDamage = 5f;
    public Vector2 DashSlideKnockbackAngle = new(0.2f, 2f);
    public float DashSlideKnockbackForce = 10f;

    [Header("Dash Kick State")]
    public float DashKickDamage = 5f;
    public Vector2 DashKickKnockbackAngle = new(2f, 0f);
    public float DashKickKnockbackForce = 10f;
    public float DashKickDuration = 1f;
}
