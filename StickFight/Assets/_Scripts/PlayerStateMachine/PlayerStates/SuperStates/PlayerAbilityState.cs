using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool _isAbilityDone;

    protected bool _isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = _core.CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        _isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isAbilityDone)
        {
            // for the first frame or so after jumping we are still 'technically' grounded, so only transition if we are not jumping (i.e. dont have a positive y velocity)
            if (_isGrounded && _core.Movement.CurrentVelocity.y < 0.01f)
                _stateMachine.ChangeState(_player.IdleState);
            else
                _stateMachine.ChangeState(_player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
