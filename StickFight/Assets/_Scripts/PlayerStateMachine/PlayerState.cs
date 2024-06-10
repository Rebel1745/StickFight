using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player _player;
    protected PlayerStateMachine _stateMachine;
    protected PlayerData _playerData;

    protected float _startTime;

    private string _animName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _playerData = playerData;
        _animName = animName;
    }

    public virtual void Enter()
    {
        DoChecks();
        _player.Anim.Play(_animName);
        _startTime = Time.time;
        Debug.Log(_animName);
    }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }
}
