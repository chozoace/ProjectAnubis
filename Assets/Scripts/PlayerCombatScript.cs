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

        }
    }

    public void StartAttack()
    {
        Attack performingAttack = null;
        //attacking, check attack's list of chains
        if (gameObject.GetComponent<FighterState>().Attacking)
        {

        }
        //Not attacking so check list of opening attacks and chose best match
        else
        {
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
    }

    public void DoMove(Attack theAttack)
    {
        //Will this instantiate the prefab? take in a parameter?
    }

	void Update () 
    {
	    
	}
}
