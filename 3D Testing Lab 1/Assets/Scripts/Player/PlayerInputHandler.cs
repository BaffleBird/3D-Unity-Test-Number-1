using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
	PlayerControls playerControls;
	public PlayerControls PlayerControl => playerControls;
	[SerializeField] PlayerCameraControl playerCameraZoom;

	Camera playerCam;
	Vector3 camX;
	Vector3 camY;

	private void Awake()
	{
		playerControls = new PlayerControls();
		inputs.Add("Jump", false);
		inputs.Add("JumpHold", false);
		inputs.Add("Dodge", false);
		inputs.Add("DodgeHold", false);
		inputs.Add("Shoot", false);

		playerControls.InGameActions.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		playerControls.InGameActions.Movement.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();

		playerControls.InGameActions.Look.performed += ctx => pointerInput = ctx.ReadValue<Vector2>();
		playerControls.InGameActions.Zoom.performed += ctx => playerCameraZoom.AdjustCameraZoomIndex(ctx.ReadValue<float>());

		playerControls.InGameActions.Jump.started += ctx => inputs["Jump"] = true;
		playerControls.InGameActions.Jump.performed += ctx => inputs["JumpHold"] = true;
		playerControls.InGameActions.Jump.canceled += ctx => inputs["JumpHold"] = false;
		playerControls.InGameActions.Jump.canceled += ctx => inputs["Jump"] = false;

		playerControls.InGameActions.Dodge.started += ctx => inputs["Dodge"] = true;
		playerControls.InGameActions.Dodge.performed += ctx => inputs["DodgeHold"] = true;
		playerControls.InGameActions.Dodge.canceled += ctx => inputs["DodgeHold"] = false;
		playerControls.InGameActions.Dodge.canceled += ctx => inputs["Dodge"] = false;

		playerControls.InGameActions.Dodge.performed += ctx => inputs["Shoot"] = true;
		playerControls.InGameActions.Dodge.canceled += ctx => inputs["Shoot"] = false;
	}

	private void LateUpdate()
	{
		pointerInput = Vector2.zero;
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
