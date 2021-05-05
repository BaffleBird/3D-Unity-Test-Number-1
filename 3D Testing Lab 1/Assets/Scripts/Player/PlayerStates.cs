using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveState : State
{
	Vector3 currentMotion;

	public Player_MoveState(string name, StateMachine stateMachine) : base(name, stateMachine) { }

	public override void StartState()
	{

	}

	public override void UpdateState()
	{

	}

	public override Vector3 MotionUpdate()
	{
		//This is wrong btw. Gotta break out the physics brain again. Probably Lerp currentMotion's X and Y based on input and max speed (Times sign of input)

		currentMotion.x = SM.myInputs.MoveInput.x;
		currentMotion.z = SM.myInputs.MoveInput.y;
		return currentMotion * SM.myStatus.currentMovement.magnitude; 
	}

	public override void EndState()
	{

	}
}
