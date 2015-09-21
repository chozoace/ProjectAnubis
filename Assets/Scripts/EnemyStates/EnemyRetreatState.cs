using UnityEngine;
using System.Collections;

public class EnemyRetreatState : State
{
    float _maxSpeed;
    float _currentXSpeed = 0;

    public EnemyRetreatState(EnemyAI fighterRef)
    {
        _stateName = "RetreatState";
        _fighterRef = fighterRef;
    }

    public override void Enter()
    {
        Debug.Log("entered retreat");
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _maxSpeed = _fighterRef.MaxSpeed - .5f;
        _fighterRigidBody = _fighterRef.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
    public override void UpdateState()
    {
        Debug.Log("In Retreat state");
        if (!_fighterRef.GetComponent<Fighter>().IsHitstunned)
        {
            //Move the enemy towards player
            if (_playerRef.transform.position.x - _fighterRef.transform.position.x > 0)
            {
                //Go left
                _currentXSpeed = -_maxSpeed;
            }
            else
            {
                _currentXSpeed = _maxSpeed;
            }
            if (Mathf.Abs(_playerRef.transform.position.x - _fighterRef.transform.position.x) > 3)
            {
                _fighterRef.ChangeState(new EnemyApproachState(_fighterRef));
            }

            Vector2 v = _fighterRigidBody.velocity;
            v.x = _currentXSpeed;
            _fighterRigidBody.velocity = v;
        }
    }

    public override void Exit()
    {
        
    }
}
