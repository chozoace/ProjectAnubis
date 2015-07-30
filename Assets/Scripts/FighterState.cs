using UnityEngine;
using System.Collections;

public class FighterState : MonoBehaviour 
{
    protected bool _attacking = false;
    public bool Attacking { get { return _attacking; } set { _attacking = value; } }
    protected bool _grounded = false;
    public bool Grounded { get { return _grounded; } }
    protected float _xDirection;
    protected float _yDirection;

	void Start () 
    {
	    
	}

	void Update () 
    {
	
	}
}
