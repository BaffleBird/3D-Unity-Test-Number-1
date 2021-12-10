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
		States.Add("WallJump", new Player_WallJumpState("WallJump", this)); myStatus.SetCooldown("WallJump", 0);

		currentState = States["Idle"];
		_previousState = currentState.StateName;
		currentState.StartState();
	}

	protected override void Update()
	{
		base.Update();

		myAnimator.SetFloat("y_input", myInputs.MoveInput.y);
		myAnimator.SetFloat("x_input", myInputs.MoveInput.x);
		myAnimator.SetFloat("y_motion", myStatus.currentMovement.z);
		myAnimator.SetFloat("x_motion", myStatus.currentMovement.x);

		TextUpdate.Instance.SetText("Player State", currentState.StateName);
		TextUpdate.Instance.SetText("Previous State", previousState);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		StabilityCheck();

		myStatus.isTouchingWall = (myCController.collisionFlags == CollisionFlags.Sides);


		//TextUpdate.Instance.SetText("Hit Normal", (Vector3.Angle(Vector3.up, myStatus.hitNormal).ToString()));
		TextUpdate.Instance.SetText("Grounded", myCController.isGrounded.ToString());
	}

	protected override void SwitchState(State newState)
	{
		base.SwitchState(newState);
		myStatus.currentState = currentState.StateName;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		myStatus.hitNormal = hit.normal;
	}
}
