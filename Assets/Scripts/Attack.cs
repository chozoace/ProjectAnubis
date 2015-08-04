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
    public bool NextMoveListen { get { return _nextMoveListen; } set { _nextMoveListen = value; } }
    [SerializeField] bool _nextMoveExecute;
    public bool NextMoveExecute { get { return _nextMoveExecute; } }
    [SerializeField] bool _movementCancel;
    [SerializeField] bool _moveFinished = false;
    [SerializeField] bool _airAttack;

    [SerializeField] List<Attack> _linkerList;

    [SerializeField] List<string> _acceptedStates;
    Fighter _fighterRef;
    PlayerCombatScript _combatScript;
    Animator _anim;

    bool _linkerCancel = false;
	
	void Awake () 
    {
        Debug.Log("In attack Awake");
        _anim = gameObject.GetComponent<Animator>();
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}

    public Attack CheckLinkers()
    {
        Debug.Log("In check linkers");
        Attack result = null;
        foreach(Attack linker in _linkerList)
        {
            if(linker.ConditionsMet(_fighterRef))
            {
                result = linker;
            }
        }
        if(result != null)
        {
            this._linkerCancel = true;
        }
        Debug.Log("CHANGED LINKER CANCEL TO: " + _linkerCancel + " of " + _attackName);
        return result;
    }

    public void Execute(Fighter fighterRef, PlayerCombatScript combatScript)
    {
        _fighterRef = fighterRef;
        _fighterRef.disableAnimator();

        if (_airAttack)
        {
            //If air attack, dont stop momentum, but pause it on hit.
            //Create on hit function
            _fighterRef.GetComponent<Rigidbody2D>().gravityScale = 0;
            //this.gameObject.GetComponent<Rigidbody2D>().gravityScale = _fighterRef.GetComponent<Rigidbody2D>().gravityScale;
            //this.gameObject.GetComponent<Rigidbody2D>().velocity = _fighterRef.GetComponent<Rigidbody2D>().velocity;
        }

        if (!_fighterRef.FacingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Attack Execute: " + _attackName);
        _fighterRef = fighterRef;
        _combatScript = combatScript;
    }

    public bool ConditionsMet(Fighter theFighter)
    {
        string currentState = theFighter.GetPlayerState.ToString();

        foreach(string acceptedState in _acceptedStates)
        {
            if (currentState == acceptedState)
                return true;
        }

        return false;
    }

	public void Update () 
    {
        if (_moveFinished && !_linkerCancel)
        {
            EndAttack();
        }
	}

    public void EndAttack()
    {
        Debug.Log("attack finsihed");
        Debug.Log("LINKER CANCEL: " + _linkerCancel + " of " + _attackName);
        if(!_linkerCancel)
            _combatScript.AttackFinsihed();
        GameObject.Destroy(this.gameObject);
    }
}
