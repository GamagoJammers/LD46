using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private float Timer = 0.0f;
    float minutes;
    float seconds;
    public Text timerText;
    private bool isPaused = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            Timer += Time.deltaTime;
            seconds = Timer % 60;
            minutes = Timer / 60;
        }
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        PauseMenu();
        if (pauseMenu.activeSelf)
        {
            isPaused = true;
        }
        else if (!pauseMenu.activeSelf)
        {
            isPaused = false;
        }
    }

    private void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            pauseMenu.SetActive(false);
        }

        else if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }

    }

    public void doExit()
    {
        Application.Quit();
    }

}
