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

	[Header("Fire VFX")]
	public ParticleSystem fireParticles;
	private ParticleSystem.EmissionModule fireEmitter;
	private ParticleSystem.MainModule mainFire;

	public float minFireEmission;
	public float maxFireEmission;
	public float minFireSize;
	public float maxFireSize;
	public float minGravity;
	public float maxGravity;
	
	[Space]

	public ParticleSystem cindersParticles;
	private ParticleSystem.EmissionModule cindersEmitter;

	public float minCindersEmission;
	public float maxCindersEmission;

	private void Awake()
	{
		StartCoroutine(NaturalEstinguishingCoroutine());
		fireEmitter = fireParticles.emission;
		mainFire = fireParticles.main;

		cindersEmitter = cindersParticles.emission;
	}

	private void Update()
	{
		UpdateLight();
		UpdateVFX();
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

	public void UpdateVFX()
	{
		mainFire.startSize = Mathf.Lerp(minFireSize, maxFireSize, vivacity / 100.0f);
		mainFire.gravityModifier = Mathf.Lerp(minGravity, maxGravity, vivacity / 100.0f);

		fireEmitter.rateOverTime = Mathf.Lerp(minFireEmission, maxFireEmission, vivacity / 100.0f);
		cindersEmitter.rateOverTime = Mathf.Lerp(minCindersEmission, maxCindersEmission, vivacity / 100.0f);

	}
}
