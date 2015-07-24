using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatScript : MonoBehaviour 
{
    [SerializeField] List<Attack> _openingAttacks;
    List<Attack> _attackQueue;
    Attack _currentAttack;
    bool _readyToExecute = true;

	void Start () 
    {
        _attackQueue = new List<Attack>();
	}

    void FixedUpdate()
    {
        //Executes attack from list when ready to fire
        if(gameObject.GetComponent<FighterState>().Attacking)
        {
            //DO I REALLY NEED AN ATTACKQUEUE OR JUST A REFERENCE FOR THE NEXT ATTACK?
        }
        else
        {
            if (_attackQueue.Count > 0)
            {
                Debug.Log("Choosing attack from attack queue");
                _currentAttack = _attackQueue[0];
                _attackQueue.RemoveAt(0);

                Debug.Log("setting true");
                gameObject.GetComponent<FighterState>().Attacking = true;
                _currentAttack.Execute(gameObject.GetComponent<FighterState>(), this);
            }
        }
    }

    public void StartAttack()
    {
        Debug.Log("In StartAttack");
        Attack performingAttack = null;
        //attacking, check attack's list of chains
        if (gameObject.GetComponent<FighterState>().Attacking)
        {
            Debug.Log("checking list of chains");
        }
        //Not attacking so check list of opening attacks and chose best match
        else
        {
            Debug.Log("checking list of opening");
            foreach (Attack theAttack in _openingAttacks)
            {
                if(theAttack.ConditionsMet(gameObject.GetComponent<FighterState>()))
                {
                    performingAttack = theAttack;
                }
            }
        }

        if(performingAttack != null)
        {
            _attackQueue.Add(performingAttack);
        }
        Debug.Log("attackQueue length: " + _attackQueue.Count);
    }

    public void AttackFinsihed()
    {
        gameObject.GetComponent<FighterState>().Attacking = false;
        _currentAttack = null;
        Debug.Log("Attack End: ");
    }

    public void DoMove(Attack theAttack)
    {
        //Will this instantiate the prefab? take in a parameter?
    }

	void Update () 
    {
	    
	}
}
