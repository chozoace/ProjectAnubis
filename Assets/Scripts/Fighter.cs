using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour 
{
    bool _attacking = false;
    public bool _grounded = false;
    bool _inHitstunFreeze = false;
    public bool InHitstunFreeze { get { return _inHitstunFreeze; } set { _inHitstunFreeze = value; } }
    float _xDirection;
    float _yDirection;
    public bool Attacking { get { return _attacking; } set { _attacking = value; } }
    public bool Grounded { get { return _grounded; } set { _grounded = value; } }
    public float XDirection { get { return _xDirection; } set { _xDirection = value; } }
    public float YDirection { get { return _yDirection; } set { _yDirection = value; } }

    //Animation states
    public enum PlayerState { Idle, Airbourne, Walking, };
    PlayerState _currentState = PlayerState.Idle;
    public PlayerState GetPlayerState { get { return _currentState; } }
    //no movement or attacks are allowed during hitstunned
    bool _hitstunned;
    public bool IsHitstunned { get { return _hitstunned; } }
    public bool _groundBounceState = false;
    public bool GroundBounceState { get { return _groundBounceState; } set { _groundBounceState = value; } }

    bool _facingRight = true;
    public bool FacingRight { get { return _facingRight; } set { _facingRight = value; } }

    Animator _anim;
    [SerializeField] Sprite _redSprite;
    [SerializeField] Sprite _blueSprite;
    [SerializeField] Sprite _greenSprite;

    [SerializeField] protected Transform _groundCheck;
    protected float _groundRadius = .2f;
    [SerializeField] protected LayerMask _whatIsGround;

	void Start () 
    {
        _anim = GetComponent<Animator>();
        _hitstunned = false;
	}

    void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _whatIsGround);
        if(_grounded && _groundBounceState)
        {
            _groundBounceState = false;
            EnterGroundBounce();
            Debug.Log("Ground Bounce");
        }
    }

	void Update () 
    {
        if(_hitstunned)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = _blueSprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = _redSprite;
        }
        
        if (!Attacking)
        {
            if (!Grounded)
                _currentState = PlayerState.Airbourne;
            else if (Grounded && gameObject.GetComponent<Rigidbody2D>().velocity.x == 0)
                _currentState = PlayerState.Idle;
            else
                _currentState = PlayerState.Walking;
        }
	}

    public void EnterGroundBounce()
    {
        Vector2 groundBounceSpeed = new Vector2(0, 8);
        this.gameObject.GetComponent<Rigidbody2D>().velocity = groundBounceSpeed;
    }

    public void EnterHitstun(float hitstunTime)
    {
        StopAllCoroutines();
        _hitstunned = true;
        if(_attacking)
        {
            GetComponent<PlayerCombatScript>().AttackFinsihed();
        }

        StartCoroutine(ExitHitstun(hitstunTime));
    }

    public void EnterHitstun()
    {
        StopAllCoroutines();
        _hitstunned = true;
    }

    public void StartHitstunTimer(float timer)
    {
        StartCoroutine(ExitHitstun(timer));
    }

    public void ApplyAttackForce(Vector2 launchSpeed)
    {
        if(launchSpeed.x != 0 && launchSpeed.y != 0)
            this.gameObject.GetComponent<Rigidbody2D>().velocity = launchSpeed;
    }

    IEnumerator ExitHitstun(float hitstunTime)
    {
        yield return new WaitForSeconds(hitstunTime);
        //while(!_grounded)
        //{
            //wait until grounded before ending hitstun
        //}
        _hitstunned = false;
    }

    public void disableAnimator()
    {
        _anim.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void activateAnimator()
    {
        _anim.enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
