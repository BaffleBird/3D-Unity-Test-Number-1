using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
	 public EnemySpiderStats spiderStats;

	protected override void Awake()
	{
		base.Awake();

		States.Add("Idle", new Enemy_IdleState("Idle", this));
		States.Add("Move", new Enemy_MoveState("Move", this));
		States.Add("Turn", new Enemy_TurnState("Turn", this));
		States.Add("Leap", new Enemy_LeapState("Leap", this));

		currentState = States["Idle"];
		_previousState = currentState.StateName;
	}

	private void Start()
	{
		currentState.StartState();
	}
}
