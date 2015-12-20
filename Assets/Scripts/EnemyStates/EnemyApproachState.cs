using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyApproachState : State 
{
    float _maxSpeed;
    float _currentXSpeed = 0;
    public bool EnemyAttacking { get { return _canAttack; } }

    public EnemyApproachState(EnemyAI fighterRef)
    {
        _stateName = "ApproachState";
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
            if (_playerRef.transform.position.x - _fighterRef.transform.position.x > 0)
            {
                //Go right
                _currentXSpeed = _maxSpeed;
            }
            else
            {
                _currentXSpeed = -_maxSpeed;
            }

            if (Mathf.Abs(_playerRef.transform.position.x - _fighterRef.transform.position.x) < 2)
            {
                //Determine if other unit is attacking via list of enemies in gamecontroller
                List<GameObject> enemyList = GameController.Instance().GetEnemyList;
                foreach (GameObject enemy in enemyList)
                {
                    EnemyAI theEnemyAI = enemy.GetComponent<EnemyAI>();
                    State enemyState = theEnemyAI.GetCurrentState;
                    if (enemyState.StateName == "EnemyAttackState" && enemy != _fighterRef && !enemy.GetComponent<Fighter>().IsHitstunned)
                    {
                        _canAttack = false;
                        break;
                    }
                    _canAttack = true;
                }

                if (!_canAttack)
                {
                    _currentXSpeed = 0;
                    Debug.Log("Name: " + _fighterRef.name);
                    //Change to attack wait
                    //Start wait timer and pause movement
                    //after wait timer, check number of enemies attacking
                    //if none go to attack state
                    //else if player has moved, advance toward player
                    //else start wait timer again
                }
                else if (_canAttack)
                {
                    //Change to attack state
                    //_currentXSpeed = 0;
                    Debug.Log("In attack, Name: " + _fighterRef.name);
                    _fighterRef.ChangeState(new EnemyAttackState(_fighterRef));
                }
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
