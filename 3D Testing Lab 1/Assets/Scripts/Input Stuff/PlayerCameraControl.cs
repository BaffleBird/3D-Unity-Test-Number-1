using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCameraControl : MonoBehaviour
{
	Transform parentTransform;
	Vector3 parentOffset;

	[SerializeField] StateMachine playerMainSM;
	[SerializeField] StateMachine playerUpperSM;
	[SerializeField] PlayerInputHandler playerInput;
	[SerializeField] CinemachineCameraOffset CM_CameraOffset;
	[SerializeField] CinemachineCameraOffset CM_AimCameraOffset;

	[SerializeField] GameObject moveCamera;
	[SerializeField] GameObject aimCamera;

	[Header("Look Controls")]
	[SerializeField] float sensitivity = 0.5f;
	[SerializeField] float snappiness = 10f;
	[SerializeField] float upperAngle = 40;
	[SerializeField] float lowerAngle = 340;
	float sensitivityModifier = 1;

	float xVelocity;
	float yVelocity;

	[Header("Zoom controls")]
	[SerializeField] float zoomSpeed = 0.1f;
	[SerializeField] float zoomLerp = 0.1f;
	[SerializeField] float zoomMin = 0f;
	[SerializeField] float zoomMax = 5f;

	float zoomIndex;
	float zoomSideOffset;

	private void Start()
	{
		parentTransform = transform.parent;
		parentOffset = transform.localPosition;
		transform.parent = null;

		zoomIndex = CM_CameraOffset.m_Offset.z;
		zoomSideOffset = CM_AimCameraOffset.m_Offset.x;
	}

	private void OnEnable()
	{
		//Subscribe to the Statemachine's signals
		playerUpperSM.TestSignalEvent += SwitchCamera;
	}

	private void Update()
	{
		if (aimCamera.activeSelf && playerInput.GetInput("CameraSide"))
		{
			zoomSideOffset = zoomSideOffset > 0 ? -1 : 1;
			playerInput.ResetInput("CameraSide");
		}
	}

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, parentTransform.position + parentOffset, 0.5f);
		UpdateLook();
		UpdateZoom();
		playerInput.ResetPointerInput();
	}

	private void OnDisable()
	{
		playerUpperSM.TestSignalEvent -= SwitchCamera;
	}

	private void SwitchCamera(string signalID)
	{
		if (signalID != "ShiftCamera")
			return;

		if ((playerUpperSM.currentStateName == "Shoot" || playerUpperSM.currentStateName == "Cannon")
			&& !aimCamera.activeInHierarchy)
		{
			moveCamera.SetActive(false);
			aimCamera.SetActive(true);
			if (playerUpperSM.currentStateName == "Cannon")
				sensitivityModifier = 0.5f;
		}
		else if ((playerUpperSM.currentStateName != "Shoot" && playerUpperSM.currentStateName != "Cannon")
			&& !moveCamera.activeInHierarchy)
		{
			moveCamera.SetActive(true);
			aimCamera.SetActive(false);
			sensitivityModifier = 1;
		}
	}

	public void UpdateLook()
	{
		//TextUpdate.Instance.SetText("Mouse Input", playerInput.PointerInput.ToString());

		// Horizontal Camera Rotation Input
		xVelocity = Mathf.Lerp(xVelocity, playerInput.PointerInput.x * sensitivity * sensitivityModifier, snappiness * Time.fixedDeltaTime);

		// Vertical Camera Rotation Input
		yVelocity = Mathf.Lerp(yVelocity, -playerInput.PointerInput.y * sensitivity * sensitivityModifier, snappiness * Time.fixedDeltaTime);

		transform.Rotate(yVelocity, xVelocity, 0);

		// Clamp the Up/Down rotation
		var angles = transform.localEulerAngles;
		angles.z = 0;

		var angle = transform.localEulerAngles.x;

		
		if (angle > 180 && angle < lowerAngle)
		{
			angles.x = lowerAngle;
		}
		else if (angle < 180 && angle > upperAngle)
		{
			angles.x = upperAngle;
		}

		transform.localEulerAngles = angles;
	}

	public void AdjustCameraZoomIndex(float zoomYAxis)
	{
		if (zoomYAxis == 0) { return; }

		if (zoomYAxis < 0)
			zoomIndex -= zoomSpeed;

		if (zoomYAxis > 0)
			zoomIndex += zoomSpeed;

		zoomIndex = Mathf.Clamp(zoomIndex, zoomMin, zoomMax);
	}

	public void UpdateZoom()
	{
		if (CM_CameraOffset.m_Offset.z != zoomIndex)
		{
			CM_CameraOffset.m_Offset.z = Mathf.Lerp(CM_CameraOffset.m_Offset.z, zoomIndex, zoomLerp);
			//TextUpdate.Instance.SetText("Zoom Index", zoomIndex.ToString());
		}

		if (CM_CameraOffset.m_Offset.x != zoomSideOffset)
		{
			CM_AimCameraOffset.m_Offset.x = Mathf.Lerp(CM_AimCameraOffset.m_Offset.x, zoomSideOffset, zoomLerp);
			//TextUpdate.Instance.SetText("Zoom Side", zoomSideOffset.ToString());
		}

	}
}
