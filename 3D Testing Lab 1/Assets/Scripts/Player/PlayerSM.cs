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

		currentState = States["Idle"];
		_previousState = currentState.StateName;
		currentState.StartState();

		myStatus.isGrounded = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Terrain")
		{
			myStatus.isGrounded = true;
		}
	}

	void GroundCheck()
	{
		RaycastHit hit;
		//if (Physics.Raycast(transform.position, Vector3.down, out hit, myRigidbody.))
	}
}
