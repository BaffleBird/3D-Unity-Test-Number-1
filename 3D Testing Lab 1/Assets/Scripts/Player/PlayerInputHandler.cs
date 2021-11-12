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
		inputs.Add("CameraSide", false);
		inputs.Add("Charge", false);

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

		playerControls.InGameActions.Shoot.performed += ctx => inputs["Shoot"] = true;
		playerControls.InGameActions.Shoot.canceled += ctx => inputs["Shoot"] = false;

		playerControls.InGameActions.CameraSide.started += ctx => inputs["CameraSide"] = true;
		playerControls.InGameActions.CameraSide.canceled += ctx => inputs["CameraSide"] = false;

		playerControls.InGameActions.Charge.performed += ctx => inputs["Charge"] = true;
		playerControls.InGameActions.Charge.canceled += ctx => inputs["Charge"] = false;
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
