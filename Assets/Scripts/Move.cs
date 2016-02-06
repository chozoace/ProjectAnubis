using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour 
{
    [Header("General Move Info")]
    [SerializeField] protected string _moveName = "Default";
    public string MoveName { get { return _moveName; } }
    [SerializeField] protected List<string> _acceptedStates;
    [SerializeField] protected char _attackInput;
    [SerializeField] protected int _YdirectionInput = 0;
    [SerializeField] protected int _XdirectionInput = 0;
    public int _attackRank;
    [SerializeField] protected int _priority;
    [SerializeField] string _animationName = "Default";
    [Header("Attack Attribute Flags (For monitoring purposes only)")]
    [SerializeField] protected bool _nextMoveListen;
    public bool NextMoveListen { get { return _nextMoveListen; } set { _nextMoveListen = value; } }
    [SerializeField] protected bool _nextMoveExecute;
    public bool NextMoveExecute { get { return _nextMoveExecute; } }
    [SerializeField] protected bool _jumpCancel = false;
    public bool CanJumpCancel { get { return _jumpCancel; } }
    [SerializeField] protected bool _translate;
    [SerializeField] protected bool _movementCancel;
    [SerializeField] protected bool _moveFinished = false;
    [SerializeField] protected bool _isAttack = false;
    public bool IsAttack { get { return _isAttack; } }

    [SerializeField]protected float _translationDistance;
    protected bool _isJump = false;
    public bool IsJump { get { return _isJump; } }

	// Use this for initialization
	void Awake () 
    {
	    switch(_attackInput)
        {
            case 'X':
                _attackRank = 0;
                break;
            case 'Y':
                _attackRank = 1;
                break;
        }
	}
	
    public virtual void Execute(Fighter fighterRef, PlayerCombatScript combatScript)
    {

    }

    public virtual bool ConditionsMet(Fighter theFighter, int input, float xInput = 0, float yInput = 0)
    {
        string currentState = theFighter.GetPlayerState.ToString();
        //checks input too

        foreach (string acceptedState in _acceptedStates)
        {
            if (currentState == acceptedState && input == _attackRank)
            {
                if (yInput == _YdirectionInput && xInput == _XdirectionInput)
                    return true;                
            }
        }

        return false;
    }
}
