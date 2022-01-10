using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAI : MonoBehaviour
{
	public Transform target;
	[HideInInspector]public Vector2 direction;

	#region TestDelegate
	public delegate void Command(string signalID);
	public event Command SignalCommand;

	public void AICommand(string signalID)
	{
		SignalCommand(signalID);
	}
	#endregion

	private void Update()
	{
		if (Keyboard.current.digit1Key.wasPressedThisFrame)
		{
			//Randomly decide left or right
			direction = Vector2.right * (Random.value > 0.5 ? 1 : -1);
			AICommand("Move");
		}

		if (Keyboard.current.digit2Key.wasPressedThisFrame)
		{
			direction = Vector2.up;
			AICommand("Move");
		}

		if (Keyboard.current.digit3Key.wasPressedThisFrame)
		{
			AICommand("Turn");
		}

		if (Keyboard.current.digit4Key.wasPressedThisFrame)
		{
			AICommand("Leap");
		}

		if (Keyboard.current.digit5Key.wasPressedThisFrame)
		{
			AICommand("Attack1");
		}
	}
}
