using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour 
{
    [SerializeField] string _attackName = "Default";
    [SerializeField] float _hitboxWidth;
    [SerializeField] float _hitboxHeight;
    [SerializeField] float _translationDistance;
    [SerializeField] string _animationName;
    [SerializeField] int _priority;
    [SerializeField] bool _activeHitbox = false;
    [SerializeField] bool _translate;
    [SerializeField] bool _nextMoveListen;
    [SerializeField] bool _nextMoveExecute;
    [SerializeField] bool _movementCancel;
    [SerializeField] bool _moveFinished = false;
    int _currentFrame;

    [SerializeField] List<Attack> _linkerList;

    enum State{ Idle, Walking, Airbourne };
    [SerializeField] State _requiredState = State.Idle;
    FighterState _fighterRef;
    PlayerCombatScript _combatScript;
    Animator _anim;
    //special cancel
	
	void Awake () 
    {
        Debug.Log("In attack Awake");
        _anim = gameObject.GetComponent<Animator>();
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}

    public void Execute(FighterState fighterRef, PlayerCombatScript combatScript)
    {
        PlayerControllerScript.Instance().disableAnimator();
        if (!PlayerControllerScript.Instance().FacingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Attack Execute: " + _attackName);
        _fighterRef = fighterRef;
        _combatScript = combatScript;
    }

    public void FixedUpdate()
    {
        Debug.Log("fixed update");
        Debug.Log("Move Finished " + _moveFinished);
    }

    public bool ConditionsMet(FighterState theFighter)
    {
        return true;
    }

	public void Update () 
    {
        Debug.Log("Update");
        if (_moveFinished)
        {
            Debug.Log("attack finsihed");
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _combatScript.AttackFinsihed();
        }
        //NOT NEEDED
	    //after _startupTime amount of time has passed, activate move hitbox
        //after _active amount of time has passed, remove move hitbox
        //after _recovery amount of time has passed, exit attack state
	}
}
