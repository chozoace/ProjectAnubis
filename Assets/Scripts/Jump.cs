using UnityEngine;
using System.Collections;

public class Jump : Move
{
    int _jumpHash;
    int _jumpSpeed = 7;

	// Use this for initialization
	void Start () 
    {
        _jumpHash = Animator.StringToHash("Jump");
        _isJump = true;
	}

    public override void Execute(Fighter fighterRef, PlayerCombatScript combatScript)
    {
           fighterRef.GetComponent<Animator>().SetTrigger(_jumpHash);
           Vector2 v = gameObject.GetComponent<Rigidbody2D>().velocity;
           v.y = _jumpSpeed;
           fighterRef.gameObject.GetComponent<Rigidbody2D>().velocity = v; 
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
