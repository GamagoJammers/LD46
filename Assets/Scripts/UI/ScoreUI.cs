using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public UI scriptTimer;
    public GameObject Timer;
    public GameObject scoreScreen;
    public Animation animScore;
    public Text displayScore;
    public Text displayDeathCause;
    public EventSystem m_eventSys;
    public GameObject firstSelEnd;
    // Start is called before the first frame update
    void Start()
    {
        scoreScreen.SetActive(false);
        //animScore.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndScore()
    {
        Timer.SetActive(false);

        scoreScreen.SetActive(true);

        m_eventSys.SetSelectedGameObject(firstSelEnd);

        displayScore.text = scriptTimer.timerText.text.ToString();

        if (GameManager.instance.isDeadFire)
            displayDeathCause.text = "Your fire is extinguished, throw more logs into it!";
        else if (!GameManager.instance.isDeadFire)
            displayDeathCause.text = "Your ram is dead, try and feed him more sandwiches!";

        StartCoroutine (FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        animScore.Play();
        yield return new WaitUntil(()=>animScore.isPlaying==false);
        Time.timeScale = 0.0f;
    }
}
