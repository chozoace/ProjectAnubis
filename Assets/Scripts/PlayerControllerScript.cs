using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField] float _maxSpeed = 30;
    [SerializeField] float _jumpSpeed = 3;
    float _currentXSpeed;
    float keyboardXDir = 0;
    int _recordedDashDirection = 1;

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
	}

	void Update () 
    {
        var inputDevice = InputManager.ActiveDevice;
        if (!_fighterRef.Attacking)
        {
            //What is held direction
            int xDirection;
            if (inputDevice.LeftStickX.Value > 0)
                xDirection = 1;
            else if (inputDevice.LeftStickX.Value < 0)
                xDirection = -1;
            else
                xDirection = 0;

            _fighterRef.XDirection = xDirection;
            _fighterRef.YDirection = xDirection;

            //X Movement
            if (!inputDevice.Name.Equals("None"))
            {
                //Dash Check
                if(_currentXSpeed == 0 && xDirection != 0)
                {
                    if (!_fighterRef._executeDash)
                        StartCoroutine(CheckDash(xDirection));
                    else if(xDirection == _recordedDashDirection)
                    {
                        Dash(xDirection);
                        _fighterRef._executeDash = false;
                    }
                    else if(xDirection != _recordedDashDirection)
                    {
                        _fighterRef._executeDash = false;
                    }
                }

                //Regular movement
                if (_fighterRef._dashing != true)
                    _currentXSpeed = (_maxSpeed * xDirection);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.D))
                    keyboardXDir = 1;
                else if (Input.GetKeyDown(KeyCode.A))
                    keyboardXDir = -1;
                if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
                    keyboardXDir = 0;

                _currentXSpeed = (_maxSpeed * keyboardXDir);
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
                if (_fighterRef._dashing == false)
                {
                    if (inputDevice.LeftStickX.Value > 0)
                        _fighterRef.FacingRight = true;
                    else if (inputDevice.LeftStickX.Value < 0)
                        _fighterRef.FacingRight = false;
                }
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
                if(gameObject.GetComponent<Rigidbody2D>().velocity.y < _jumpSpeed - 1)
                    _combatScript.StartAttack(0);
            }
            else if(inputDevice.Action4.WasPressed)
            {
                //Y Button Press
                //if (gameObject.GetComponent<Rigidbody2D>().velocity.y < _jumpSpeed - 1)
                if (gameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
                    _combatScript.StartAttack(1);
            }
            if (inputDevice.Action1.WasPressed && _fighterRef.Grounded)
            {
                _combatScript.AddJump(_playerJump);
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (gameObject.GetComponent<Rigidbody2D>().velocity.y < _jumpSpeed - 1)
                    _combatScript.StartAttack();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
                _combatScript.AddJump(_playerJump);
        }

	}

    void Dash(int xDirection)
    {
        _fighterRef._dashing = true;
        _currentXSpeed = ((_maxSpeed + 5) * xDirection);

        if (_currentXSpeed > 0)
            _fighterRef.FacingRight = true;
        else if (_currentXSpeed < 0)
            _fighterRef.FacingRight = false;

        StartCoroutine(EndDash());
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(.4f);
        _fighterRef._dashing = false;
    }

    IEnumerator CheckDash(int dashDirection)
    {
        if (!_fighterRef._executeDash)
        {
            _recordedDashDirection = dashDirection;
            _fighterRef._executeDash = true;
        }
        yield return new WaitForSeconds(.2f);
        _fighterRef._executeDash = false;
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
