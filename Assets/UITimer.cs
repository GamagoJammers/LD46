using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private float Timer = 0.0f;
    float minutes;
    float seconds;
    public Text timerText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        seconds = Timer%60;
        minutes = Timer / 60;
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
