﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{

	[Range(0.0f,100.0f)]
	[SerializeField]
	private float vivacity = 100.0f;

	private Coroutine actualNaturalExtinguishingCoroutine;

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
		actualNaturalExtinguishingCoroutine = StartCoroutine(NaturalEstinguishingCoroutine());
		
		fireEmitter = fireParticles.emission;
		mainFire = fireParticles.main;

		cindersEmitter = cindersParticles.emission;
		
	}

	private void Update()
	{
		UpdateLight();
		UpdateVFX();
	}

	/// <summary>
	/// update the spot light parameters
	/// </summary>
	public void UpdateLight()
	{
		fireLight.spotAngle = Mathf.Lerp(minLightSpotAngle, maxLightSpotAngle, vivacity / 100.0f);
		fireLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, vivacity / 100.0f);
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

	public void UpdateVFX()
	{
		mainFire.startSize = Mathf.Lerp(minFireSize, maxFireSize, vivacity / 100.0f);
		mainFire.gravityModifier = Mathf.Lerp(minGravity, maxGravity, vivacity / 100.0f);

		fireEmitter.rateOverTime = Mathf.Lerp(minFireEmission, maxFireEmission, vivacity / 100.0f);
		cindersEmitter.rateOverTime = Mathf.Lerp(minCindersEmission, maxCindersEmission, vivacity / 100.0f);
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
