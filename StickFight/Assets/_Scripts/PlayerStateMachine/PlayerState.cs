using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core _core;

    protected Player _player;
    protected PlayerStateMachine _stateMachine;
    protected PlayerData _playerData;

    protected CC_Movement Movement { get => _movement ??= _core.GetCoreComponent<CC_Movement>(); }
    private CC_Movement _movement;
    protected CC_CollisionSenses CollisionSenses { get => _collisionSenses ??= _core.GetCoreComponent<CC_CollisionSenses>(); }
    private CC_CollisionSenses _collisionSenses;
    protected CC_Stats Stats { get => _stats ??= _core.GetCoreComponent<CC_Stats>(); }
    private CC_Stats _stats;

    protected bool _isAnimationFinished;
    protected bool _isExitingState;
    // super flags
    protected bool _playerDamaged = false;
    protected bool _playerKnockedBack = false;
    protected bool _playerKnockedUp = false;

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

    public void SetKnockedBack(bool knockedBack)
    {
        _playerKnockedBack = knockedBack;
    }
}
