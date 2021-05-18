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
		if (SM.myInputs.GetInput("Jump"))
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
	float moveSpeed = 7f;

	Vector3 currentMotion;
	Vector3 velocityRef;

	Quaternion targetRot;
	Vector3 targetDirection;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		targetRot = SM.myRigidbody.rotation;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.GetInput("Jump"))
			SM.SwitchState("Jump");
		else if (SM.myInputs.MoveInput == Vector2.zero && currentMotion.sqrMagnitude != 0)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * moveSpeed, ref velocityRef, 0.16f);

		return currentMotion; 
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myRigidbody.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myRigidbody.MoveRotation(targetRot);
		}
	}

	public override void EndState()
	{

	}
}

public class Player_JumpState : State
{
	Vector3 currentMotion;
	float moveSpeed = 0f;

	Quaternion targetRot;
	Vector3 targetDirection;
	
	public Player_JumpState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		moveSpeed = 0f;
		switch (SM.previousState)
		{
			case "Move":
				moveSpeed = 7f;
				break;
			case "Idle":
				moveSpeed = 0f;
				break;
		}
		targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		currentMotion = targetDirection.normalized * moveSpeed;
		
		currentMotion.y = 16f;
		SM.myStatus.isGrounded = false;
	}

	public override void UpdateState()
	{
		if (SM.myStatus.isGrounded && SM.myInputs.MoveInput != Vector2.zero)
			SM.SwitchState("Move");
		else if (SM.myStatus.isGrounded)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.y -= SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myRigidbody.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myRigidbody.MoveRotation(targetRot);
		}
	}

	public override void EndState()
	{
		SM.myStatus.SetCooldown("Jump", 0.1f);
	}
}
