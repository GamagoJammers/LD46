using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    private float timer;
    public int minRainTime;
    public int maxRainTime;
    public int minRainDuration;
    public int maxRainDuration;
    public int rateAcceleration;
    private int RainTime;
    private int actualTime;
    private int targetTime;
    private float rate;
    private bool raining = false;

    public ParticleSystem rain;

    public Light light;

    public Campfire campfire;
    // Start is called before the first frame update
    void Start()
    {
        rain.Stop();
        targetTime = Random.Range(minRainTime, maxRainTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(actualTime);
        if (!raining)
        {
            if (actualTime >= targetTime)
            {
                StartCoroutine("Rain");
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
        //Debug.Log(targetTime);
        raining = true;
        light.color = new Color32(109,177,248,255);
        rain.Play();
        rate = campfire.naturalEstinguishingRate;
        campfire.naturalEstinguishingRate = rate/rateAcceleration;
        RainTime = Random.Range(minRainDuration, maxRainDuration);
        //Debug.Log(RainTime);
        yield return new WaitForSeconds(RainTime);
        rain.Stop();
        light.color = new Color32(163,153,255,255);
        campfire.naturalEstinguishingRate = rate;
        timer = 0;
        actualTime = 0;
        targetTime = Random.Range(minRainTime, maxRainTime);
        raining = false;
    }
}
