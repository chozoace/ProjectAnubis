using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
    Fighter _fighterRef;
    Animator _anim;
    PlayerCombatScript _combatScript;
    State _currentState = null;
    string _previousState = "";

    Rigidbody2D _myRigidbody;

	void Start () 
    {
        _anim = GetComponent<Animator>();
        _fighterRef = this.gameObject.GetComponent<Fighter>();
        _myRigidbody = GetComponent<Rigidbody2D>();
        _currentState = new EnemyPatrolState(this);
        _currentState.Enter();
        Debug.Log("enemy starting at state: " + _currentState.StateName);
	}

    public void ChangeState(State newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

	void Update ()
    {
        //_anim.SetFloat("XSpeed", Mathf.Abs(_myRigidbody.velocity.x));
        //_anim.SetFloat("YSpeed", _myRigidbody.velocity.y);
        _anim.SetBool("Hitstunned", _fighterRef.IsHitstunned);
        _anim.SetBool("Grounded", _fighterRef.Grounded);
        
        //UpdateState
        _currentState.UpdateState();
	}
}
