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
        //Start wait timer after wait timer, check number of enemies attacking
        //if none go to attack state
        //else if player has moved
    }

    public override void UpdateState()
    {
        Debug.Log("AttackWaitState");
        
        

        base.UpdateState();
    }

    public override void Exit()
    {
        
    }
}
