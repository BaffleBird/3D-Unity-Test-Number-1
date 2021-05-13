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
		currentMotion.y = 0;
		xVelocity = 0;
		yVelocity = 0;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.GetInput("Jump") == Button.Down)
			SM.SwitchState("Jump");
		else if (SM.myInputs.MoveInput != Vector2.zero)
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
	float moveSpeed = 10f;

	Vector3 currentMotion;
	Vector3 velocityRef;

	Quaternion targetRot;

	Camera cam;
	Vector3 camX;
	Vector3 camY;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		targetRot = SM.myRigidbody.rotation;
		cam = Camera.main;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.GetInput("Jump") == Button.Down)
			SM.SwitchState("Jump");
		else if (SM.myInputs.MoveInput == Vector2.zero)
			SM.SwitchState("Idle");
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
		currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * moveSpeed, ref velocityRef, 0.16f);
		return currentMotion; 
	}

	public override void FixedUpdateState()
	{
		if (currentMotion != Vector3.zero)
			targetRot = Quaternion.Lerp(SM.myRigidbody.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion.normalized)), 0.2f);
		SM.myRigidbody.MoveRotation(targetRot);
	}

	public override void EndState()
	{

	}
}

public class Player_JumpState : State
{
	Vector3 currentMotion;
	Quaternion targetRot;

	float xVelocity;
	float yVelocity;

	public Player_JumpState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion.x = SM.myInputs.MoveInput.x;
		currentMotion.z = SM.myInputs.MoveInput.y;
		currentMotion = currentMotion * SM.myStatus.currentMovement.magnitude;
		xVelocity = 0;
		yVelocity = 0;

		currentMotion.y = 20f;
		SM.myStatus.isGrounded = false;
	}

	public override void UpdateState()
	{
		if (SM.myStatus.isGrounded && SM.myInputs.MoveInput != Vector2.zero)
			SM.SwitchState("Move");
		if (SM.myStatus.isGrounded)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.y -= SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (currentMotion != Vector3.zero)
			targetRot = Quaternion.Lerp(SM.myRigidbody.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
		SM.myRigidbody.MoveRotation(targetRot);
	}

	public override void EndState()
	{

	}
}
