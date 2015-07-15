using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerScript : MonoBehaviour 
{
    [SerializeField] float _maxSpeed = 30;
    [SerializeField] float _jumpSpeed = 3;
    [SerializeField] Transform _groundCheck;
    float _groundRadius = .32f;
    [SerializeField] LayerMask _whatIsGround;
    float _currentXSpeed;

    bool _grounded = false;
    bool _attacking = false;
    bool _facingRight = true;

    Animator anim;
    int jumpHash = Animator.StringToHash("Jump");
    
	void Start () 
    {
	    //Testing
        anim = GetComponent<Animator>();
	}

    void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _whatIsGround);
    }

	void Update () 
    {
        var inputDevice = InputManager.ActiveDevice;

        //X Movement
        _currentXSpeed = (_maxSpeed * inputDevice.LeftStickX.Value);
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.x = _currentXSpeed;

        Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.x);

        anim.SetFloat("XSpeed", Mathf.Abs(_currentXSpeed));
        anim.SetFloat("YSpeed", v.y);
        anim.SetBool("Grounded", _grounded);

        gameObject.GetComponent<Rigidbody2D>().velocity = v;
        //Update facing var
        if (inputDevice.LeftStickX.Value > 0)
            _facingRight = true;
        else if(inputDevice.LeftStickX.Value < 0)
            _facingRight = false;
        //turn player around according to direction
        if (!_facingRight)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        //Debug.Log(_facingRight);

        if(inputDevice.Action1.WasPressed && _grounded)
        {
            Jump();
        }
        else if(inputDevice.Action3.WasPressed && _grounded)
        {
            //What will do move do? 
            gameObject.GetComponent<PlayerCombatScript>().StartAttack();
        }

	}

    void Jump()
    {
        anim.SetTrigger(jumpHash);
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.y = _jumpSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }
}
