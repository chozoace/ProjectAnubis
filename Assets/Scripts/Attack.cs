using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] int _priority;
    int _currentFrame;

    [SerializeField] List<Attack> _linkerList;

    enum State{ Idle, Walking, Airbourne };
    [SerializeField] State _requiredState = State.Idle;
    //special cancel
	
	void Start () 
    {
	    //Start timer here
	}

    public bool ConditionsMet(FighterState theFighter)
    {
        return false;
    }
	
	void Update () 
    {
	    //after _startupTime amount of time has passed, activate move hitbox
        //after _active amount of time has passed, remove move hitbox
        //after _recovery amount of time has passed, exit attack state
	}
}
