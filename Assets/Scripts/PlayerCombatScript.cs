using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombatScript : MonoBehaviour 
{
    [SerializeField] List<GameObject> _openingAttacks;
    List<Attack> attackQueue;

	void Start () 
    {
	    
	}

    public void StartAttack()
    {
        //looks at list of moves and chooses best match then calls DoMove
    }

    public void DoMove(GameObject theAttackPrefab)
    {
        //Will this instantiate the prefab? take in a parameter?
    }
	
    void FixedUpdate()
    {

    }

	void Update () 
    {
	    
	}
}
