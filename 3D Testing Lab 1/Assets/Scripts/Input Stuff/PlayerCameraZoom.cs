using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCameraZoom : MonoBehaviour
{
	[SerializeField] CinemachineFreeLook targetFreeLookCamera;

	[SerializeField] float[] defaultRigRadii;
	[SerializeField] float[] defaultRigHeights;
	[SerializeField] float zoomSpeed = 1f;
	[SerializeField] float zoomMin = 3;
	[SerializeField] float zoomMax = 50;

	float currentRadius = 0f;
	float newRadius = 0f;

	public void AdjustCameraZoomIndex(float zoomYAxis)
	{
		if (zoomYAxis == 0) { return; }

		if (zoomYAxis < 0)
		{
			newRadius = currentRadius + zoomSpeed;
		}

		if (zoomYAxis < 0)
		{
			newRadius = currentRadius - zoomSpeed;
		}
	}

	public void UpdateZoom()
	{
		if (currentRadius != newRadius)
		{
			currentRadius = Mathf.Lerp(currentRadius, newRadius, 0.2f);
			currentRadius = Mathf.Clamp(currentRadius, zoomMin, zoomMax);

			//Update Rig Stuff Here
			targetFreeLookCamera.m_Orbits[1].m_Radius = currentRadius; //Middle Rig
			targetFreeLookCamera.m_Orbits[0].m_Height = currentRadius; //Top Rig
			targetFreeLookCamera.m_Orbits[2].m_Height = -currentRadius; //Bottom Rig

		}
	}
}
