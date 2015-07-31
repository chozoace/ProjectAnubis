using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour 
{
    bool _attacking = false;
    bool _grounded = false;
    float _xDirection;
    float _yDirection;
    public bool Attacking { get { return _attacking; } set { _attacking = value; } }
    public bool Grounded { get { return _grounded; } set { _grounded = value; } }
    public float XDirection { get { return _xDirection; } set { _xDirection = value; } }
    public float YDirection { get { return _yDirection; } set { _yDirection = value; } }

    public enum PlayerState { Idle, Airbourne, Walking };
    PlayerState _currentState = PlayerState.Idle;
    public PlayerState GetPlayerState { get { return _currentState; } }

    bool _facingRight = true;
    public bool FacingRight { get { return _facingRight; } set { _facingRight = value; } }

    Animator _anim;

	void Start () 
    {
        _anim = GetComponent<Animator>();
	}

	void Update () 
    {
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
