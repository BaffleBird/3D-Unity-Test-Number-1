using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputHandler : MonoBehaviour
{
	protected Vector2 moveInput = new Vector2();
	public Vector2 MoveInput => moveInput;

	protected Vector2 pointerInput = new Vector2();
	public Vector2 PointerInput => pointerInput;

	protected Dictionary<string, Button> inputs = new Dictionary<string, Button>();


	public Button GetInput(string s) => inputs[s];

	public void ResetAllInputs()
	{
		foreach (var key in inputs.Keys.ToList())
		{
			inputs[key] = Button.Up;
		}
	}
}

public enum Button { Down, Hold, Up }
