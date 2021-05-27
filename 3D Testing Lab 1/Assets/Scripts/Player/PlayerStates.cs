using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : State
{
	Vector3 currentMotion;
	float xVelocity;
	float yVelocity;

	float refGravity;

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
		if (SM.myInputs.GetInput("Jump") && SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (SM.myInputs.GetInput("Dodge"))
			SM.SwitchState("Dodge");
		else if (SM.myInputs.MoveInput != Vector2.zero)
			SM.SwitchState("Move");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.x = Mathf.SmoothDamp(currentMotion.x, 0, ref xVelocity, 0.2f);
		currentMotion.z = Mathf.SmoothDamp(currentMotion.z, 0, ref yVelocity, 0.2f);
		if (!SM.myCController.isGrounded)
			currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refGravity, 1f);
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
	float refGravity;

	Quaternion targetRot;
	Vector3 targetDirection;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.GetInput("Jump"))
			SM.SwitchState("Jump");
		else if (SM.myInputs.GetInput("Dodge"))
			SM.SwitchState("Dodge");
		else if (SM.myInputs.MoveInput == Vector2.zero && currentMotion.sqrMagnitude != 0)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		Vector3 targetMotion = Vector3.SmoothDamp(currentMotion, targetDirection * moveSpeed, ref velocityRef, 0.16f);
		currentMotion.x = targetMotion.x;
		currentMotion.z = targetMotion.z;
		if (!SM.myCController.isGrounded)
			currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refGravity, 1f);

		return currentMotion; 
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myInputs.transform.rotation = targetRot;
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

	float jumpSpeed = 12f;
	float jumpCounter = 0;
	float refVelocity;
	
	public Player_JumpState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		moveSpeed = 0f;
		switch (SM.previousState)
		{
			case "Move":
				moveSpeed = 7f;
				break;
			case "Dodge":
				moveSpeed = 12f;
				break;
			case "Sprint":
				moveSpeed = 10f;
				break;
			case "Idle":
				moveSpeed = 0f;
				break;
		}
		
		if (SM.myCController.isGrounded)
		{
			targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
			currentMotion = targetDirection.normalized * moveSpeed;
			currentMotion.y = jumpSpeed;
			jumpCounter = 0.25f;
		}
		else
		{
			targetDirection = SM.myModel.transform.forward;
			currentMotion = targetDirection.normalized * moveSpeed;
			jumpCounter = 0f;
		}
		
	}

	public override void UpdateState()
	{
		if (jumpCounter > 0)
			jumpCounter -= Time.deltaTime;
		if (jumpCounter > 0 && SM.myInputs.GetInput("Jump"))
			currentMotion.y = jumpSpeed;


		
		if (SM.myInputs.GetInput("Dodge"))
			SM.SwitchState("Dodge");
		if (SM.myCController.isGrounded)
		{
			if (!SM.myStatus.isStableGround)
				SM.SwitchState("Slide");
			else if (SM.myInputs.MoveInput != Vector2.zero && currentMotion.y <= 0)
				SM.SwitchState("Move");
			else if (currentMotion.y <= 0)
				SM.SwitchState("Idle");
		}
		
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refVelocity, 1f);
		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myInputs.transform.rotation = targetRot;
		}
	}

	public override void EndState()
	{
		SM.myStatus.SetCooldown("Jump", 0.1f);
	}
}

public class Player_DodgeState : State
{
	Vector3 currentMotion;
	Vector3 velocityRef;

	float refGravity;

	Quaternion targetRot;
	Vector3 targetDirection;

	float dodgeSpeed = 12f;
	float dodgeCounter = 0f;

	public Player_DodgeState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		if (SM.myInputs.MoveInput != Vector2.zero)
			targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		else
			targetDirection = SM.myModel.transform.forward;
		currentMotion = targetDirection.normalized * dodgeSpeed;
		dodgeCounter = 0.3f;

		SM.myInputs.ResetInput("Dodge");
	}

	public override void UpdateState()
	{
		if (dodgeCounter > 0) dodgeCounter -= Time.deltaTime;

		if (SM.myInputs.GetInput("Jump") && SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (dodgeCounter <= 0 && SM.myCController.isGrounded && SM.myInputs.MoveInput != Vector2.zero && SM.myInputs.GetInput("DodgeHold"))
			SM.SwitchState("Sprint");
		else if (dodgeCounter <= 0 && SM.myCController.isGrounded && SM.myInputs.MoveInput != Vector2.zero)
			SM.SwitchState("Move");
		else if (dodgeCounter <= 0 && !SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (dodgeCounter <= 0 && currentMotion.sqrMagnitude != 0 && SM.myCController.isGrounded)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		if (dodgeCounter > 0)
			currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * dodgeSpeed, ref velocityRef, 0.16f);
		else
			currentMotion = Vector3.SmoothDamp(currentMotion, Vector3.zero, ref velocityRef, 0.16f);

		if (!SM.myCController.isGrounded)
			currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity * 0.25f, ref refGravity, 1f);

		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myInputs.transform.rotation = targetRot;
		}
	}

	public override void EndState()
	{
		SM.myStatus.SetCooldown("Dodge", 0.25f);
	}
}

public class Player_SprintState : State
{
	float sprintSpeed = 10f;

	Vector3 currentMotion;
	Vector3 velocityRef;
	float refGravity;

	Quaternion targetRot;
	Vector3 targetDirection;

	public Player_SprintState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.GetInput("Jump"))
			SM.SwitchState("Jump");
		else if (SM.myInputs.MoveInput != Vector2.zero && !SM.myInputs.GetInput("DodgeHold"))
			SM.SwitchState("Move");
		else if (SM.myInputs.MoveInput == Vector2.zero)
			SM.SwitchState("Idle");

	}

	public override Vector3 MotionUpdate()
	{
		targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * sprintSpeed, ref velocityRef, 0.16f);
		if (!SM.myCController.isGrounded)
			currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refGravity, 1f);

		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myInputs.transform.rotation = targetRot;
		}
	}

	public override void EndState()
	{

	}
}

public class Player_SlideState : State
{
	float slideSpeed = 12f;
	float slideCounter;

	Vector3 slideMotion;
	Vector3 currentMotion;
	float xVelocity;
	float yVelocity;

	float refGravity;

	public Player_SlideState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;
		xVelocity = 0;
		yVelocity = 0;

		slideCounter = 0.5f;
	}

	public override void UpdateState()
	{
		if (slideCounter > 0)
			slideCounter -= Time.deltaTime;

		if (SM.myStatus.isStableGround)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		slideMotion.x = ((1f - SM.myStatus.hitNormal.y) * SM.myStatus.hitNormal.x) * slideSpeed;
		slideMotion.z = ((1f - SM.myStatus.hitNormal.y) * SM.myStatus.hitNormal.z) * slideSpeed;

		currentMotion.x = Mathf.SmoothDamp(currentMotion.x, slideMotion.x, ref xVelocity, 0.2f);
		currentMotion.z = Mathf.SmoothDamp(currentMotion.z, slideMotion.z, ref yVelocity, 0.2f);
		currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refGravity, 1f);

		return currentMotion;
	}

	public override void EndState()
	{

	}
}