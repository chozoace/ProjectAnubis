﻿using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerScript : FighterState
{
    [SerializeField] float _maxSpeed = 30;
    [SerializeField] float _jumpSpeed = 3;
    [SerializeField] Transform _groundCheck;
    float _groundRadius = .2f;
    [SerializeField] LayerMask _whatIsGround;
    float _currentXSpeed;

    bool _facingRight = true;
    public bool FacingRight { get { return _facingRight; } }

    public enum PlayerState { Idle, Airbourne, Walking };
    public PlayerState _currentState = PlayerState.Idle;
    public PlayerState GetPlayerState { get { return _currentState; } }

    Animator anim;
    int jumpHash = Animator.StringToHash("Jump");
    static PlayerControllerScript _instance;

    public static PlayerControllerScript Instance()
    {
        return _instance;
    }

	void Start () 
    {
	    //Testing
        anim = GetComponent<Animator>();
        _instance = this;
	}

    void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _whatIsGround);
    }

	void Update () 
    {
        var inputDevice = InputManager.ActiveDevice;
        if (!_attacking)
        {
            if (!_grounded)
                _currentState = PlayerState.Airbourne;
            else if (_grounded && gameObject.GetComponent<Rigidbody2D>().velocity.x == 0)
                _currentState = PlayerState.Idle;
            else
                _currentState = PlayerState.Walking;

            _xDirection = inputDevice.LeftStickX.Value;
            _yDirection = inputDevice.LeftStickY.Value;

            //X Movement
            _currentXSpeed = (_maxSpeed * inputDevice.LeftStickX.Value);
            Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
            v.x = _currentXSpeed;

            //Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.x);

            anim.SetFloat("XSpeed", Mathf.Abs(_currentXSpeed));
            anim.SetFloat("YSpeed", v.y);
            anim.SetBool("Grounded", _grounded);

            gameObject.GetComponent<Rigidbody2D>().velocity = v;
            //Update facing var
            if (inputDevice.LeftStickX.Value > 0)
                _facingRight = true;
            else if (inputDevice.LeftStickX.Value < 0)
                _facingRight = false;
            //turn player around according to direction
            if (!_facingRight)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

            if (inputDevice.Action1.WasPressed && _grounded)
            {
                Jump();
            }
        }
        //Also check if listening for inputs
        if (inputDevice.Action3.WasPressed)
        {
            //What will do move do? 
            //CHECK FOR JUMP HEIGHT, ONLY TRIGGER AT MID-HIGH HEIGHT
            Debug.Log("position before attack: " + transform.position);
            StopMovement();
            gameObject.GetComponent<PlayerCombatScript>().StartAttack();
        }

	}

    public void StopMovement()
    {
        _currentXSpeed = 0;
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.x = _currentXSpeed;
        v.y = 0;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }

    public void disableAnimator()
    {
        anim.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void activateAnimator()
    {
        anim.enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    void Jump()
    {
        anim.SetTrigger(jumpHash);
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.y = _jumpSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }
}
