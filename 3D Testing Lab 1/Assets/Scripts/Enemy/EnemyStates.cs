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
		else if (SM.myInputs.GetInput("Leap"))
			SM.SwitchState("Leap");
		else if (SM.myInputs.GetInput("Attack1"))
			SM.SwitchState("Attack1");

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

		targetRotation = Quaternion.LookRotation((SM.myInputs.PointerTarget - SM.transform.position).normalized);
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
		targetRotation = Quaternion.LookRotation((SM.myInputs.PointerTarget - SM.transform.position).normalized);
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

public class Enemy_LeapState : Enemy_State
{
	Vector3 currentMotion = new Vector3();
	float jumpCounter;
	public Enemy_LeapState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = Vector3.zero;
		jumpCounter = 0.1f;

		//Get Target Position from InputHandler PointerTarget
		//Get Jump Angle and Leap Gravity from SpiderStats
		float angle = ESM.spiderStats.leapAngle * Mathf.Deg2Rad;

		//Separate Vertical and Lateral Components of Target Position
		Vector3 targetVertical = MathHelper.ZeroVector(ESM.myInputs.PointerTarget, true, false, true);
		Vector3 targetLateral = MathHelper.ZeroVector(ESM.myInputs.PointerTarget, false, true, false);
		Vector3 currentLateral = MathHelper.ZeroVector(SM.transform.position, false, true, false);

		//Get the Vertical and Lateral distances
		Vector3 offset = ESM.myInputs.PointerTarget - SM.transform.position;
		float distance = offset.magnitude * 2;

		//Calculate Leap Vector
		float velocity = (distance * Mathf.Sqrt(ESM.spiderStats.leapGravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * -offset.y * Mathf.Cos(angle));
		currentMotion = new Vector3(0, velocity * Mathf.Sin(angle), velocity * Mathf.Cos(angle));

		//Rotate Vector to towards target location
		float angleBetween = Vector3.Angle(Vector3.forward, targetLateral - currentLateral) * (targetLateral.x > currentLateral.x ? 1 : -1);
		currentMotion = Quaternion.AngleAxis(angleBetween, Vector3.up) * currentMotion;
	}

	public override void UpdateState()
	{
		jumpCounter -= Time.deltaTime;
		Transition();
	}

	public override void Transition()
	{
		//Return to IdleState On Impact for now
		if (SM.myCController.isGrounded && jumpCounter <= 0)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.y -= ESM.spiderStats.leapGravity * Time.fixedDeltaTime;
		return currentMotion;
	}

	public override void EndState()
	{
		SM.myInputs.ResetInput("Leap");
	}
}

public class Enemy_ShootState : Enemy_State
{
	Vector3 currentMotion;
	float attackTime = 0;

	public Enemy_ShootState(string name, EnemyStateMachine statemachine) : base(name, statemachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
		currentMotion.y = 0;

		attackTime = 3;
		SM.FireSignal("Fire");
	}

	public override void UpdateState()
	{
		attackTime -= Time.deltaTime;
		Transition();
	}

	public override void Transition()
	{
		if (attackTime <= 0)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		Vector3 brakeVector = new Vector3(0, currentMotion.y, 0);
		currentMotion = Vector3.Lerp(currentMotion, brakeVector, 0.1f);
		currentMotion.y = -SM.myStatus.Gravity;
		return currentMotion;
	}

	public override void EndState()
	{
		SM.FireSignal("Ceasefire");
		SM.myInputs.ResetInput("Attack1");
	}
}