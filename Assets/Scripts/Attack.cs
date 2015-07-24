using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour 
{
    [SerializeField] string _attackName = "Default";
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
    FighterState _fighterRef;
    PlayerCombatScript _combatScript;
    //special cancel
	
	void Start () 
    {
	    
	}

    public void Execute(FighterState fighterRef, PlayerCombatScript combatScript)
    {
        //Start timer here
        Debug.Log("Attack Execute: " + _attackName);
        _fighterRef = fighterRef;
        _combatScript = combatScript;
        _combatScript.AttackFinsihed();
    }

    public bool ConditionsMet(FighterState theFighter)
    {
        return true;
    }

	void Update () 
    {
	    //after _startupTime amount of time has passed, activate move hitbox
        //after _active amount of time has passed, remove move hitbox
        //after _recovery amount of time has passed, exit attack state
	}
}
