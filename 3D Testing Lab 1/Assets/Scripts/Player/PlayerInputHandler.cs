using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
	PlayerControls playerControls;
	public PlayerControls PlayerControl => playerControls;
	[SerializeField] PlayerCameraZoom playerCameraZoom;

	private void Awake()
	{
		playerControls = new PlayerControls();
		inputs.Add("Jump", Button.Up);

		playerControls.InGameActions.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		playerControls.InGameActions.Movement.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();

		playerControls.InGameActions.Zoom.performed += ctx => playerCameraZoom.AdjustCameraZoomIndex(ctx.ReadValue<float>());
		playerControls.InGameActions.Zoom.canceled += ctx => playerCameraZoom.AdjustCameraZoomIndex(0f);

		playerControls.InGameActions.Action1.started += ctx => inputs["Jump"] = Button.Down;
		playerControls.InGameActions.Action1.performed += ctx => inputs["Jump"] = Button.Hold;
		playerControls.InGameActions.Action1.canceled += ctx => inputs["Jump"] = Button.Up;
	}

	private void OnEnable()
	{
		playerControls.Enable();
	}

	private void OnDisable()
	{
		playerControls.Disable();
	}
}
