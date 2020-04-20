using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MinMaxFloat
{
	public float min;
	public float max;
}

public static class Tools
{
	public static Vector3 RotatePosAroundPoint(Vector3 originPoint, Vector3 positionToRotate, float angle)
	{
		return new Vector3((positionToRotate.x - originPoint.x) * Mathf.Cos(angle) - (positionToRotate.z - originPoint.z) * Mathf.Sin(angle) + originPoint.x, originPoint.y,
						   (positionToRotate.x - originPoint.x) * Mathf.Sin(angle) + (positionToRotate.z - originPoint.z) * Mathf.Cos(angle) + originPoint.z);
	}

	public static Vector3 RandomPointOnCircle(MinMaxFloat distanceFromCenter)
	{
		int maximumTries = 10;

		for(int i=0; i<maximumTries; i++)
		{
			Vector2 point = Random.insideUnitCircle * distanceFromCenter.max;

			if(point.magnitude > distanceFromCenter.min)
			{
				return new Vector3(point.x, 0.0f, point.y);
			}
		}

		return Vector3.zero;
	}
}
