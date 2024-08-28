using System.Collections;
using System.Collections.Generic;
using StickFight;
using UnityEngine;

public class E_Zombie : Enemy
{
    public E_Zombie_IdleState IdleState { get; private set; }
    public E_Zombie_MoveState MoveState { get; private set; }

    [SerializeField] private D_IdleStateData _idleStateData;
    [SerializeField] private D_MoveStateData _moveStateData;

    public override void Start()
    {
        base.Start();

        IdleState = new E_Zombie_IdleState(this, StateMachine, "idle", _idleStateData, this);
        MoveState = new E_Zombie_MoveState(this, StateMachine, "move", _moveStateData, this);

        StateMachine.Initialise(MoveState);
    }
}
