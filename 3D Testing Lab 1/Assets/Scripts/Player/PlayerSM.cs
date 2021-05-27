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
		States.Add("Sprint", new Player_SprintState("Sprint", this));
		States.Add("Slide", new Player_SlideState("Slide", this));

		currentState = States["Idle"];
		_previousState = currentState.StateName;
		currentState.StartState();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (myCController.isGrounded)
		{
			myStatus.isStableGround = (Vector3.Angle(Vector3.up, myStatus.hitNormal)) <= myCController.slopeLimit;
			//TextUpdate.Instance.SetText("Hit Normal", (Vector3.Angle(Vector3.up, myStatus.hitNormal).ToString()));
		}
		else
			myStatus.isStableGround = false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		myStatus.hitNormal = hit.normal;
	}
}
