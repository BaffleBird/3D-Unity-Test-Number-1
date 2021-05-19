using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : StateMachine
{
	protected override void Awake()
	{
		base.Awake();

		States.Add("Idle", new Player_IdleState("Idle", this));
		States.Add("Move", new Player_MoveState("Move", this));
		States.Add("Jump", new Player_JumpState("Jump", this)); myStatus.SetCooldown("Jump", 0);
		States.Add("Dodge", new Player_DodgeState("Dodge", this)); myStatus.SetCooldown("Dodge", 0);

		currentState = States["Idle"];
		_previousState = currentState.StateName;
		currentState.StartState();

		myStatus.isGrounded = true;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		GroundCheck();
		TextUpdate.Instance.setText(myStatus.groundSlope.ToString());
	}
}
