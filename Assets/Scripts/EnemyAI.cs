using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
    Fighter _fighterRef;
    Animator _anim;
    PlayerCombatScript _combatScript;

	void Start () 
    {
        _anim = GetComponent<Animator>();
        _fighterRef = this.gameObject.GetComponent<Fighter>();
	}

	void Update ()
    {
        _anim.SetBool("Hitstunned", _fighterRef.IsHitstunned);
        _anim.SetBool("Grounded", _fighterRef.Grounded);
	}
}
