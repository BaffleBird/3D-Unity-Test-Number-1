using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
	[SerializeField] PlayerControls playerControls;
	public PlayerControls PlayerControl => playerControls;

	private void Awake()
	{
		playerControls = new PlayerControls();
		inputs.Add("Jump", Button.Up);
	}

	private void OnEnable()
	{
		playerControls.Enable();

		playerControls.InGameActions.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		playerControls.InGameActions.Movement.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();

		playerControls.InGameActions.Action1.started += ctx => inputs["Jump"] = Button.Down;
		playerControls.InGameActions.Action1.performed += ctx => inputs["Jump"] = Button.Hold;
		playerControls.InGameActions.Action1.canceled += ctx => inputs["Jump"] = Button.Up;
	}


	private void OnDisable()
	{
		playerControls.Disable();
	}
}
