using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public UI timer;
    public GameObject scoreScreen;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            EndScore();
        }
    }

    public void EndScore()
    {
        scoreScreen.SetActive(true);
        scoreText.text = timer.timerText.ToString();
    }
}
