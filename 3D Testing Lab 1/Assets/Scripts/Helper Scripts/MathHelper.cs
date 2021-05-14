using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
	//Scale a value within a given range into a new range
    public static float ScaleValue(float value, float currentMin, float currentMax, float newMin, float newMax)
	{
		//Remove the offset from the current min value and replace it with the new min
		//Multply by the scale fraction of the current and new ranges
		return newMin + (value - currentMin) * (newMax - newMin) / (currentMax - currentMin);
	}

	//Snap a value to the positive value, negative value, or to 0
	public static float SnapValue(float value, float snapValue)
	{
		if (value > snapValue * 0.5f)
			return snapValue;
		else if (value < -snapValue * 0.5f)
			return -snapValue;
		return 0;
	}

	//Just Zeroes out the Y component of a Vector
	public static Vector3 ZeroVectorY(Vector3 vector)
	{
		return new Vector3(vector.x, 0, vector.z);
	}

	public static Vector3 CameraAdjustedVector(Camera targetCamera, Vector2 movementVector)
	{
		Vector3 camX;
		Vector3 camY;
		camX = targetCamera.transform.right;
		camY = targetCamera.transform.forward;
		camX.y = 0;
		camY.y = 0;
		camX.Normalize();
		camY.Normalize();

		return (camX * movementVector.x) + (camY * movementVector.y);
	}
}
