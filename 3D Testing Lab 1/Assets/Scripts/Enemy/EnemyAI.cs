using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAI : MonoBehaviour
{
	public Transform target;

	public Vector2 direction;

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
			AICommand("Move");
			//Randomly decide left or right
		}

		if (Keyboard.current.digit2Key.wasPressedThisFrame)
		{
			AICommand("Move");
			//Randomly decide forward or back (Or maybe just forward
		}
	}
}
