using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public UI scriptTimer;
    public GameObject Timer;
    public GameObject scoreScreen;
    public Text displayScore;
    public Text displayDeathCause;
    // Start is called before the first frame update
    void Start()
    {
        scoreScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndScore()
    {
        Time.timeScale = 0.0f;
        scoreScreen.SetActive(true);
        Timer.SetActive(false);
        displayScore.text = scriptTimer.timerText.text.ToString();
        if (GameManager.instance.isDeadFire)
            displayDeathCause.text = "Your fire is extinguished, throw more logs into it!";
        else if (!GameManager.instance.isDeadFire)
            displayDeathCause.text = "Your ram is dead, try and feed him more sandwiches!";
    }
}
