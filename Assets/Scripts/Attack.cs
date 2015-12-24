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
    [SerializeField] float _hitstunFreezeTime = 0;
    [SerializeField] float _slamSpeed = 0;

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

    public Attack CheckLinkers(int attackRank)
    {
        //Debug.Log("In check linkers");
        Attack result = null;
        foreach(Attack linker in _linkerList)
        {
            if (linker != null)
            {
                if (linker.ConditionsMet(_fighterRef, attackRank))
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
        if (other.offset.y >= 0)
        {
            if (!_fighterRef.FacingRight && !_hitboxCollided)
            {
                _xLaunchSpeed = (float)(_xLaunchSpeed * -1.0f);
                _hitboxCollided = true;
            }
            Debug.Log("collision at: " + other.gameObject.name);
            if (other.gameObject.GetComponent<Fighter>() != null && other.gameObject.GetComponent<Fighter>() != _fighterRef)
            {
                Fighter enemyRef = other.gameObject.GetComponent<Fighter>();
                Debug.Log(other.gameObject.name + " is a fighter");

                //apply damage, apply hitstun time, apply translation, or hit pause
                if (_hitstunFreezeTime > 0 && _fighterRef.InHitstunFreeze == false && enemyRef.Grounded == false)
                {
                    //SHOULD BE APPLIED FOR ALL ATTACKS NOT JUST AIR
                    StartCoroutine(AttackFreeze(enemyRef));
                }
                else
                {
                    if (_airAttack)
                    {
                        Debug.Log("enter regular");
                    }
                    //If already hitstun, reenter hitstun and play hit animation again
                    //Duration is determined by hitstun time;
                    enemyRef.EnterHitstun(_hitstunTime);
                    enemyRef.ApplyAttackForce(new Vector2(_xLaunchSpeed, _yLaunchSpeed));
                }
            }
        }
    }
    //Currently only does air attack freeze ADD ONE FOR GROUND
    IEnumerator AttackFreeze(Fighter enemyRef)
    {
        _fighterRef.InHitstunFreeze = true;
        enemyRef.InHitstunFreeze = true;

        Debug.Log("entered hit freeze");
        enemyRef.EnterHitstun();
        //Freeze both player and target for x time
        Vector2 thisVelocity = _fighterRef.GetComponent<Rigidbody2D>().velocity;
        Vector2 otherVelocity = enemyRef.GetComponent<Rigidbody2D>().velocity;
        thisVelocity.y += 3;
        if (_slamSpeed != 0)
        {
            otherVelocity.y = _slamSpeed;
            thisVelocity.y = 4;
            if (otherVelocity.x > 0)
                otherVelocity.x += 4;
            else
                otherVelocity.x -= 4;
        }
        else
        {
            otherVelocity.y += 4f;
        }
        float thisGravity = _fighterRef.GetComponent<Rigidbody2D>().gravityScale;
        float otherGravity = enemyRef.GetComponent<Rigidbody2D>().gravityScale;
        _anim.speed = 0;
        _fighterRef.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _fighterRef.GetComponent<Rigidbody2D>().gravityScale = 0;
        enemyRef.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        enemyRef.GetComponent<Rigidbody2D>().gravityScale = 0;

        yield return new WaitForSeconds(_hitstunFreezeTime);

        _anim.speed = 1;
        _fighterRef.GetComponent<Rigidbody2D>().velocity = thisVelocity;
        enemyRef.GetComponent<Rigidbody2D>().velocity = otherVelocity;
        _fighterRef.GetComponent<Rigidbody2D>().gravityScale = thisGravity;
        enemyRef.GetComponent<Rigidbody2D>().gravityScale = otherGravity;
        enemyRef.StartHitstunTimer(_hitstunTime);
        _fighterRef.InHitstunFreeze = false;
        enemyRef.InHitstunFreeze = false;
    }

    IEnumerator ExitAttackFreeze(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
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
        else if (_fighterRef.tag == "Player")
            _fighterRef.GetComponent<PlayerControllerScript>().StopMovement();
        else
        {

        }

        if (!_fighterRef.FacingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        //Debug.Log("Attack Execute: " + _attackName);
    }

	void Update () 
    {
        bool attackEndCalled = false;

        this.gameObject.transform.position = _fighterRef.transform.position;
        if (_airAttack)
            this.gameObject.GetComponent<Rigidbody2D>().velocity = _fighterRef.gameObject.GetComponent<Rigidbody2D>().velocity;
        if (_moveFinished && !_linkerCancel && !attackEndCalled)
        {
            attackEndCalled = true;
            EndAttack();
        }

        if(_airAttack && _fighterRef.Grounded && !attackEndCalled)
        {
            attackEndCalled = true;
            EndAttack();
        }
	}

    public void EndAttack()
    {
        //Play Landing animation for air attack

        //Debug.Log("attack finsihed");
        //Debug.Log("LINKER CANCEL: " + _linkerCancel + " of " + _attackName);
        if(!_linkerCancel)
            _combatScript.AttackFinsihed();
        GameObject.Destroy(this.gameObject);
    }
}
