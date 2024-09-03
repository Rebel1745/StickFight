using System.Collections;
using System.Collections.Generic;
using StickFight;
using UnityEngine;

public class E_Zombie : Enemy
{
    public E_Zombie_IdleState IdleState { get; private set; }
    public E_Zombie_MoveState MoveState { get; private set; }
    public E_Zombie_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E_Zombie_ChargeState ChargeState { get; private set; }
    public E_Zombie_LookForPlayerState LookForPlayerState { get; private set; }

    [SerializeField] private D_IdleStateData _idleStateData;
    [SerializeField] private D_MoveStateData _moveStateData;
    [SerializeField] private D_PlayerDetectedStateData _playerDetectedStateData;
    [SerializeField] private D_ChargeStateData _chargeStateData;
    [SerializeField] private D_LookForPlayerData _lookForPlayerStateData;

    public override void Start()
    {
        base.Start();

        IdleState = new E_Zombie_IdleState(this, StateMachine, "idle", _idleStateData, this);
        MoveState = new E_Zombie_MoveState(this, StateMachine, "move", _moveStateData, this);
        PlayerDetectedState = new E_Zombie_PlayerDetectedState(this, StateMachine, "playerDetected", _playerDetectedStateData, this);
        ChargeState = new E_Zombie_ChargeState(this, StateMachine, "charge", _chargeStateData, this);
        LookForPlayerState = new E_Zombie_LookForPlayerState(this, StateMachine, "lookForPlayer", _lookForPlayerStateData, this);

        StateMachine.Initialise(MoveState);
    }

    // TODO: create a TurnState which just animates the zombie turning around
    /*public override void Flip()
    {
        FlipFacingDirection();
        Anim.Play("Turn");
    }*/
}
