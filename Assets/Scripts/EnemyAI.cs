using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
    Fighter _fighterRef;
    Animator _anim;
    PlayerCombatScript _combatScript;
    State _currentState = null;
    public State GetCurrentState { get { return _currentState; } }
    string _previousState = "";

    [SerializeField] float _maxSpeed = 2;
    public float MaxSpeed { get { return _maxSpeed; } }

    Rigidbody2D _myRigidbody;
    public bool _walkingReversed;

	void Start () 
    {
        _anim = GetComponent<Animator>();
        _fighterRef = this.gameObject.GetComponent<Fighter>();
        _myRigidbody = GetComponent<Rigidbody2D>();
        _currentState = new EnemyPatrolState(this);
        _currentState.Enter();

        GameController.Instance().GetEnemyList.Add(this.gameObject);
	}

    public void ChangeState(State newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

	void Update ()
    {
        _anim.SetFloat("XSpeed", Mathf.Abs(_myRigidbody.velocity.x));
        //_anim.SetFloat("YSpeed", _myRigidbody.velocity.y);
        _anim.SetBool("Hitstunned", _fighterRef.IsHitstunned);
        _anim.SetBool("Grounded", _fighterRef.Grounded);
        
        //UpdateState
        //Debug.Log(_currentState.StateName);

        if (GetComponent<Rigidbody2D>().velocity.x > 0)
            _fighterRef.FacingRight = true;
        else if (GetComponent<Rigidbody2D>().velocity.x < 0)
            _fighterRef.FacingRight = false;

        if (_walkingReversed && _fighterRef.FacingRight == true)
            _fighterRef.FacingRight = false;
        else if (_walkingReversed && _fighterRef.FacingRight == false)
            _fighterRef.FacingRight = true;

        if (!_fighterRef.FacingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

        _currentState.UpdateState();
	}
}
