using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class PlayerCombatScript : MonoBehaviour 
{
    [SerializeField] List<Attack> _openingAttacks;
    List<Move> _moveQueue;
    //Used to instantiate attack
    Attack _attackPrefab;
    //the instance of the attack
    Attack _currentAttack;
    Fighter _fighterRef;
    Move _movePrefab;

    bool _moveListened = false;

	void Start () 
    {
        _moveQueue = new List<Move>();
        _fighterRef = this.gameObject.GetComponent<Fighter>();
	}

    void FixedUpdate()
    {
        //Executes attack from list when ready to fire
        //If attacking
        if(gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            //check if move.canfire is true, if so execute move in list
            if(_moveQueue.Count > 0 && _currentAttack.NextMoveExecute)
            {
                if (_moveQueue[0].IsJump && _currentAttack.CanJumpCancel)
                {
                    _currentAttack.EndAttack();
                    ExecuteMove(_moveQueue[0]);
                }
                else if(_moveQueue[0].IsJump && !_currentAttack.CanJumpCancel)
                {
                    _moveQueue.RemoveAt(0);
                }
                else if(_moveQueue[0].IsAttack)
                {
                    _currentAttack.EndAttack();
                    ExecuteMove(_moveQueue[0]);
                }
                
            }
            /*foreach (Move move in _moveQueue)
            {
                if ((move.IsAttack && _currentAttack.NextMoveExecute) ||
                    (move.IsJump && _currentAttack.CanJumpCancel))
                {
                    _currentAttack.EndAttack();
                    ExecuteMove(move);
                }
            }*/

        }
        else
        {
            if (_moveQueue.Count > 0)
            {
                ExecuteMove(_moveQueue[0]);
            }
        }
    }

    void ExecuteMove(Move move)
    {
        //Chooses attack from queue and executes it
        //Debug.Log("Choosing attack from attack queue");
        //Try and have this go through the list until it finds
        //a viable attack, any non viable attacks are thrown away
        if (_moveQueue[0].IsAttack)
        {
            _attackPrefab = (Attack)_moveQueue[0];
            _moveQueue.RemoveAt(0);

            _currentAttack = (Attack)Instantiate(_attackPrefab, this.gameObject.transform.position, Quaternion.identity);
            _currentAttack.Execute(_fighterRef, this);
            _moveListened = false;
        }
        //else if(_moveQueue[0].IsJump)
        else
        {
            _movePrefab = _moveQueue[0];
            _moveQueue.RemoveAt(0);
            _movePrefab.Execute(_fighterRef, this);
            _moveListened = false;
        }
    }

    public void AddMove(Move theMove)
    {
        Move performingMove = theMove;

        if (gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            if(_currentAttack.NextMoveListen && !_moveListened)
            {
                _moveQueue.Add(performingMove);
                _currentAttack.NextMoveListen = false;
                _moveListened = true;
            }
        }
        else
        {
            _moveQueue.Add(performingMove);
        }
    }

    public void StartAttack(int attackRank = 0)
    {
        //attackRank 0 = normal attack
        //1 = Cast
        //2 = Soul

        Attack performingAttack = null;
        //if attacking, check attack's list of chains
        if (gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            //Debug.Log("checking list of chains: " + _currentAttack.NextMoveListen);
            //Check if attack is listening, if so, add move to queue
            if(_currentAttack.NextMoveListen && !_moveListened)
            {
               //Debug.Log("checking next move linkers");
                performingAttack = _currentAttack.CheckLinkers(attackRank);
                _currentAttack.NextMoveListen = false;
                _moveListened = true;
            }
        }
        //Not attacking so check list of opening attacks and chose best match
        else
        {
            //Debug.Log("checking list of opening");
            foreach (Attack theAttack in _openingAttacks)
            {
                if (theAttack.ConditionsMet(_fighterRef, attackRank))
                {
                    performingAttack = theAttack;
                    //break is temporary, if more than one attack meets conditions, choose by priority
                    break;
                }
            }
        }

        if(performingAttack != null)
        {
            _moveQueue.Add(performingAttack);
            gameObject.GetComponent<Fighter>().Attacking = true;
        }
        //if(_currentAttack != null)
            //Debug.Log("nextMoveListen after change :" + _currentAttack.NextMoveListen);
        //Debug.Log("attackQueue length: " + _moveQueue.Count);
    }



    public void AttackFinsihed()
    {
        //Enable fighter collisions

        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
        GameObject.Destroy(_currentAttack.gameObject);
        if(_currentAttack)
            this.gameObject.transform.position = _currentAttack.transform.position;
        _attackPrefab = null;
        _currentAttack = null;
        _fighterRef.Attacking = false;
        _fighterRef.activateAnimator();
    }

	void Update () 
    {
        
	}
}
