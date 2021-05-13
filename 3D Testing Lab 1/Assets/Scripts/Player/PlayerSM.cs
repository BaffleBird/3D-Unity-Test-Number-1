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
		States.Add("Jump", new Player_JumpState("Jump", this));

		currentState = States["Idle"];
		_previousState = currentState.StateName;
		currentState.StartState();

		myStatus.isGrounded = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Floor")
		{
			myStatus.isGrounded = true;
		}
	}
}
