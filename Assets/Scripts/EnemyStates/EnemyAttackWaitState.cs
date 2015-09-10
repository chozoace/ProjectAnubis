using UnityEngine;
using System.Collections;

public class EnemyAttackWaitState : State
{
    EnemyAI _fighterRef;
    GameObject _playerRef;

    public EnemyAttackWaitState(EnemyAI fighterRef)
    {
        _stateName = "AttackWaitState";
        _fighterRef = fighterRef;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        Debug.Log("AttackWaitState");
        
        //Move the enemy towards player

        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
