using UnityEngine;
using System.Collections;

public class EnemyApproachState : State 
{
    EnemyAI _fighterRef;
    GameObject _playerRef;

    public EnemyApproachState(EnemyAI fighterRef)
    {
        _stateName = "ApproachState";
        _fighterRef = fighterRef;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        Debug.Log("ApproachState");
        
        //Move the enemy towards player



        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
