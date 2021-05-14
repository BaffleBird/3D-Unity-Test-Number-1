using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
	PlayerControls playerControls;
	public PlayerControls PlayerControl => playerControls;
	[SerializeField] PlayerCameraZoom playerCameraZoom;

	Camera playerCam;
	Vector3 camX;
	Vector3 camY;

	private void Awake()
	{
		playerControls = new PlayerControls();
		inputs.Add("Jump", false);

		playerControls.InGameActions.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		playerControls.InGameActions.Movement.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();

		playerControls.InGameActions.Zoom.performed += ctx => playerCameraZoom.AdjustCameraZoomIndex(ctx.ReadValue<float>());

		playerControls.InGameActions.Jump.started += ctx => inputs["Jump"] = true;
		playerControls.InGameActions.Jump.canceled += ctx => inputs["Jump"] = false;

		playerCam = Camera.main;
	}

	private void FixedUpdate()
	{
		camX = playerCam.transform.right;
		camY = playerCam.transform.forward;
		camX.y = 0;
		camY.y = 0;
		camX.Normalize();
		camY.Normalize();

		pointerTarget = camX + camY;
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
