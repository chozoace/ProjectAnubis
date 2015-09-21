using UnityEngine;
using System.Collections;

public class EnemyPatrolState : State
{
    EnemyAI _fighterRef;
    GameObject _playerRef;
    Vector2 _playerPosition;
    Vector2 _myPosition;

    public EnemyPatrolState(EnemyAI fighterRef)
    {
        _stateName = "PatrolState";
        _fighterRef = fighterRef;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        if (_playerRef)
        {
            _playerPosition = _playerRef.transform.position;
            _myPosition = _fighterRef.transform.position;

            if (Mathf.Abs(_myPosition.x - _playerPosition.x) <= 5)
            {
                //Debug.Log("out of patrol");
                _fighterRef.ChangeState(new EnemyApproachState(_fighterRef));
            }
            //else
                //Debug.Log("On patrol");
        }
        else
        {
            if(PlayerControllerScript.Instance())
                _playerRef = PlayerControllerScript.Instance().gameObject;
        }

        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
