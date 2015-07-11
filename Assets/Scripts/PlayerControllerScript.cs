using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerScript : MonoBehaviour 
{
    [SerializeField] float _maxSpeed = 30;
    [SerializeField] float _jumpSpeed = 3;
    float _currentXSpeed;
    
	void Start () 
    {
	    //Testing
	}
	
	void Update () 
    {
        var inputDevice = InputManager.ActiveDevice;

        //X Movement
        _currentXSpeed = (_maxSpeed * inputDevice.LeftStickX.Value);
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.x = _currentXSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = v;

        if(inputDevice.Action1.WasPressed)
        {
            Jump();
        }
	}

    void Jump()
    {
        Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
        v.y = _jumpSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = v;
    }
}
