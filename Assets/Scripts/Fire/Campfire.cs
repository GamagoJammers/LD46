using System.Collections;
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

    [Header("Camp Fire Light")]

    public Light campFireLight;
    public MinMaxFloat campFireLightRange;

    [Header("Fire Light")]

	public Light fireLight;
	public MinMaxFloat lightSpotAngle;
	public MinMaxFloat lightIntensity;

	[Header("Fire VFX")]
	public ParticleSystem fireParticles;
	private ParticleSystem.EmissionModule fireEmitter;
	private ParticleSystem.MainModule mainFire;

	public MinMaxFloat fireEmission;
	public MinMaxFloat fireSize;
	public MinMaxFloat gravity;
	
	[Space]

	public ParticleSystem cindersParticles;
	private ParticleSystem.EmissionModule cindersEmitter;

	public float minCindersEmission;
	public float maxCindersEmission;

	private void Start()
	{
		actualNaturalExtinguishingCoroutine = StartCoroutine(NaturalEstinguishingCoroutine());
		
		fireEmitter = fireParticles.emission;
		mainFire = fireParticles.main;

		cindersEmitter = cindersParticles.emission;
		
	}

	private void Update()
	{
		if(!GameManager.instance.isPaused)
		{
			UpdateLight();
			UpdateVFX();
		}
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
		GameManager.instance.isDeadFire = true;
		GameManager.instance.GameOver();
		//gameover
	}

	public void UpdateVFX()
	{
		mainFire.startSize = Mathf.Lerp(fireSize.min, fireSize.max, vivacity / 100.0f);
		mainFire.gravityModifier = Mathf.Lerp(gravity.min, gravity.max, vivacity / 100.0f);

		fireEmitter.rateOverTime = Mathf.Lerp(fireEmission.min, fireEmission.max, vivacity / 100.0f);
		cindersEmitter.rateOverTime = Mathf.Lerp(minCindersEmission, maxCindersEmission, vivacity / 100.0f);

        campFireLight.range = Mathf.Lerp(campFireLightRange.min, campFireLightRange.max, vivacity / 100.0f);
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
		vivacityAfter = Mathf.Clamp(vivacityAfter, 0.0f, 100.0f);
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
