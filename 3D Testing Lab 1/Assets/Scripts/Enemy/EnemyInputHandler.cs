using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInputHandler : InputHandler
{
	[SerializeField] EnemyAI enemyAI;

	private void Awake()
	{
		inputs.Add("Move", false);
		inputs.Add("Turn", false);
		enemyAI.SignalCommand += OnAICommand;
	}

	private void Update()
	{
		pointerTarget = (enemyAI.target.position - transform.position).normalized;
	}

	void OnAICommand(string signalID)
	{
		if (inputs.ContainsKey(signalID))
		{
			inputs[signalID] = true;
			moveInput = enemyAI.direction;
		}
			
	}

	private void OnDisable()
	{
		enemyAI.SignalCommand -= OnAICommand;
	}
}
