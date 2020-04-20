using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    private float timer;
    public int minRainTime;
    public int maxRainTime;
    public int thunderChance;
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

    public Light fireLight;

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
        //Debug.Log(targetTime);
        raining = true;
        fireLight.color = new Color32(109,177,248,255);
        rain.Play();
        rate = campfire.naturalEstinguishingRate;
        campfire.naturalEstinguishingRate = rate/rateAcceleration;
        rainTime = Random.Range(minRainDuration, maxRainDuration);
        //Debug.Log(RainTime);
        yield return new WaitForSeconds(rainTime);
        rain.Stop();
        fireLight.color = new Color32(163,153,255,255);
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
            woodenTreeGenerator.DestroyOne();
        }
        thunder = false;
    }
}
