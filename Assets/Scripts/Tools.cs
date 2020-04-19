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
	public static Vector2 RotatePosAroundPoint(Vector2 originPoint, Vector2 positionToRotate, float angle)
	{
		return new Vector2((positionToRotate.x - originPoint.x) * Mathf.Cos(angle) - (positionToRotate.y - originPoint.y) * Mathf.Sin(angle) + originPoint.x,
						   (positionToRotate.x - originPoint.x) * Mathf.Sin(angle) + (positionToRotate.y - originPoint.y) * Mathf.Cos(angle) + originPoint.y);
	}
}
