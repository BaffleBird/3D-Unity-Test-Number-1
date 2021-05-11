using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCameraZoom : MonoBehaviour
{
	[SerializeField] CinemachineFreeLook targetFreeLookCamera;

	float[] baseRadii;
	float[] baseHeights;
	[SerializeField] float zoomSpeed = 0.1f;
	[SerializeField] float zoomMin = 0.25f;
	[SerializeField] float zoomMax = 1.5f;

	float currentRadius = 1f;
	float newRadius = 1f;

	private void Start()
	{
		//Retain the Default Orbit settings as set in editor
		baseRadii = new float[] { targetFreeLookCamera.m_Orbits[0].m_Radius, targetFreeLookCamera.m_Orbits[1].m_Radius, targetFreeLookCamera.m_Orbits[2].m_Radius };
		baseHeights = new float[] { targetFreeLookCamera.m_Orbits[0].m_Height, targetFreeLookCamera.m_Orbits[1].m_Height, targetFreeLookCamera.m_Orbits[2].m_Height };
	}

	private void LateUpdate()
	{
		UpdateZoom();
	}

	public void AdjustCameraZoomIndex(float zoomYAxis)
	{
		if (zoomYAxis == 0) { return; }

		if (zoomYAxis < 0)
			newRadius = currentRadius + zoomSpeed;

		if (zoomYAxis > 0)
			newRadius = currentRadius - zoomSpeed;
	}

	public void UpdateZoom()
	{
		if (currentRadius != newRadius)
		{
			currentRadius = Mathf.Lerp(currentRadius, newRadius, 0.2f);
			currentRadius = Mathf.Clamp(currentRadius, zoomMin, zoomMax);

			//Update Rig Stuff Here
			// > 
			targetFreeLookCamera.m_Orbits[0].m_Height = baseHeights[0] * currentRadius; //Top Rig
			targetFreeLookCamera.m_Orbits[1].m_Radius = baseRadii[1] * currentRadius; //Middle Rig
			//targetFreeLookCamera.m_Orbits[2].m_Height = -currentRadius; //Bottom Rig

		}
	}
}
