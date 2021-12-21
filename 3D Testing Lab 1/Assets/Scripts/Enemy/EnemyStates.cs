using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_State : State
{
	public EnemyStateMachine ESM;
	public Enemy_State(string name, EnemyStateMachine statemachine) : base(name, statemachine) 
	{
		ESM = statemachine;
	}
}

public class Enemy_IdleState : Enemy_State
{
	Vector3 currentMotion;
	float xVelocity;
	float yVelocity;
	
	public Enemy_IdleState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;
		xVelocity = 0;
		yVelocity = 0;
	}

	public override void UpdateState()
	{
		Transition();
	}

	public override void Transition()
	{
		if (SM.myInputs.GetInput("Move"))
		{
			SM.SwitchState("Move");
		}
			
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.x = Mathf.SmoothDamp(currentMotion.x, 0, ref xVelocity, 0.3f);
		currentMotion.z = Mathf.SmoothDamp(currentMotion.z, 0, ref yVelocity, 0.3f);
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
	}
}

public class Enemy_MoveState : Enemy_State
{
	float moveSpeed = 6f;
	float turnSpeed = 6f;
	float moveTimer = 0f;

	Vector3 currentMotion;
	Vector3 targetDirection;
	Quaternion targetRotation;

	public Enemy_MoveState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;
		moveTimer = 5f;

		//Have an target point that lerps towards the player position.
		//Then Rotate Transform to face that target point
			//Do this later
	}

	public override void UpdateState()
	{
		moveTimer -= Time.deltaTime;
		//Rotate Transform to face target
		targetRotation = Quaternion.LookRotation(SM.myInputs.PointerTarget);
		targetRotation.x = 0;
		targetRotation.z = 0;

		Transition();
	}

	public override void Transition()
	{
		if(moveTimer <= 0)
		{
			SM.SwitchState("Idle");
		}
	}

	public override Vector3 MotionUpdate()
	{
		SM.transform.rotation = Quaternion.Slerp(SM.transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
		targetDirection = MathHelper.TransformAdjustedVector(SM.transform, SM.myInputs.MoveInput);
		Vector3 targetMotion = targetDirection * moveSpeed;
		currentMotion.x = targetMotion.x;
		currentMotion.z = targetMotion.z;
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
		SM.myInputs.ResetInput("Move");
	}
}

public class Enemy_TurnState : Enemy_State
{
	Vector3 currentMotion;
	float turnSpeed = 6f;

	public Enemy_TurnState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = Vector3.zero;
		currentMotion.y = 0;
	}

	public override void UpdateState()
	{
		
		Transition();
	}

	public override void Transition()
	{

	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
	}
}

//Crab Walk in any direction

//Leap