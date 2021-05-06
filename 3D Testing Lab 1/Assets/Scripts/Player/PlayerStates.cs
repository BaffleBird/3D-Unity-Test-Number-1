using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : State
{
	Vector3 currentMotion;

	public Player_IdleState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.MoveInput != Vector2.zero)
			SM.SwitchState("Move");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.x = Mathf.Lerp(currentMotion.x, 0, SM.myStatus.LerpSpeed);
		currentMotion.z = Mathf.Lerp(currentMotion.z, 0, SM.myStatus.LerpSpeed);
		return currentMotion;
	}

	public override void EndState()
	{

	}
}

public class Player_MoveState : State
{
	Vector3 currentMotion;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{
		currentMotion = SM.myStatus.currentMovement;
	}

	public override void UpdateState()
	{
		if (SM.myInputs.MoveInput == Vector2.zero)
			SM.SwitchState("Idle");
	}

	public override Vector3 MotionUpdate()
	{
		currentMotion.x = Mathf.Lerp(currentMotion.x, SM.myInputs.MoveInput.x * SM.myStatus.MaxSpeed, SM.myStatus.LerpSpeed);
		currentMotion.z = Mathf.Lerp(currentMotion.z, SM.myInputs.MoveInput.y * SM.myStatus.MaxSpeed, SM.myStatus.LerpSpeed);
		return currentMotion; 
	}

	public override void EndState()
	{

	}
}
