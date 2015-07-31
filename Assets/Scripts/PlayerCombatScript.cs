using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatScript : MonoBehaviour 
{
    [SerializeField] List<Attack> _openingAttacks;
    List<Attack> _attackQueue;
    //Used to instantiate attack
    Attack _attackPrefab;
    //the instance of the attack
    Attack _currentAttack;
    Fighter _fighterRef;

    bool _moveListened = false;

	void Start () 
    {
        Debug.Log("combat script start");
        _attackQueue = new List<Attack>();
        _fighterRef = this.gameObject.GetComponent<Fighter>();
	}

    void FixedUpdate()
    {
        //Executes attack from list when ready to fire
        //If attacking
        if(gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            //check if move.canfire is true, if so execute move in list
            if(_currentAttack.NextMoveExecute && _attackQueue.Count > 0)
            {
                _currentAttack.EndAttack();
                ExecuteAttack();
            }
        }
        else
        {
            if (_attackQueue.Count > 0)
            {
                ExecuteAttack();
            }
        }
    }

    void ExecuteAttack()
    {
        //Chooses attack from queue and executes it
        Debug.Log("Choosing attack from attack queue");
        _attackPrefab = _attackQueue[0];
        _attackQueue.RemoveAt(0);

        _currentAttack = (Attack)Instantiate(_attackPrefab, this.gameObject.transform.position, Quaternion.identity);
        _currentAttack.Execute(_fighterRef, this);
        _moveListened = false;
    }

    public void StartAttack()
    {
        this.gameObject.GetComponent<PlayerControllerScript>().StopMovement();
        Debug.Log("In StartAttack");
        Attack performingAttack = null;
        //attacking, check attack's list of chains
        if (gameObject.GetComponent<Fighter>().Attacking && _currentAttack != null)
        {
            Debug.Log("checking list of chains: " + _currentAttack.NextMoveListen);
            //Check if attack is listening, if so, add move to queue
            if(_currentAttack.NextMoveListen && !_moveListened)
            {
                Debug.Log("checking next move linkers");
                performingAttack = _currentAttack.CheckLinkers();
                _currentAttack.NextMoveListen = false;
                _moveListened = true;
            }
        }
        //Not attacking so check list of opening attacks and chose best match
        else
        {
            Debug.Log("checking list of opening");
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
            _attackQueue.Add(performingAttack);
            gameObject.GetComponent<Fighter>().Attacking = true;
        }
        if(_currentAttack != null)
            Debug.Log("nextMoveListen after change :" + _currentAttack.NextMoveListen);
        Debug.Log("attackQueue length: " + _attackQueue.Count);
    }

    public void AttackFinsihed()
    {
        Debug.Log("Attack End");
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
