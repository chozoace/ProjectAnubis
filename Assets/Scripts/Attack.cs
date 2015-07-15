using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour 
{
    float _startUp;
    float _active;
    float _recovery;
    //dash cancel
    //move cancel
    //special cancel
    //next move listen
    //next move fire
    //tech move listen
    //tech move fire
    //translation active, when during the attack am I being translated
	
	void Start () 
    {
	    //Start timer here
	}
	
	void Update () 
    {
	    //after _startupTime amount of time has passed, activate move hitbox
        //after _active amount of time has passed, remove move hitbox
        //after _recovery amount of time has passed, exit attack state
	}
}
