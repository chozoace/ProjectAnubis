using UnityEngine;
using System.Collections;

public class State
{
    //Enemy states
    /*public static EnemyPatrolState _enemyPatrolState = new EnemyPatrolState();
    public static EnemyRetreatState _enemyRetreateState = new EnemyRetreatState();
    public static EnemyAttackWaitState _enemyAttackWaitState = new EnemyAttackWaitState();
    public static EnemyAttackState _enemyAttackState = new EnemyAttackState();
    public static EnemyApproachState _enemyApproachState = new EnemyApproachState();*/

    protected EnemyAI _fighterRef;
    protected Rigidbody2D _fighterRigidBody;
    protected GameObject _playerRef;
    protected bool _canAttack = false;
    public bool CanAttack { get { return _canAttack; } }

    protected string _stateName = "Default";
    public virtual string StateName { get { return _stateName; } }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void UpdateState() { }

}
