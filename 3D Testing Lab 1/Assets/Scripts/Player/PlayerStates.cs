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

		SM.myAnimator.SetTrigger("Idle");
	}

	public override void UpdateState()
	{
		if (!SM.myStatus.isStableGround && SM.myCController.isGrounded)
			SM.SwitchState("Slide");
		else if (SM.myInputs.GetInput("Jump") && SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (!SM.myCController.isGrounded)
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
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
		SM.myAnimator.ResetTrigger("Idle");
	}
}

public class Player_MoveState : State
{
	float moveSpeed = 6f;

	Vector3 currentMotion;
	Vector3 velocityRef;

	float refGravity;

	Quaternion targetRot;
	Vector3 targetDirection;

	Vector3 uphillResistance;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;

		SM.myAnimator.SetTrigger("Move");
	}

	public override void UpdateState()
	{
		//TextUpdate.Instance.SetText("Slope %", SM.StabilityPercentage().ToString());

		if (!SM.myStatus.isStableGround && SM.myCController.isGrounded)
			SM.SwitchState("Slide");
		else if (SM.myInputs.GetInput("Jump") || !SM.myCController.isGrounded)
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
		currentMotion.y = -SM.myStatus.Gravity;

		UphillResist();

		return currentMotion; 
	}

	public void UphillResist()
	{
		//If they are in opposite directions, generate resistance based on how opposite they are.
		uphillResistance.x = ((1f - SM.myStatus.hitNormal.y) * SM.myStatus.hitNormal.x);
		if (currentMotion.normalized.x * uphillResistance.normalized.x < 0)
		{
			uphillResistance.x *= moveSpeed * (SM.StabilityPercentage() / 100) * MathHelper.AbAbDifference(currentMotion.normalized.x, uphillResistance.normalized.x) * 0.2f;
		}
		else
			uphillResistance.x = 0;

		uphillResistance.z = ((1f - SM.myStatus.hitNormal.y) * SM.myStatus.hitNormal.z);
		if (currentMotion.normalized.z * uphillResistance.normalized.z < 0)
		{
			uphillResistance.z *= moveSpeed * (SM.StabilityPercentage() / 100) * MathHelper.AbAbDifference(currentMotion.normalized.z, uphillResistance.normalized.z) * 0.2f;
		}
		else
			uphillResistance.z = 0;

		currentMotion += uphillResistance;

		TextUpdate.Instance.SetText("Uphill", uphillResistance.ToString());
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.transform.rotation = targetRot;
		}
	}

	public override void EndState()
	{
		SM.myAnimator.ResetTrigger("Move");
	}
}

public class Player_DodgeState : State
{
	Vector3 currentMotion;
	Vector3 velocityRef;

	float refGravity;

	Quaternion targetRot;
	Vector3 targetDirection;

	float dodgeSpeed = 14f;
	float dodgeCount = 0.3f;
	//float dodgeCancellable = 0.1f;
	float dodgeCounter = 0f;

	public Player_DodgeState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		if (SM.myInputs.MoveInput != Vector2.zero)
			targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		else
			targetDirection = SM.myModel.transform.forward;
		currentMotion = targetDirection.normalized * dodgeSpeed;

		dodgeCounter = dodgeCount;
		SM.myInputs.ResetInput("Dodge");

