using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour 
{
    [SerializeField] float _startUp;
    [SerializeField] float _active;
    [SerializeField] float _recovery;
    [SerializeField] float _hitboxWidth;
    [SerializeField] float _hitboxHeight;
    [SerializeField] float _translationDistance;
    [SerializeField] float _translationTime;
    [SerializeField] float _nextMoveListen;
    [SerializeField] float _nextMoveExecute;
    [SerializeField] float _movementCancel;
    [SerializeField] string _animationName;
    //special cancel
	
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
