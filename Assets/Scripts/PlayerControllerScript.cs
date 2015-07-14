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
    
	void Start () 
    {
	    //Testing
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
        gameObject.GetComponent<Rigidbody2D>().velocity = v;
        
        if(inputDevice.Action1.WasPressed && _grounded)
        {
            Jump();
        }
        else if(inputDevice.Action3.WasPressed && _grounded)
        {
            //What will do move do? 
            gameObject.GetComponent<PlayerCombatScript>().DoMove();
        }

	}

    void Jump()
    {
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.y = _jumpSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }
}
