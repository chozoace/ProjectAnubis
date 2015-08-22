using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : Move 
{
    [SerializeField] bool _activeHitbox = false;
    [SerializeField] bool _airAttack;
    [SerializeField] int _damage;
    [SerializeField] float _hitstunTime;
    [SerializeField] float _xLaunchSpeed;
    [SerializeField] float _yLaunchSpeed;

    [SerializeField] List<Attack> _linkerList;

    Fighter _fighterRef;
    PlayerCombatScript _combatScript;
    Animator _anim;

    bool _hitboxCollided;
    bool _linkerCancel = false;
	
	void Awake () 
    {
        Debug.Log("In attack Awake");
        //doesnt set it to true fast enough? had to serialize field
        //_isAttack = true;
        _anim = gameObject.GetComponent<Animator>();
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _hitboxCollided = false;
	}

    void ExecuteAttack()
    {

    }

    public Attack CheckLinkers()
    {
        //Debug.Log("In check linkers");
        Attack result = null;
        foreach(Attack linker in _linkerList)
        {
            if (linker != null)
            {
                if (linker.ConditionsMet(_fighterRef))
                {
                    result = linker;
                }
            }
        }
        if(result != null)
        {
            this._linkerCancel = true;
        }
        //Debug.Log("CHANGED LINKER CANCEL TO: " + _linkerCancel + " of " + _attackName);
        return result;
    }

    //On collision Enter, Ignores fighterRef's hurtboxes
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_fighterRef.FacingRight && !_hitboxCollided)
        {
            _xLaunchSpeed = (float)(_xLaunchSpeed * -1.0f);
            _hitboxCollided = true;
        }
        Debug.Log("collision at: " + other.gameObject.name);
        if(other.gameObject.GetComponent<Fighter>() != null && other.gameObject.GetComponent<Fighter>() != _fighterRef)
        {
            Fighter enemyRef = other.gameObject.GetComponent<Fighter>();
            Debug.Log(other.gameObject.name + " is a fighter");

            //apply damage, apply hitstun time, apply translation
            enemyRef.EnterHitstun(_hitstunTime);
            enemyRef.ApplyAttackForce(new Vector2(_xLaunchSpeed, _yLaunchSpeed));
        }
    }

    public override void Execute(Fighter fighterRef, PlayerCombatScript combatScript)
    {
        Debug.Log("Attack Execute");
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        _fighterRef = fighterRef;
        _combatScript = combatScript;
        _fighterRef.disableAnimator();
        //turn off collisions for fighter ref

        Rigidbody2D body = this.gameObject.GetComponent<Rigidbody2D>();

        if (_airAttack)
        {
            //If air attack, dont stop momentum, but pause it on hit.
            //Create on hit function
            body.gravityScale = _fighterRef.GetComponent<Rigidbody2D>().gravityScale;
            Vector2 v = _fighterRef.GetComponent<Rigidbody2D>().velocity;
            body.velocity = v;
            Debug.Log("Fighter velocity: " + v);
            Debug.Log("Attack velocity: " + body.velocity);
        }
        _fighterRef.GetComponent<PlayerControllerScript>().StopMovement();

        if (!_fighterRef.FacingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        //Debug.Log("Attack Execute: " + _attackName);
    }

	void Update () 
    {
        if (_moveFinished && !_linkerCancel)
        {
            EndAttack();
        }
	}

    public void EndAttack()
    {
        //Debug.Log("attack finsihed");
        //Debug.Log("LINKER CANCEL: " + _linkerCancel + " of " + _attackName);
        if(!_linkerCancel)
            _combatScript.AttackFinsihed();
        GameObject.Destroy(this.gameObject);
    }
}
