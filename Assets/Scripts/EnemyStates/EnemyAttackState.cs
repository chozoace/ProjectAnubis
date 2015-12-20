using UnityEngine;
using System.Collections;

public class EnemyAttackState : State
{
    float _maxSpeed;
    float _currentXSpeed = 0;
    bool _attackStarted = false;

    public EnemyAttackState(EnemyAI fighterRef)
    {
        _stateName = "EnemyAttackState";
        _fighterRef = fighterRef;
    }

    public override void Enter()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _maxSpeed = _fighterRef.MaxSpeed;
        _fighterRigidBody = _fighterRef.GetComponent<Rigidbody2D>();
    }
	
	public override void UpdateState() 
    {
        if (!_fighterRef.GetComponent<Fighter>().IsHitstunned)
        {
            //Move the enemy towards player
            if (!_attackStarted)
            {
                if (_playerRef.transform.position.x - _fighterRef.transform.position.x > 0)
                {
                    //Go right
                    _currentXSpeed = _maxSpeed;
                }
                else
                {
                    _currentXSpeed = -_maxSpeed;
                }
            }
            //When close enough attack
            if( Mathf.Abs(_playerRef.transform.position.x - _fighterRef.transform.position.x) < .8)
            {
                _currentXSpeed = 0;
                if(!_attackStarted)
                    _fighterRef.GetComponent<PlayerCombatScript>().StartAttack();
                _attackStarted = true;
            }
            if(_attackStarted)
            {
                if(!_fighterRef.GetComponent<Fighter>().Attacking)
                    _fighterRef.ChangeState(new EnemyRetreatState(_fighterRef));
            }

            Vector2 v = _fighterRigidBody.velocity;
            v.x = _currentXSpeed;
            _fighterRigidBody.velocity = v;
        }
	}

    public override void Exit()
    {
        base.Exit();
    }
}
