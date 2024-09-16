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
    public E_Zombie_MeleeAttackState MeleeAttackState { get; private set; }
    public E_Zombie_TurnState TurnState { get; private set; }

    [SerializeField] private D_IdleState _idleStateData;
    [SerializeField] private D_MoveState _moveStateData;
    [SerializeField] private D_PlayerDetectedState _playerDetectedStateData;
    [SerializeField] private D_ChargeState _chargeStateData;
    [SerializeField] private D_LookForPlayer _lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState _meleeAttackStateData;
    [SerializeField] private D_TurnState _turnStateData;

    [SerializeField] private Transform _meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        IdleState = new E_Zombie_IdleState(this, StateMachine, "idle", _idleStateData, this);
        MoveState = new E_Zombie_MoveState(this, StateMachine, "move", _moveStateData, this);
        PlayerDetectedState = new E_Zombie_PlayerDetectedState(this, StateMachine, "playerDetected", _playerDetectedStateData, this);
        ChargeState = new E_Zombie_ChargeState(this, StateMachine, "charge", _chargeStateData, this);
        LookForPlayerState = new E_Zombie_LookForPlayerState(this, StateMachine, "lookForPlayer", _lookForPlayerStateData, this);
        MeleeAttackState = new E_Zombie_MeleeAttackState(this, StateMachine, "meleeAttack", _meleeAttackPosition, _meleeAttackStateData, this);
        TurnState = new E_Zombie_TurnState(this, StateMachine, "turn", _turnStateData, this);

        StateMachine.Initialise(MoveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(_meleeAttackPosition.position, _meleeAttackStateData.AttackRadius);
    }
}
