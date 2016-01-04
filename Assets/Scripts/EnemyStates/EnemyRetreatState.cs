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
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _maxSpeed = _fighterRef.MaxSpeed - .5f;
        _fighterRigidBody = _fighterRef.GetComponent<Rigidbody2D>();

        _fighterRef._walkingReversed = true;
    }
	
	// Update is called once per frame
    public override void UpdateState()
    {
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
            if (Mathf.Abs(_playerRef.transform.position.x - _fighterRef.transform.position.x) > 2.5)
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
        _fighterRef._walkingReversed = false;
    }
}
