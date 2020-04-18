using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{

	[Range(0.0f,100.0f)]
	[SerializeField]
	private float vivacity = 100.0f;

	[Header("Natural estinguishing")]

	public float naturalEstinguishingRate = 0.1f;
	public float naturalEstinguishingAmount = 0.5f;

	[Header("Fire Light")]

	public Light fireLight;
	public float minLightSpotAngle;
	public float maxLightSpotAngle;
	public float minLightIntensity;
	public float maxLightIntensity;

	private void Awake()
	{
		StartCoroutine(NaturalEstinguishingCoroutine());
	}

	private void Update()
	{
		UpdateLight();
	}

	public void UpdateLight()
	{
		float normalizedVivacityValue = Mathf.InverseLerp(0.0f, 100.0f, vivacity);

		fireLight.spotAngle = Mathf.Lerp(minLightSpotAngle, maxLightSpotAngle, normalizedVivacityValue);
		fireLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, normalizedVivacityValue);
	}

	private IEnumerator NaturalEstinguishingCoroutine()
	{
		while(vivacity > 0.0f)
		{
			yield return new WaitForSeconds(naturalEstinguishingRate);
			vivacity -= naturalEstinguishingAmount;
		}

		//gameover
	}
}
