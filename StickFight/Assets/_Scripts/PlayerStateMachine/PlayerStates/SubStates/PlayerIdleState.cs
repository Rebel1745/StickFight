using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        // stop all X movement as we are idle
        _core.Movement.SetVelocityZero();
        // Here we set linear drag becuase for some reason (no idea why) after a dash attack hits we keep moving forever
        // this value will be reset when we exit the idle state
        _core.Movement.SetLinearDrag(_playerData.PostDashIdleLinearDrag);
    }

    public override void Exit()
    {
        base.Exit();
        _core.Movement.ResetLinearDrag();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_isExitingState)
        {
            if (_xInput != 0)
            {
                _stateMachine.ChangeState(_player.MoveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
