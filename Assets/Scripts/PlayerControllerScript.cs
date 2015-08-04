using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] float _maxSpeed = 30;
    [SerializeField] float _jumpSpeed = 3;
    [SerializeField] Transform _groundCheck;
    float _groundRadius = .2f;
    [SerializeField] LayerMask _whatIsGround;
    float _currentXSpeed;

    Animator _anim;
    PlayerCombatScript _combatScript;
    int _jumpHash = Animator.StringToHash("Jump");
    static PlayerControllerScript _instance;
    Fighter _fighterRef;

    public static PlayerControllerScript Instance()
    {
        return _instance;
    }

	void Start () 
    {
        _anim = GetComponent<Animator>();
        _combatScript = GetComponent<PlayerCombatScript>();
        _fighterRef = this.gameObject.GetComponent<Fighter>();
        _instance = this;
	}

    void FixedUpdate()
    {
        _fighterRef.Grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _whatIsGround);
    }

	void Update () 
    {
        var inputDevice = InputManager.ActiveDevice;
        if (!_fighterRef.Attacking)
        {
            //What is held direction
            _fighterRef.XDirection = inputDevice.LeftStickX.Value;
            _fighterRef.YDirection = inputDevice.LeftStickY.Value;

            //X Movement
            _currentXSpeed = (_maxSpeed * inputDevice.LeftStickX.Value);
            Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
            v.x = _currentXSpeed;

            _anim.SetFloat("XSpeed", Mathf.Abs(_currentXSpeed));
            _anim.SetFloat("YSpeed", v.y);
            _anim.SetBool("Grounded", _fighterRef.Grounded);

            gameObject.GetComponent<Rigidbody2D>().velocity = v;

            //Update facing var
            if (inputDevice.LeftStickX.Value > 0)
                _fighterRef.FacingRight = true;
            else if (inputDevice.LeftStickX.Value < 0)
                _fighterRef.FacingRight = false;

            //turn player around according to direction
            if (!_fighterRef.FacingRight)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);

            if (inputDevice.Action1.WasPressed && _fighterRef.Grounded)
                Jump();
        }
        if (inputDevice.Action3.WasPressed)
        {
            //CHECK FOR JUMP HEIGHT, ONLY TRIGGER AT MID-HIGH HEIGHT(when speed is 0 or greater)
            Debug.Log("position before attack: " + transform.position);
            //StopMovement();
            _combatScript.StartAttack();
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

    void Jump()
    {
        _anim.SetTrigger(_jumpHash);
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.y = _jumpSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }
}
