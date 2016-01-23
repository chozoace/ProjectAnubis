using UnityEngine;
using System.Collections;

public class PlayerDash : Move
{
    int _dashHash;
    int _endDashHash;
    float _dashSpeed;
    public float DashSpeed { get { return _dashSpeed; } set { _dashSpeed = value; } }
    Fighter _fighterRef;

	// Use this for initialization
	void Start () 
    {
        _moveName = "Player Dash";
        _dashHash = Animator.StringToHash("Dash");
        _endDashHash = Animator.StringToHash("EndDash");
	}

    public override void Execute(Fighter fighterRef, PlayerCombatScript combatScript)
    {
        _fighterRef = fighterRef;
        fighterRef.GetComponent<Animator>().SetTrigger(_dashHash);
        PlayerControllerScript playerController = fighterRef.GetComponent<PlayerControllerScript>();

        playerController.CurrentXSpeed = ((playerController.MaxSpeed + 3) * playerController.getXDirection);

        if (playerController.CurrentXSpeed > 0)
            _fighterRef.FacingRight = true;
        else if (playerController.CurrentXSpeed < 0)
            _fighterRef.FacingRight = false;

        StartCoroutine(EndDash());
    }

    public void ForceEndDash()
    {
        StopAllCoroutines();
        if (_fighterRef.Grounded)
            _fighterRef.GetComponent<Animator>().SetTrigger(_endDashHash);
        _fighterRef._dashing = false;
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(.5f);
        if(_fighterRef.Grounded)
            _fighterRef.GetComponent<Animator>().SetTrigger(_endDashHash);
        _fighterRef._dashing = false;
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
