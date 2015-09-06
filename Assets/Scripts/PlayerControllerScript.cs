using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] float _maxSpeed = 30;
    [SerializeField] float _jumpSpeed = 3;
    float _currentXSpeed;

    Animator _anim;
    PlayerCombatScript _combatScript;
    int _jumpHash = Animator.StringToHash("Jump");
    static PlayerControllerScript _instance;
    Fighter _fighterRef;
    Jump _playerJump;

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
        _playerJump = this.gameObject.AddComponent<Jump>();
        _playerJump.JumpSpeed = _jumpSpeed;
        Debug.Log("_playerjump is " + _playerJump);
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
            if (!inputDevice.Name.Equals("None"))
                _currentXSpeed = (_maxSpeed * inputDevice.LeftStickX.Value);
            else
            {
                if (Input.GetKeyDown(KeyCode.D))
                    _currentXSpeed = _maxSpeed;
                else if (Input.GetKeyDown(KeyCode.A))
                    _currentXSpeed = -_maxSpeed;
                if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
                    _currentXSpeed = 0;
            }

            Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
            v.x = _currentXSpeed;

            _anim.SetFloat("XSpeed", Mathf.Abs(_currentXSpeed));
            _anim.SetFloat("YSpeed", v.y);
            _anim.SetBool("Grounded", _fighterRef.Grounded);

            gameObject.GetComponent<Rigidbody2D>().velocity = v;

            //Update facing var
            if (!inputDevice.Name.Equals("None"))
            {
                if (inputDevice.LeftStickX.Value > 0)
                    _fighterRef.FacingRight = true;
                else if (inputDevice.LeftStickX.Value < 0)
                    _fighterRef.FacingRight = false;
            }
            else
            {
                if (_currentXSpeed > 0)
                    _fighterRef.FacingRight = true;
                else if (_currentXSpeed < 0)
                    _fighterRef.FacingRight = false;
            }

            //turn player around according to direction
            if (!_fighterRef.FacingRight)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        if (!inputDevice.Name.Equals("None"))
        {
            if (inputDevice.Action3.WasPressed)
            {
                //CHECK FOR JUMP HEIGHT, ONLY TRIGGER AT MID-HIGH HEIGHT(when speed is 0 or greater)
                //Debug.Log("position before attack: " + transform.position);
                //StopMovement();
                if(gameObject.GetComponent<Rigidbody2D>().velocity.y < _jumpSpeed - 1.8)
                    _combatScript.StartAttack();
            }
            if (inputDevice.Action1.WasPressed && _fighterRef.Grounded)
            {
                Debug.Log("player controller calling add jump with " + _playerJump);
                _combatScript.AddJump(_playerJump);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
                _combatScript.StartAttack();
            else if (Input.GetKeyDown(KeyCode.Space))
                _combatScript.AddJump(_playerJump);
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
        if (_fighterRef.Grounded)
        {
            _anim.SetTrigger(_jumpHash);
            Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
            v.y = _jumpSpeed;
            gameObject.GetComponent<Rigidbody2D>().velocity = v;
        }
        else
        {
            
        }
    }
}
