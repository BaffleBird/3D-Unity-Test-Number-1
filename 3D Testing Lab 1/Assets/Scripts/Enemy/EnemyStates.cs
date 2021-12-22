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
			SM.SwitchState("Move");
		else if (SM.myInputs.GetInput("Turn"))
			SM.SwitchState("Turn");

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
	float moveSpeed = 3f;
	float turnSpeed = 3f;
	float moveTimer = 0f;

	Vector3 currentMotion;
	Vector3 targetDirection;
	Quaternion targetRotation;

	public Enemy_MoveState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;

		moveSpeed = ESM.spiderStats.moveSpeed;
		turnSpeed = ESM.spiderStats.turnSpeed;
		moveTimer = Random.Range(3,5);

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
		SM.transform.rotation = Quaternion.RotateTowards(SM.transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
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
	float turnSpeed = 0.1f;
	float moveTimer = 0f;

	Vector3 currentMotion;
	Quaternion targetRotation;

	public Enemy_TurnState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = Vector3.zero;
		currentMotion.y = 0;

		turnSpeed = ESM.spiderStats.turnSpeed * 1.2f;
		moveTimer = Random.Range(3, 5);
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
		if (moveTimer <= 0 || (Quaternion.Angle(SM.transform.rotation, targetRotation) <= 3))
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		SM.transform.rotation = Quaternion.RotateTowards(SM.transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
		SM.myInputs.ResetInput("Turn");
	}
}

//Crab Walk in any direction

//Leap