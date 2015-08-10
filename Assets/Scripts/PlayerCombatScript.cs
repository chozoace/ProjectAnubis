using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Debug.Log("combat script start");
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
                    Debug.Log("Jump entering execution");
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
        Debug.Log("in execute move with " + _moveQueue[0].name);
        if (_moveQueue[0].IsAttack)
        {
            _attackPrefab = (Attack)_moveQueue[0];
            _moveQueue.RemoveAt(0);

            _currentAttack = (Attack)Instantiate(_attackPrefab, this.gameObject.transform.position, Quaternion.identity);
            _currentAttack.Execute(_fighterRef, this);
            _moveListened = false;
        }
        else if(_moveQueue[0].IsJump)
        {
            Debug.Log("Inside of execute move if statement");
            _movePrefab = _moveQueue[0];
            _moveQueue.RemoveAt(0);
            _movePrefab.Execute(_fighterRef, this);
            _moveListened = false;
        }
    }

    public void AddJump(Move theMove)
    {
        Move performingMove = theMove;

        Debug.Log(performingMove);

        if (gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            if(_currentAttack.NextMoveListen && !_moveListened)
            {
                Debug.Log("jump cancelled jump");
                _moveQueue.Add(performingMove);
                _currentAttack.NextMoveListen = false;
                _moveListened = true;
            }
        }
        else
        {
            Debug.Log("Non jump cancelled jump ");
            _moveQueue.Add(performingMove);
        }
    }

    public void StartAttack()
    {
        Debug.Log("In StartAttack");
        Attack performingAttack = null;
        //attacking, check attack's list of chains
        if (gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            //Debug.Log("checking list of chains: " + _currentAttack.NextMoveListen);
            //Check if attack is listening, if so, add move to queue
            if(_currentAttack.NextMoveListen && !_moveListened)
            {
               //Debug.Log("checking next move linkers");
                performingAttack = _currentAttack.CheckLinkers();
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
                if(theAttack.ConditionsMet(_fighterRef))
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
        //Debug.Log("Attack End");
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
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
