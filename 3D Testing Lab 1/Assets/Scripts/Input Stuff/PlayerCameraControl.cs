using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


public class PlayerCameraControl : MonoBehaviour
{
	Transform parentTransform;
	Vector3 parentOffset;

	[SerializeField] PlayerInputHandler playerInput;
	[SerializeField] CinemachineVirtualCamera CM_virtualCamera;
	[SerializeField] CinemachineCameraOffset CM_CameraOffset;
	[SerializeField] Cinemachine3rdPersonAim CM_camera3rdPersonAim;

	[Header("Look Controls")]
	[SerializeField] float sensitivity = 0.5f;
	[SerializeField] float snappiness = 10f;
	[SerializeField] float upperAngle = 40;
	[SerializeField] float lowerAngle = 340;

	float xVelocity;
	float yVelocity;

	[Header("Zoom controls")]
	[SerializeField] float zoomSpeed = 0.1f;
	[SerializeField] float zoomLerp = 0.1f;
	[SerializeField] float zoomMin = 0f;
	[SerializeField] float zoomMax = 5f;

	float zoomIndex;

	private void Start()
	{
		parentTransform = transform.parent;
		parentOffset = transform.localPosition;
		transform.parent = null;

		zoomIndex = CM_CameraOffset.m_Offset.z;
	}

	private void Update()
	{
		UpdateLook();
		UpdateZoom();
	}

	private void LateUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, parentTransform.position + parentOffset, 0.2f);
	}

	public void UpdateLook()
	{
		//TextUpdate.Instance.SetText("Mouse Input", playerInput.PointerInput.ToString());

		// Horizontal Camera Rotation Input
		xVelocity = Mathf.Lerp(xVelocity, playerInput.PointerInput.x * sensitivity, snappiness * Time.deltaTime);

		// Vertical Camera Rotation Input
		yVelocity = Mathf.Lerp(yVelocity, -playerInput.PointerInput.y * sensitivity, snappiness * Time.deltaTime);

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
			TextUpdate.Instance.SetText("Zoom Index", zoomIndex.ToString());
		}
	}
}
