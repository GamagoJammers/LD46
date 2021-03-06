﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class ThunderEvent : UnityEvent<Vector3> { }

public class RainManager : MonoBehaviour
{
    private float timer;
    public int minRainTime;
    public int maxRainTime;
    public int thunderChance;
    public int thunderLightIntensity;
    public int minRainDuration;
    public int maxRainDuration;
    public int rateAcceleration;
    private int rainTime;
    private int actualTime;
    private int targetTime;
    private float rate;
    private bool raining = false;
    private bool thunder = false;

    public WoodenTreeGenerator woodenTreeGenerator;

    public ParticleSystem rain;

    public Light thunderLight;

    public Light fireLight;

    public Campfire campfire;

    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public ThunderEvent m_thunderEvent;

    private void Awake()
    {
        m_thunderEvent = new ThunderEvent();   
    }

    private void OnDisable()
    {
        m_thunderEvent.RemoveAllListeners();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(VirtualCamera != null)
        {
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
        rain.Stop();
        targetTime = Random.Range(minRainTime, maxRainTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!raining && !GameManager.instance.isPaused)
        {
            if (actualTime >= targetTime)
            {
                StartCoroutine("Rain");
                StartCoroutine("Thunder");
            }
            else
            {
                timer += Time.deltaTime;
                actualTime = Mathf.RoundToInt(timer % 60);
            }
        }
    }

    public IEnumerator Rain()
    {
        raining = true;
        fireLight.color = new Color32(109,100,248,255);
        rain.Play();
        rate = campfire.naturalEstinguishingRate;
        campfire.naturalEstinguishingRate = rate/rateAcceleration;
        rainTime = Random.Range(minRainDuration, maxRainDuration);
        yield return new WaitForSeconds(rainTime);
        rain.Stop();
        fireLight.color = new Color32(255,117,8,255);
        campfire.naturalEstinguishingRate = rate;
        timer = 0;
        actualTime = 0;
        targetTime = Random.Range(minRainTime, maxRainTime);
        raining = false;
    }

    public IEnumerator Thunder()
    {
        if(Random.Range(1,100) <= thunderChance)
        {
            thunder = true;
        }
        yield return new WaitForSeconds(Random.Range(1,rainTime-1));
        if (thunder)
        {
            virtualCameraNoise.m_AmplitudeGain = 5;
            thunderLight.intensity = thunderLightIntensity;
            Vector3 position = woodenTreeGenerator.DestroyOne();
            m_thunderEvent.Invoke(position);
            yield return new WaitForSeconds(1);
            thunderLight.intensity = 0;
            virtualCameraNoise.m_AmplitudeGain = 0;
        }
        thunder = false;
    }

    public bool IsRaining()
    {
        return raining;
    }
}
