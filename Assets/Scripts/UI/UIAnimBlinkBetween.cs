using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimBlinkBetween : MonoBehaviour
{


	public GameObject Sprite1;
	public GameObject Sprite2;
	public float timebetween;

	private float check;
	private float lastchange = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        lastchange = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
    	check = Time.time;
        if(check - lastchange >= timebetween)
        {
        	SwapImage();
        	lastchange = Time.time;
        }
    }

    void SwapImage()
    {
    	if(Sprite1.activeSelf)
    	{
    		Sprite1.SetActive(false);
    		Sprite2.SetActive(true);
    	}
    	else
    	{
    		Sprite2.SetActive(false);
    		Sprite1.SetActive(true);
    	}
    }
}