		SM.myAnimator.SetTrigger("Dodge");
		//SM.myAnimator.Play("Dodge");
	}

	public override void UpdateState()
	{
		if (dodgeCounter > 0) dodgeCounter -= Time.deltaTime;

		if (!SM.myStatus.isStableGround && SM.myCController.isGrounded)
			SM.SwitchState("Slide");
		else if (SM.myInputs.GetInput("Jump") && SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (dodgeCounter <= 0)
		{
			if (SM.myCController.isGrounded && SM.myInputs.MoveInput != Vector2.zero && SM.myInputs.GetInput("DodgeHold"))
				SM.SwitchState("Sprint");
			else if (!SM.myCController.isGrounded)
				SM.SwitchState("Jump");
			else if (SM.myCController.isGrounded && SM.myInputs.MoveInput != Vector2.zero)
				SM.SwitchState("Move");
			else if (currentMotion.sqrMagnitude != 0 && SM.myCController.isGrounded)
				SM.SwitchState("Idle");
		}
		

	}

	public override Vector3 MotionUpdate()
	{
		if (dodgeCounter > 0)
			currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * dodgeSpeed, ref velocityRef, 0.3f);
		else
			currentMotion = Vector3.SmoothDamp(currentMotion, Vector3.zero, ref velocityRef, 0.16f);

		if (!SM.myCController.isGrounded)
			currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refGravity, 1f);

		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.3f);
			SM.myInputs.transform.rotation = targetRot;
		}
	}

	public override void EndState()
	{
		SM.myStatus.SetCooldown("Dodge", 0.25f);
		SM.myAnimator.ResetTrigger("Dodge");
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

		SM.myAnimator.SetTrigger("Sprint");
	}

	public override void UpdateState()
	{
		if (!SM.myStatus.isStableGround && SM.myCController.isGrounded)
			SM.SwitchState("Slide");
		else if (SM.myInputs.GetInput("Jump") || !SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (SM.myInputs.MoveInput != Vector2.zero && (!SM.myInputs.GetInput("DodgeHold") || SM.myInputs.GetInput("Shoot")))
			SM.SwitchState("Move");
		else if (SM.myInputs.MoveInput == Vector2.zero)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		float adjustedSpeed = sprintSpeed - (sprintSpeed * (SM.StabilityPercentage() / 100) * 0.5f);
		targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		currentMotion = Vector3.SmoothDamp(currentMotion, targetDirection * adjustedSpeed, ref velocityRef, 0.2f);
		currentMotion.y = -SM.myStatus.Gravity;

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
		SM.myAnimator.ResetTrigger("Sprint");
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
		moveSpeed = MathHelper.ZeroVectorY(SM.myStatus.currentMovement).magnitude;
		
		if (SM.previousState == "Slide" && SM.myCController.isGrounded)
		{
			targetDirection = MathHelper.ZeroVectorY(SM.myStatus.currentMovement.normalized);
			currentMotion = targetDirection.normalized * moveSpeed;
			currentMotion.y = jumpSpeed;
		}
		else if (SM.myCController.isGrounded || !SM.myStatus.GetCooldown("Coyote"))
		{
			targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
			currentMotion = targetDirection.normalized * moveSpeed;
			currentMotion.y = jumpSpeed;
			jumpCounter = 0.25f;
			SM.myStatus.SetCooldown("Coyote", 0f);
		}
		else
		{
			targetDirection = SM.myModel.transform.forward;
			currentMotion = targetDirection.normalized * moveSpeed;
			jumpCounter = 0f;
			SM.myStatus.SetCooldown("Coyote", 0.16f);
		}

		SM.myInputs.ResetInput("Jump");
		SM.myAnimator.SetTrigger("Jump");
	}

	public override void UpdateState()
	{
		if (jumpCounter > 0)
			jumpCounter -= Time.deltaTime;
		if (jumpCounter > 0 && SM.myInputs.GetInput("JumpHold"))
			currentMotion.y = jumpSpeed;

		if (SM.myInputs.GetInput("Jump") && (Vector3.Angle(Vector3.up, SM.myStatus.hitNormal) > 80) && SM.myStatus.isTouchingWall) //Check if touching wall
			SM.SwitchState("WallJump");
		else if (SM.myInputs.GetInput("Jump") && !SM.myStatus.GetCooldown("Coyote"))
			SM.SwitchState("Jump");
		else if (SM.myInputs.GetInput("Dodge"))
			SM.SwitchState("Dodge");
		else if (SM.myCController.isGrounded)
		{
			if (!SM.myStatus.isStableGround)
				SM.SwitchState("Slide");
			else if (SM.myInputs.MoveInput != Vector2.zero && currentMotion.y <= 0 && SM.myInputs.GetInput("DodgeHold"))
				SM.SwitchState("Sprint");
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
		SM.myAnimator.ResetTrigger("Jump");
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

	Quaternion targetRot;

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

		if (!SM.myCController.isGrounded)
			SM.SwitchState("Jump");
		else if (SM.myInputs.GetInput("Jump"))
			SM.SwitchState("Jump");
		else if (SM.myStatus.isStableGround)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		//Determine direction of slide from colllison normal
		slideMotion.x = ((1f - SM.myStatus.hitNormal.y) * SM.myStatus.hitNormal.x) * slideSpeed;
		slideMotion.z = ((1f - SM.myStatus.hitNormal.y) * SM.myStatus.hitNormal.z) * slideSpeed;

		//Determine clockwise(+/right) vs counterclockwise(-/left) depending on camera and movement keys
		//Compare targetDirection to slideMotion to to determine direction (Figure out if x axis is positive or negative)
		Vector3 targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		Vector3 sideWind = Quaternion.Euler(0, MathHelper.AngleDir(slideMotion, targetDirection) * 60, 0) * MathHelper.ZeroVectorY(slideMotion) * slideSpeed * 0.16f;

		//Apply sideWind to slideMotion x and z
		currentMotion.x = Mathf.SmoothDamp(currentMotion.x, slideMotion.x + sideWind.x, ref xVelocity, 0.2f);
		currentMotion.z = Mathf.SmoothDamp(currentMotion.z, slideMotion.z + sideWind.z, ref yVelocity, 0.2f);
		currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity * 0.5f, ref refGravity, 0.2f);

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

public class Player_WallJumpState : State
{
	//Stronger than a normal jump, so figure that out
	//2 Stages
	//Launch Stage
	//Jump Stage (Should work like the normal jump state after launch? But no jump holds and jump speed might differ per angle of approach?)
	Vector3 currentMotion;
	float moveSpeed = 12f;

	Quaternion targetRot;
	Vector3 targetDirection;

	Vector3 JumpTrajectory;

	float jumpSpeed = 20f;
	float refVelocity;

	float animationHold = 0.2f; // **TEMPORARY** //
	bool launched = false;

	public Player_WallJumpState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		//Record Trajectory - Determine Vector of direction of approach
		if (SM.myInputs.MoveInput != Vector2.zero)
			targetDirection = MathHelper.CameraAdjustedVector(Camera.main, SM.myInputs.MoveInput);
		else
			targetDirection = SM.myModel.transform.forward;

		JumpTrajectory = Vector3.zero;
		JumpTrajectory = Vector3.Reflect(targetDirection * moveSpeed, SM.myStatus.hitNormal);
		JumpTrajectory.y = jumpSpeed;

		//Snap to Wall for animation if possible
		SM.myAnimator.Play("Wall Jump");
		targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(SM.myStatus.hitNormal)), 0.2f);
		SM.myInputs.transform.rotation = targetRot;

		//Stop - Wait a Minute
		currentMotion = Vector3.zero;
		animationHold = 0.1f; // **TEMPORARY** //
		launched = false;
	}

	
	public override void UpdateState()
	{
		//Wait a sec for animation I guess
		//Modify trajectory with input?
		//Launch - Hold for launch strength?
		//Same Shenanigans

		if (animationHold > 0)
			animationHold -= Time.deltaTime;
		else if (launched == false)
		{
			currentMotion = JumpTrajectory;
			launched = true;
			SM.myAnimator.SetTrigger("Jump");
		}
		else
		{
			if (SM.myInputs.GetInput("Jump") && (Vector3.Angle(Vector3.up, SM.myStatus.hitNormal) > 80) && SM.myStatus.isTouchingWall) //Check if touching wall
			{
				SM.SwitchState("WallJump");
				SM.myStatus.SetCooldown("WallJump", 0.3f);
			}
			else if (SM.myInputs.GetInput("Dodge"))
			{
				SM.SwitchState("Dodge");
				SM.myStatus.SetCooldown("WallJump", 1f);
			}
			else if (SM.myCController.isGrounded)
			{
				if (!SM.myStatus.isStableGround)
					SM.SwitchState("Slide");
				else if (SM.myInputs.MoveInput != Vector2.zero && currentMotion.y <= 0 && SM.myInputs.GetInput("DodgeHold"))
					SM.SwitchState("Sprint");
				else if (SM.myInputs.MoveInput != Vector2.zero && currentMotion.y <= 0)
					SM.SwitchState("Move");
				else if (currentMotion.y <= 0)
					SM.SwitchState("Idle");
			}
		}
	}

	public override Vector3 MotionUpdate()
	{
		//Same Shenanigans
		if (launched) currentMotion.y = Mathf.SmoothDamp(currentMotion.y, -SM.myStatus.Gravity, ref refVelocity, 1f);
		return currentMotion;
	}

	public override void FixedUpdateState()
	{
		if (MathHelper.ZeroVectorY(currentMotion) != Vector3.zero && animationHold <= 0)
		{
			targetRot = Quaternion.Lerp(SM.myModel.transform.rotation, Quaternion.LookRotation(MathHelper.ZeroVectorY(currentMotion).normalized), 0.2f);
			SM.myInputs.transform.rotation = targetRot;
		}
		
	}

	public override void EndState()
	{
		SM.myStatus.SetCooldown("WallJump", 0.1f);
	}
}