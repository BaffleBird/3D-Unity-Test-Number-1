using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : State
{
	Vector3 currentMotion;
	float xVelocity;
	float yVelocity;

	public Player_IdleState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		xVelocity = 0;
		yVelocity = 0;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.MoveInput != Vector2.zero)
			SM.SwitchState("Move");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.x = Mathf.SmoothDamp(currentMotion.x, 0, ref xVelocity, 0.2f);
		currentMotion.z = Mathf.SmoothDamp(currentMotion.z, 0, ref yVelocity, 0.2f);
		return currentMotion;
	}

	public override void EndState()
	{

	}
}

public class Player_MoveState : State
{
	Vector3 currentMotion;
	Vector3 velocityRef;

	Camera cam;
	Vector3 camX;
	Vector3 camY;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		cam = Camera.main;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.MoveInput == Vector2.zero)
			SM.SwitchState("Idle");
		else if (currentMotion.normalized != Vector3.zero)
		{
			SM.myBody.transform.rotation = Quaternion.Lerp(SM.myBody.transform.rotation, Quaternion.LookRotation(currentMotion.normalized), 0.2f);
		}
	}

	public override Vector3 MotionUpdate()
	{
		camX = cam.transform.right;
		camY = cam.transform.forward;
		camX.y = 0;
		camY.y = 0;
		camX.Normalize();
		camY.Normalize();

		Vector3 targetDirection = (camX * SM.myInputs.MoveInput.x) + (camY * SM.myInputs.MoveInput.y);
		currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * SM.myStatus.MaxSpeed, ref velocityRef, 0.16f);
		return currentMotion; 
	}

	public override void EndState()
	{

	}
}
