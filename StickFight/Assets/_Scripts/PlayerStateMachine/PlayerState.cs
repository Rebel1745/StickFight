using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core _core;

    protected Player _player;
    protected PlayerStateMachine _stateMachine;
    protected PlayerData _playerData;

    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    protected float _startTime;

    private string _animName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _playerData = playerData;
        _animName = animName;
        _core = _player.Core;
    }

    public virtual void Enter()
    {
        DoChecks();

        _player.Anim.SetBool(_animName, true);
        _startTime = Time.time;
        _isAnimationFinished = false;
        _isExitingState = false;
    }
    public virtual void Exit()
    {
        _player.Anim.SetBool(_animName, false);
        _isExitingState = true;
    }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishedTrigger() => _isAnimationFinished = true;
}
