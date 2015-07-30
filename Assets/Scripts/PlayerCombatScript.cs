using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatScript : MonoBehaviour 
{
    [SerializeField] List<Attack> _openingAttacks;
    List<Attack> _attackQueue;
    [HideInInspector]public Attack _currentAttack;
    Attack _attackPrefab;
    bool _readyToExecute = true;
    bool _moveListened = false;

	void Start () 
    {
        Debug.Log("combat script start");
        _attackQueue = new List<Attack>();
	}

    void FixedUpdate()
    {
        //Executes attack from list when ready to fire
        if(gameObject.GetComponent<FighterState>().Attacking && _attackPrefab != null)
        {
            //DO I REALLY NEED AN ATTACKQUEUE OR JUST A REFERENCE FOR THE NEXT ATTACK?
            //check if move.canfire is true, if so execute move in list
            if(_attackPrefab.NextMoveExecute && _attackQueue.Count > 0)
            {
                _attackPrefab.EndAttack();

                _currentAttack = _attackQueue[0];
                _attackQueue.RemoveAt(0);

                Debug.Log("PLayer position at time of attack: " + PlayerControllerScript.Instance().transform.position);
                //gameObject.GetComponent<FighterState>().Attacking = true;
                _attackPrefab = (Attack)Instantiate(_currentAttack, PlayerControllerScript.Instance().transform.position, Quaternion.identity);
                _attackPrefab.Execute(gameObject.GetComponent<PlayerControllerScript>(), this);
                _moveListened = false;
            }
        }
        else
        {
            if (_attackQueue.Count > 0)
            {
                Debug.Log("Choosing attack from attack queue");
                _currentAttack = _attackQueue[0];
                _attackQueue.RemoveAt(0);

                Debug.Log("setting true");
                //gameObject.GetComponent<FighterState>().Attacking = true;
                _attackPrefab = (Attack)Instantiate(_currentAttack, PlayerControllerScript.Instance().transform.position, Quaternion.identity);
                _attackPrefab.Execute(gameObject.GetComponent<PlayerControllerScript>(), this);
                _moveListened = false;
            }
        }
    }

    public void StartAttack()
    {
        this.gameObject.GetComponent<PlayerControllerScript>().StopMovement();
        Debug.Log("In StartAttack");
        Attack performingAttack = null;
        //attacking, check attack's list of chains
        if (gameObject.GetComponent<FighterState>().Attacking && _attackPrefab != null)
        {
            Debug.Log("checking list of chains: " + _attackPrefab.NextMoveListen);
            //Check if attack is listening, if so, add move to queue
            if(_attackPrefab.NextMoveListen && !_moveListened)
            {
                Debug.Log("checking next move linkers");
                performingAttack = _attackPrefab.CheckLinkers();
                _attackPrefab.NextMoveListen = false;
                _moveListened = true;
            }
        }
        //Not attacking so check list of opening attacks and chose best match
        else
        {
            Debug.Log("checking list of opening");
            foreach (Attack theAttack in _openingAttacks)
            {
                if(theAttack.ConditionsMet(gameObject.GetComponent<PlayerControllerScript>()))
                {
                    performingAttack = theAttack;
                }
            }
        }

        if(performingAttack != null)
        {
            _attackQueue.Add(performingAttack);
            gameObject.GetComponent<FighterState>().Attacking = true;
        }
        if(_attackPrefab != null)
            Debug.Log("nextMoveListen after change :" + _attackPrefab.NextMoveListen);
        Debug.Log("attackQueue length: " + _attackQueue.Count);
    }

    public void AttackFinsihed()
    {
        Debug.Log("Attack End");
        PlayerControllerScript pc = gameObject.GetComponent<PlayerControllerScript>();
        pc.GetComponent<Rigidbody2D>().gravityScale = 2;
        pc.transform.position = _attackPrefab.transform.position;
        _currentAttack = null;
        _attackPrefab = null;
        pc.Attacking = false;
        pc.activateAnimator();
    }

    public void DoMove(Attack theAttack)
    {
        //Will this instantiate the prefab? take in a parameter?
    }

	void Update () 
    {
	    if(_currentAttack != null)
        {
            //_currentAttack.Update();
        }
	}
}
