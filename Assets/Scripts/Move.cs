using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour 
{
    [SerializeField] string _moveName = "Default";
    [SerializeField] protected List<string> _acceptedStates;
    [SerializeField] protected int _priority;
    [SerializeField] string _animationName = "Default";
    [SerializeField] protected bool _nextMoveListen;
    public bool NextMoveListen { get { return _nextMoveListen; } set { _nextMoveListen = value; } }
    [SerializeField] protected bool _nextMoveExecute;
    public bool NextMoveExecute { get { return _nextMoveExecute; } }
    [SerializeField] protected bool _jumpCancel = false;
    public bool CanJumpCancel { get { return _jumpCancel; } }
    [SerializeField] protected bool _translate;
    [SerializeField] protected float _translationDistance;
    [SerializeField] protected bool _movementCancel;
    [SerializeField] protected bool _moveFinished = false;

    [SerializeField] protected bool _isAttack = false;
    public bool IsAttack { get { return _isAttack; } }
    protected bool _isJump = false;
    public bool IsJump { get { return _isJump; } }

	// Use this for initialization
	void Start () 
    {
	
	}
	
    public virtual void Execute(Fighter fighterRef, PlayerCombatScript combatScript)
    {

    }

    public virtual bool ConditionsMet(Fighter theFighter)
    {
        string currentState = theFighter.GetPlayerState.ToString();

        foreach (string acceptedState in _acceptedStates)
        {
            if (currentState == acceptedState)
                return true;
        }

        return false;
    }
}
