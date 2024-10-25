using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : PlayerState
{
    private AttackDetails _attackDetails;
    private PlayerState _previousState;
    private float _currentHealth;

    public PlayerDamagedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName) : base(player, stateMachine, playerData, animName)
    {
    }

    public void SetDamagedVariables(AttackDetails attackDetails, PlayerState previousState)
    {
        _attackDetails = attackDetails;
        _previousState = previousState;
    }

    public override void Enter()
    {
        base.Enter();

        _currentHealth = Stats.DecreaseHealth(_attackDetails.AttackDamage);
        Debug.Log(_currentHealth);

        if (_currentHealth == 0f)
        {
            Debug.Log("Dead");
        }
        else
        {
            // TODO: insert transition to knockback state (maybe rework knockback to send only AttackDetails)
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        Debug.Log(_previousState);
        _stateMachine.ChangeState(_previousState);
    }

    public override void Exit()
    {
        base.Exit();
        _previousState.SetDamaged(false);
    }
}
