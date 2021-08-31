﻿using System.Collections;
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

	//Return a what percentage a value is of another
	public static float Percentage(float valueToPercent, float maxValue)
	{
		return (valueToPercent / maxValue) * 100;
	}

	//Zero out a component of a Vector
	public static Vector3 ZeroVectorY(Vector3 vector)
	{
		return new Vector3(vector.x, 0, vector.z);
	}

	public static Vector2 ZeroVectorY(Vector2 vector)
	{
		return new Vector2(vector.x, 0);
	}

	public static Vector3 ZeroVectorX(Vector3 vector)
	{
		return new Vector3(0, vector.y, vector.z);
	}

	public static Vector2 ZeroVectorX(Vector2 vector)
	{
		return new Vector2(0, vector.y);
	}

	//Aligns a vector to Camera direction
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

	//Check which direction from a vector
	public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);

		if (dir > 0f)
			return 1f;
		else if (dir < 0f)
			return -1f;
		return 0f;
	}

	public static float AngleDir(Vector3 fwd, Vector3 targetDir)
	{
		var dot = fwd.x * -targetDir.z + fwd.z * targetDir.x;
		if (dot > 0)
			return 1f;
		else if (dot < 0)
			return -1f;
		else
			return 0f;
	}

	//The Absolute-Absolute difference
	public static float AbAbDifference(float a, float b)
	{
		return Mathf.Abs(Mathf.Abs(a) - Mathf.Abs(b));
	}
}