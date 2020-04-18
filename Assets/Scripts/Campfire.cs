using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
	public struct MinMaxFloat
	{
		public float min;
		public float max;
	}

	[Range(0.0f,100.0f)]
	[SerializeField]
	private float vivacity = 100.0f;

	private Coroutine actualNaturalExtinguishingCoroutine;

	[Header("Natural estinguishing")]

	public float naturalEstinguishingRate = 0.1f;
	public float naturalEstinguishingAmount = 0.5f;

	[Header("Fire Light")]

	public Light fireLight;
	public MinMaxFloat lightSpotAngle;
	public MinMaxFloat lightIntensity;

	private void Awake()
	{
		actualNaturalExtinguishingCoroutine = StartCoroutine(NaturalEstinguishingCoroutine());
	}

	private void Update()
	{
		UpdateLight();
	}

	/// <summary>
	/// update the spot light parameters
	/// </summary>
	public void UpdateLight()
	{
		fireLight.spotAngle = Mathf.Lerp(lightSpotAngle.min, lightSpotAngle.max, vivacity / 100.0f);
		fireLight.intensity = Mathf.Lerp(lightIntensity.min, lightIntensity.max, vivacity / 100.0f);
	}

	/// <summary>
	/// coroutine that extinguish
	/// </summary>
	/// <returns></returns>
	private IEnumerator NaturalEstinguishingCoroutine()
	{
		while(vivacity > 0.0f)
		{
			yield return new WaitForSeconds(naturalEstinguishingRate);
			vivacity -= naturalEstinguishingAmount;
		}
		//gameover
	}

	/// <summary>
	/// make the fire regain some vivacity (called when wood is thrown at it for example)
	/// </summary>
	/// <param name="amount"></param>
	public void RegainVivacity(float amount)
	{
		StartCoroutine(SmoothRegainVivacity(amount));
	}

	/// <summary>
	/// coroutine that make the fire regain vivacity
	/// </summary>
	/// <param name="regain"></param>
	/// <returns></returns>
	private IEnumerator SmoothRegainVivacity(float regain)
	{
		float vivacityBefore = vivacity;
		float vivacityAfter = vivacity + regain;
		float currentLerpTime = 0.0f;
		float lerpTime = 0.5f;

		if(actualNaturalExtinguishingCoroutine != null)
		{
			StopCoroutine(actualNaturalExtinguishingCoroutine);
			actualNaturalExtinguishingCoroutine = null;
		}
		while(currentLerpTime < lerpTime)
		{
			float completion = currentLerpTime / lerpTime;
			vivacity = Mathf.Lerp(vivacityBefore, vivacityAfter, completion);
			currentLerpTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		if(actualNaturalExtinguishingCoroutine == null)
			actualNaturalExtinguishingCoroutine = StartCoroutine(NaturalEstinguishingCoroutine());

		yield return null;
	}
}
