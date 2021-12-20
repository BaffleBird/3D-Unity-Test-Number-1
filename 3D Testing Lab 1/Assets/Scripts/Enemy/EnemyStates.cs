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
	float moveTimer = 0f;

	Vector3 currentMotion;

	Vector3 targetDirection;

	public Enemy_MoveState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		float randomVal = Random.value;
		if(randomVal > 0.5)
		{

		}
		else
		{

		}

		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;
		moveTimer = 5f;
	}

	public override void UpdateState()
	{

		Transition();
	}

	public override void Transition()
	{
		if(moveTimer <= 0)
		{
			//swap back to idle
		}
	}

	public override Vector3 MotionUpdate()
	{
		targetDirection = MathHelper.TransformAdjustedVector(ESM.transform, SM.myInputs.MoveInput);
		Vector3 targetMotion = targetDirection * moveSpeed;
		currentMotion.x = targetMotion.x;
		currentMotion.z = targetMotion.z;
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
	}
}

public class Enemy_TurnState : Enemy_State
{
	Vector3 currentMotion;
	float turnSpeed;

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