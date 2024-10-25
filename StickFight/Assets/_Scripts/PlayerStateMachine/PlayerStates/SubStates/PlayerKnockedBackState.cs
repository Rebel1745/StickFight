using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockedBackState : PlayerState
{
    // initial variables
    private PlayerState _returnState;
    private Vector2 _angle;
    private float _strength;
    private float _duration;
    private int _direction;
    private bool _ignoreGravity;

    // housekeeping variable
    private float _knockbackStartTime;

    public PlayerKnockedBackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public void SetKnockbackVariables(PlayerState returnState, Vector2 angle, float strength, int direction, float duration, bool ignoreGravity)
    {
        _returnState = returnState;
        _angle = angle;
        _strength = strength;
        _duration = duration;
        _direction = direction;
        _ignoreGravity = ignoreGravity;
    }

    public override void Enter()
    {
        base.Enter();

        if (_ignoreGravity) Movement?.SetGravityScaleZero();
        Movement?.SetVelocity(_strength, _angle, _direction);
        Movement.CanSetVelocity = false;
        _knockbackStartTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if the knockback time has been exceeded or if we don't have a duration specified and we are on the way down and touching the ground, we can set the velocity again
        if ((_duration != 0f && Time.time > _knockbackStartTime + _duration) || (_duration == 0f && Movement.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground))
        {
            _stateMachine.ChangeState(_returnState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        Movement.CanSetVelocity = true;
        Movement.SetVelocityZero();
        if (_ignoreGravity) Movement.ResetGravityScale();
    }
}
