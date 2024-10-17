using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.CheckIfShouldFlip(_xInput);

        Movement?.SetVelocityX(_xInput * _playerData.MovementVelocity);

        if (!_isExitingState)
        {
            if (_xInput == 0)
            {
                _stateMachine.ChangeState(_player.IdleState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        // play the footstep particle
        Movement?.PlayFootstepParticles();
    }
}
